using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace CustomSlider
{
	public partial class MultipleValueSlider : Control, IDensitySlider
	{

		public event EventHandler ValueChanged;

		#region Consants - help with customizability in the future

		private const float TRACK_BAR_PROPORTION_FROM_BOTTOM = (float)0.2;
		//private const float SLIDER_WIDTH_TRACK_BAR_WIDTH_RATIO = (float)0.05;
		private const int SLIDER_HEIGHT = 15;
		private const int DISTANCE_FROM_TRACK_TO_HISTOGRAM = 15;

		#endregion

		#region Variables

		private GraphicsPath sliderGP = null;
		private int sliderValue = 50;
		private int offset = 0;
		private int baseOffset = 1;
		private List<uint> itemsInIndices = new List<uint>(new uint[] { 100, 500, 900, 150, 330, 205, 506 }); //multipurpose. The count of this List indicates how many indices there are
		//and the value of each element indicates the number of elements associated with that index
		//defaults to two indices of size 50 each
		private List<int> rangeOfValues = new List<int>(new int[] { 50 });
		private int largestIndex = 0;
		private int smallestIndex = 0;
		private bool clickedOnSlider = false;
		private float spaceBetweenTicks = 0;
		

		#endregion

		#region Getters and Setters

		public int Value
		{
			get { return sliderValue; }
			set
			{
				int max = calculateMax();
				if (value >= 0 && value <= calculateMax())
				{
					sliderValue = value;
				}
				else if (value < 0)
				{
					sliderValue = 0;
				}
				else if (value > calculateMax())
				{
					sliderValue = calculateMax();
				}
				Invalidate();
				updateOffset();

				if (ValueChanged != null)
					ValueChanged(this, new EventArgs());
			}
		}

		public List<uint> ItemsInIndices
		{
			get { return itemsInIndices; }
			set
			{
				itemsInIndices = value;
				largestIndex = findLargestIndex();
				Invalidate();
			}
		}

		public List<int> RangeOfValues
		{
			get { return rangeOfValues; }
			private set { rangeOfValues = value; }
		}

		#endregion

		public MultipleValueSlider()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
					 ControlStyles.ResizeRedraw | ControlStyles.Selectable |
					 ControlStyles.SupportsTransparentBackColor | ControlStyles.UserMouse |
					 ControlStyles.UserPaint, true); //I got this directly from another example I found on the internet

			SetStyle(ControlStyles.Selectable, true);

			this.TabStop = true;
			largestIndex = findLargestIndex();
			smallestIndex = findSmallestInex();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Graphics g = e.Graphics;

			//outline client rectangle
			Pen outlinePen = new Pen(Color.Red);
			//g.DrawRectangle(outlinePen, 0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1);

			//draw track

			/*Big comment coming:
			 * Since I'm making a slider I want my slider's size to change as a function of the width of the track bar.
			 * This means that a larger track bar (graphically larger) will have a wider slider.
			 * The center of my slider will be what determines it's value.
			 * This raises an issue: how is the slider drawn at the ends of the trackbar? Should it be cut off and only partially drawn?
			 * I decided that my track bar should be drawn smaller to accomodate a full slider at the ends. This means that
			 * each end of the track bar is half the width of the slider shorter than the full length.
			 * 
			 * This needs some algebra because the width of the slider depends on the width of the trackbar and the width of the track bar
			 * depends on the width of the slider.
			 * 
			 * Here's my algebra:
			 * Let r = width of the slider
			 * Let w = width of the track bar
			 * Let CRW = width of ClientRectangle (we always know this)
			 * 
			 * w = CRW - r (width of trackbar is the width of the client rectangle minus the width of the slider)
			 * and
			 * r = (0.05w) (width of slider is 1/20 (a value I chose) the width of trackbar)
			 * 
			 * w = CRW - 0.05w
			 * 1.05w = CRW
			 * w = CRW/1.05
			 * 
			 */
			//float trackWidth = (float)ClientRectangle.Width / (float)(1 + SLIDER_WIDTH_TRACK_BAR_WIDTH_RATIO);
			//float sliderWidth = (float)SLIDER_WIDTH_TRACK_BAR_WIDTH_RATIO * trackWidth;
			int sliderWidth = 30;
			int trackWidth = (int)(ClientRectangle.Width - 1 - sliderWidth);

			int trackYValue = (int)(ClientRectangle.Height - TRACK_BAR_PROPORTION_FROM_BOTTOM * ClientRectangle.Height); //the Y value of the start point of the track. The start and end points are the same since the track is a straight line. 0.2 was arbitrarily chosen
			int trackXStart = ClientRectangle.X + sliderWidth / 2 + 1; //the X value of the start point of the track
			int trackXEnd = ClientRectangle.Width - sliderWidth / 2; //the x value of the end point of the track

			Pen blackPen = new Pen(Color.Black, 2);
			g.DrawLine(blackPen, trackXStart, trackYValue, trackXEnd, trackYValue);

			//This following code will draw ticks and the histograms above them
			int histogramLowerY = trackYValue - SLIDER_HEIGHT / 2 - 3;
			int histogramUpperY = ClientRectangle.Y + 1; //don't want histograms going all the way to the top of the ClientRectangle
			int histogramMaxHeight = histogramLowerY - histogramUpperY;
			float histogramHeight;
			SolidBrush histogramBrush = new SolidBrush(Color.Blue);
			int index = 0;
			spaceBetweenTicks = (float)(trackWidth / (float)itemsInIndices.Count);

			for (float i = trackXStart; i < trackXEnd - 1; i += spaceBetweenTicks)
			{
				//draw ticks
				g.DrawLine(blackPen, i, trackYValue, i, trackYValue + 10); //ticks will be 10 pixels tall

				//draw histogram
				histogramHeight = (float)(histogramMaxHeight * (float)((float)itemsInIndices[index] / (float)itemsInIndices[largestIndex]));
				g.FillRectangle(histogramBrush, i, histogramLowerY - histogramHeight, spaceBetweenTicks, histogramHeight);
				g.DrawRectangle(blackPen, i, histogramLowerY - histogramHeight, (float)(trackWidth / (float)itemsInIndices.Count), histogramHeight);

				////draw size # of items in index above histogram
				//string toDraw = "" + itemsInIndices[index];
				//Font font = new Font("Arial", 8);
				//g.DrawString(toDraw, font, new SolidBrush(Color.White), i, histogramLowerY - histogramHeight + 1);

				index += 1;
			}

			//Deal with slider positioning
			int indexOfSlider = findIndexOfSliderValue();
			float sliderCenterX = trackXStart;
			float proportion;
			if (indexOfSlider != -1)
			{
				//This will make the x value of the slider go to the right tick. From here it will be shifted over more
				//based on it's value and the number of items assocaited with that index
				for (int i = 0; i < indexOfSlider; i++)
				{
					sliderCenterX += spaceBetweenTicks;
				}

				proportion = ((float)(sliderValue - calculateSum(indexOfSlider - 1)) / (float)itemsInIndices[indexOfSlider]);
				sliderCenterX += proportion * (spaceBetweenTicks);
			}

			//if (Focused)
			//{
			//    string toDraw = "I'm focused";
			//    Font font = new Font("Arial", 8);
			//    g.DrawString(toDraw, font, new SolidBrush(Color.Black), 2, 2);
			//}

			//draw the slider
			SolidBrush sliderBrush = new SolidBrush(Color.Gray);
			sliderGP = generateSliderPath(sliderCenterX, trackYValue, sliderWidth);
			g.FillPath(sliderBrush, sliderGP);

			//outline slider
			g.DrawPath(blackPen, sliderGP);

			//if (clickedOnSlider)
			//{
			//    this.Cursor = new Cursor(Cursor.Current.Handle);
			//    Point tempPoint = new Point((int)sliderCenterX, (int)trackYValue);
			//    Cursor.Position = PointToScreen(tempPoint);
			//}
		}

		#region Overridden Events

		/// <summary>
		/// This method enables slider dragging
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left && mouseInSliderRegion(e.Location))
			{
				Capture = true;
				clickedOnSlider = true;
			}
		}

		/// <summary>
		/// This method allows the slider to be dragged, but only if it's been clicked on
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (Capture && clickedOnSlider)
			{
				if (e.X < 0)
					Value = 0;
				else if (e.X > ClientRectangle.Width)
					Value = calculateMax();
				else //The mouse is somewhere between the beginning and end of the track
				{
					//Need to find the index of the mouse (which histogram is the mouse under?)
					//Then find out how far "into" the index the mouse is. Is it 1/4 past the beginning of the index? 1/2 way? 3/17?
					//Based on the index of the mosue and it's "penetration" into that index I can calculate a value

					//Find index of mouse
					bool foundIndex = false;
					int mouseIndex = -1;
					for (float i = sliderGP.GetBounds().Width / 2 + spaceBetweenTicks; i < ClientRectangle.Width - 1 && !foundIndex; i += spaceBetweenTicks)
					{
						mouseIndex++;
						if (e.X < i)
							foundIndex = true;
					}

					//Find mouse penetration
					float penetration = (e.X - (spaceBetweenTicks * mouseIndex) - sliderGP.GetBounds().Width / 2) / spaceBetweenTicks;
					//penetration -= (sliderGP.GetBounds().Width / 2) / spaceBetweenTicks;

					//calculate value
					int tempValue = calculateSum(mouseIndex - 1);
					tempValue += (int)(penetration * itemsInIndices[mouseIndex]);

					Value = tempValue;

				}
			}

		}

		/// <summary>
		/// This method disables slider movement
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			Capture = false;
			clickedOnSlider = false;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			this.Focus();
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
		}

		protected override void OnEnter(EventArgs e)
		{
			this.Invalidate();
			base.OnEnter(e);
		}

		protected override void OnLeave(EventArgs e)
		{
			this.Invalidate();
			base.OnLeave(e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);

			switch (e.KeyCode)
			{
				case Keys.Left:
					Value = Value - 1;
					break;
				case Keys.Right:
					Value = Value + 1;
					break;
			}
		}

		/// <summary>
		/// Processes a dialog key.
		/// </summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys"></see> values that represents the key to process.</param>
		/// <returns>
		/// true if the key was processed by the control; otherwise, false.
		/// </returns>
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Tab)
				return base.ProcessDialogKey(keyData);
			else
			{
				OnKeyDown(new KeyEventArgs(keyData));
				return true;
			}
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// This method calculates the total number of items being mapped by the slider.
		/// This is done by looping through the itemsInIndices List and adding the value of each element
		/// </summary>
		/// <returns>An int representing the sum of values in itemsInIndices</returns>
		private int calculateMax()
		{
			return calculateSum(itemsInIndices.Count - 1);
		}

		/// <summary>
		/// This method generates a graphicsPath for the slider
		/// </summary>
		/// <param name="sliderCenterX">The x value of the slider (left edge location)</param>
		/// <param name="sliderCenterY">the center of the slider heightwise</param>
		/// <param name="sliderWidth">the width of the slider</param>
		/// <returns>A rectangular graphics path</returns>
		private GraphicsPath generateSliderPath(float sliderCenterX, float sliderCenterY, float sliderWidth)
		{
			GraphicsPath gp = new GraphicsPath();

			float leftX, rightX, topY, bottomY;

			leftX = sliderCenterX - sliderWidth / 2;
			rightX = sliderCenterX + sliderWidth / 2;
			topY = sliderCenterY - SLIDER_HEIGHT / 2;
			bottomY = sliderCenterY + SLIDER_HEIGHT / 2;
			// slider starts as nothing


			gp.AddLine(leftX, topY, leftX, bottomY);
			//after this line we have:
			//
			// |

			gp.AddLine(leftX, bottomY, rightX, bottomY);
			//after this line we have:
			//
			// |_

			gp.AddLine(rightX, bottomY, rightX, topY);
			//after this line we have:
			//
			// |_|

			gp.AddLine(rightX, topY, leftX, topY);
			//after this line we have:
			//  _
			// |_|

			return gp;

		}

		/// <summary>
		/// This method finds which index the slider falls in
		/// </summary>
		/// <returns>Returns an integer indicating that index. -1 if the index could not be determined</returns>
		private int findIndexOfSliderValue()
		{
			int sumSoFar = 0;
			int result = -1;

			for (int i = 0; i < itemsInIndices.Count && result == -1; i++)
			{
				sumSoFar += (int)itemsInIndices[i];
				if (sliderValue <= sumSoFar)
					result = i;
			}

			return result;
		}

		/// <summary>
		/// Calculates the sum of values up to and including a zero based index
		/// </summary>
		/// <param name="index">The index to calculate up to. This should be zero based meaning that to calculate up to the 3rd index of the list you should pass 2</param>
		/// <returns></returns>
		private int calculateSum(int index)
		{
			int sum = 0;
			if (index < 0)
				return 0;
			else
			{
				for (int i = 0; i <= index; i++)
				{
					sum += (int)itemsInIndices[i];
				}

				return sum;
			}
		}

		/// <summary>
		/// Finds the index with that maps the largest number of items
		/// </summary>
		/// <returns>The value of the index that maps the largest number of items</returns>
		private int findLargestIndex()
		{
			int largest = 0;

			for (int i = 0; i < itemsInIndices.Count; i++)
			{
				if (itemsInIndices[i] > itemsInIndices[largest])
					largest = i;
			}

			return largest;
		}

		/// <summary>
		/// Finds the index with that maps the smallest number of items
		/// </summary>
		/// <returns>The value of the index that maps the smallest number of items</returns>
		private int findSmallestInex()
		{
			int smallest = 0;

			for (int i = 0; i < itemsInIndices.Count; i++)
			{
				if (itemsInIndices[i] < itemsInIndices[smallest])
					smallest = i;
			}

			return smallest;
		}

		/// <summary>
		/// This method checks to see if a certain point is within the bounds of the slider
		/// </summary>
		/// <param name="point">The point that is being checked</param>
		/// <returns>Returns true if the point is in the slider region, false otherwise</returns>
		private bool mouseInSliderRegion(Point point)
		{
			return sliderGP.GetBounds().Contains(point);
		}

		private void updateOffset()
		{
			if (spaceBetweenTicks > 0)
				offset = ((int)Math.Round(baseOffset * itemsInIndices[smallestIndex] / spaceBetweenTicks * itemsInIndices[findIndexOfSliderValue()] / itemsInIndices[smallestIndex],MidpointRounding.AwayFromZero) - 1);

			updateRangeOfValues();
		}

		private void updateRangeOfValues()
		{
			List<int> temp = new List<int>();
			for (int i = offset; i > 0; i--)
			{
				if (Value - i >= 1)
					temp.Add(Value - i);
			}

			temp.Add(Value);

			for (int i = 1; i <= offset; i++)
			{
				if (Value + i <= calculateMax())
					temp.Add(Value + i);
			}

			rangeOfValues = temp;
		}

		#endregion
	}
}
