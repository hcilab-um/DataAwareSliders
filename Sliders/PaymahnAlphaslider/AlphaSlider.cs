using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace CustomSlider
{
	[DefaultEvent("Scroll")]
	public class AlphaSlider : Control, IDensitySlider
	{
		#region Events

		public event EventHandler ValueChanged;

		#endregion

		#region Consants - help with customizability in the future

		private const float TRACK_BAR_PROPORTION_FROM_BOTTOM = (float)0.2;
		//private const float SLIDER_WIDTH_TRACK_BAR_WIDTH_RATIO = (float)0.05;
		private const int SLIDER_HEIGHT = 15;
		private const int DISTANCE_FROM_TRACK_TO_HISTOGRAM = 15;

		#endregion

		#region Variables
		GraphicsPath sliderGP = null;
		private int sliderValue = 50;
		private List<uint> itemsInIndices = new List<uint>(new uint[] { 100, 500, 900, 150, 330, 205, 506 }); //multipurpose. The count of this List indicates how many indices there are
		//and the value of each element indicates the number of elements associated with that index
		//defaults to two indices of size 50 each
		private int largestIndex;

		private int baseFineThreshold = 5;
		private int fineThreshold = 2;				//how far the mouse has to move when dragging the "fine" portion before the value of the slider is changed
		private int fineValueChange = 1;
		private int baseMediumThreshold = 1;
		private int mediumThreshold = 1;			//same as above but for the "medium" portion of the slider
		private int mediumValueChange = 10;
		
		private int lastX;							//previous x value. When checking to see if we've crossed a threshold the current mouse position is compared against this value
		private bool clickedOnSlider = false;		//if the user performs a mouse down on the slider this will be true. Will be set to false when upon mouse up
		private bool clickedOnTopHalf = false;		//bool value indicating if the user mouse down-ed on the top half of slider
		private bool clickedOnBottomHalf = false;	//bool value indicating if the user mouse down-ed on the bottom half of slider
		private bool densityAware = true;
		private bool drawHistograms = true;
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
				if (ValueChanged != null)
					ValueChanged(this, new EventArgs());
				Invalidate();

			}
		}

		public List<uint> ItemsInIndices
		{
			get { return itemsInIndices; }
			set 
			{ 
				itemsInIndices = value;
				largestIndex = findLargestIndex();
				Invalidate(); }
		}

		public int BaseFineThreshold
		{
			get { return baseFineThreshold; }
			set { baseFineThreshold = value; }
		}

		public int BaseMediumThreshold
		{
			get { return baseMediumThreshold; }
			set { baseMediumThreshold = value; }
		}

		public int FineThreshold
		{
			get { return fineThreshold; }
		}

		public int MediumThreshold
		{
			get { return mediumThreshold; }
		}

		public bool DensityAware
		{
			get { return densityAware; }
			set
			{ 
				densityAware = value;
				mediumThreshold = baseMediumThreshold;
				fineThreshold = baseFineThreshold;
				Value = sliderValue;
			}
		}

		public bool DrawHistograms
		{
			get { return drawHistograms; }
			set { drawHistograms = value; Invalidate(); }
		}

		#endregion

		public AlphaSlider()
		{
			//InitializeComponent();
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
					 ControlStyles.ResizeRedraw | ControlStyles.Selectable |
					 ControlStyles.SupportsTransparentBackColor | ControlStyles.UserMouse |
					 ControlStyles.UserPaint, true); //I got this directly from another example I found on the internet

			largestIndex = findLargestIndex();
		}


		#region Paint

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
			int trackXStart = ClientRectangle.X + sliderWidth/2 + 1; //the X value of the start point of the track
			int trackXEnd = ClientRectangle.Width - sliderWidth/2; //the x value of the end point of the track
			

			Pen blackPen = new Pen(Color.Black, 2);
			g.DrawLine(blackPen, trackXStart, trackYValue, trackXEnd, trackYValue);

			//This following code will draw ticks and the histograms above them
			int histogramLowerY = trackYValue - SLIDER_HEIGHT / 2 - 3;
			int histogramUpperY = ClientRectangle.Y + 1; //don't want histograms going all the way to the top of the ClientRectangle
			int histogramMaxHeight = histogramLowerY - histogramUpperY;
			float histogramHeight;
			SolidBrush histogramBrush = new SolidBrush(Color.Blue);
			int index = 0;

			for (float i = trackXStart; i < trackXEnd - 1; i += (float)(trackWidth / (float)itemsInIndices.Count))
			{
			    //draw ticks
			    g.DrawLine(blackPen, i, trackYValue, i, trackYValue + 10); //ticks will be 10 pixels tall

			    //draw histogram
				if (drawHistograms)
				{
					//histogramHeight = histogramMaxHeight * (float)((float)itemsInIndices[index] / (float)calculateMax()); //max possible height of histogram times the proportion of items in current index
					histogramHeight = (float)(histogramMaxHeight * (float)((float)itemsInIndices[index] / (float)itemsInIndices[largestIndex]));
					g.FillRectangle(histogramBrush, i, histogramLowerY - histogramHeight, (int)(trackWidth / (float)itemsInIndices.Count), histogramHeight);
					g.DrawRectangle(blackPen, i, histogramLowerY - histogramHeight, (float)(trackWidth / (float)itemsInIndices.Count), histogramHeight);

					////draw size # of items in index above histogram
					//string toDraw = "" + itemsInIndices[index];
					//Font font = new Font("Arial", 8);
					//g.DrawString(toDraw, font, new SolidBrush(Color.White), i, histogramLowerY - histogramHeight + 1);
				}

				index += 1;
			}

			//if (Focused)
			//{
			//    string toDraw = "I'm focused";
			//    Font font = new Font("Arial", 8);
			//    g.DrawString(toDraw, font, new SolidBrush(Color.Black), 2, 2);
			//}

			//Deal with slider positioning
			int indexOfSlider = findIndexOfSliderValue();
			int sliderX = trackXStart - sliderWidth/2;
			float proportion;
			if (indexOfSlider != -1)
			{
				//This will make the x value of the slider go to the right tick. From here it will be shifted over more
				//based on it's value and the number of items assocaited with that index
				for (int i = 0; i < indexOfSlider; i++)
				{
					sliderX += (int)(trackWidth / (float)itemsInIndices.Count);
				}

				proportion = ((float)(sliderValue - calculateSum(indexOfSlider - 1)) / (float)itemsInIndices[indexOfSlider]);
				sliderX += (int)(proportion * ((float)(trackWidth / (float)itemsInIndices.Count)));
			}

			//draw the slider
			SolidBrush sliderBrush = new SolidBrush(Color.Gray);
			sliderGP = generateSliderPath(sliderX, trackYValue, sliderWidth);
			g.FillPath(sliderBrush, sliderGP);

			//outline slider
			g.DrawPath(blackPen, sliderGP);
			g.DrawLine(blackPen, sliderX, trackYValue, sliderX + sliderWidth, trackYValue);

			//Redraw mouse pointer over slider
			if (clickedOnSlider)
			{
				this.Cursor = new Cursor(Cursor.Current.Handle);
				Point tempPoint = new Point();
				tempPoint.X = (int)(sliderX + sliderWidth / 2);
				if (clickedOnTopHalf)
					tempPoint.Y = (int)trackYValue - (int)SLIDER_HEIGHT / 4;
				else if (clickedOnBottomHalf)
					tempPoint.Y = (int)trackYValue + (int)SLIDER_HEIGHT / 4;

				Cursor.Position = PointToScreen(tempPoint);
				lastX = tempPoint.X;
			}
		}

		#endregion

		#region Overridden events

		/// <summary>
		/// This method enables slider dragging
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left && mouseInSliderRegion(e.Location))
			{
				//Debug.WriteLine("Clicked inside mouse region");
				lastX = e.Location.X;
				Capture = true;
				clickedOnSlider = true;

				if (mouseInTopHalf(e.Location))
				{
					clickedOnTopHalf = true;
					clickedOnBottomHalf = false;
				}
				else
				{
					clickedOnBottomHalf = true;
					clickedOnTopHalf = false;
				}

				OnMouseMove(e);
			}
		}

		/// <summary>
		/// This method allows the slider to be dragged, but only if it's been clicked on
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (Capture && clickedOnSlider && (clickedOnTopHalf || clickedOnBottomHalf))
			{
				//Debug.WriteLine("Moving mouse in region! {0}", e.Location);
				if(densityAware)
					updateThresholds();

				int valueChangeOnMove;
				int moveThreshold;

				if (clickedOnTopHalf)
				{
					valueChangeOnMove = mediumValueChange;
					moveThreshold = mediumThreshold;
				}
				else
				{
					valueChangeOnMove = fineValueChange;
					moveThreshold = fineThreshold;
				}

				if ((e.X - lastX) >= moveThreshold)
				{
					/*The reason for this loop and the only just below it takes a bit of explaining:
					 * Mouse movement is not immediately captured. What I mean by this is that based on how quickly the move is physcially moved
					 * the value of it's position when this event is entered could be different.
					 * 
					 * To illustrate this a bit:
					 * If the user jerks the mouse the pointer might move 100 pixels and only register a 
					 * single event instead of 10 events that are expected.
					 * This means that we will enter this code once with 
					 * e.X = 756 and lastX = 656
					 * instead of coming here 10 times with
					 * e.X = 666 and lastX = 656 followed by
					 * e.x = 676 and lastX = 666 followed by
					 * e.X = 686 and lastX = 676 followed by
					 * ...
					 * e.X = 756 and lastX = 746
					 * 
					 * In the event that e.X = 756 and lastX = 656 happens I just simulate the 10 expected events to occur
					 * by appropriately changing the slider value as many times as necessary
					 * 
					 * This helps smooth out movement and make the slider more predictable
					 */
					for (int i = lastX; i < e.X; i += moveThreshold)
					{
						Value = sliderValue + valueChangeOnMove;
					}
					lastX = e.X;
				}
				else if ((e.X - lastX) <= -1 * moveThreshold)
				{
					for (int i = lastX; i > e.X; i -= moveThreshold)
					{
						Value = sliderValue - valueChangeOnMove;
					}

					lastX = e.X;
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
			clickedOnTopHalf = false;
			clickedOnBottomHalf = false;
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
		/// This method checks to see if a point is in the slider's top half. It is assumed that the point has already been determined to be within the bounds of the slider.
		/// </summary>
		/// <param name="point">The point to be checked</param>
		/// <returns>Returns true if the point is in the top half of the slider. False if it isn't (this means that it must be in the bottom half)</returns>
		private bool mouseInTopHalf(Point point)
		{
			RectangleF sliderRectangle = sliderGP.GetBounds();

			return point.Y <= sliderRectangle.Top + sliderRectangle.Height / 2;
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

		/// <summary>
		/// This method updates the thresholds for fine and medium granularity dragging.
		/// </summary>
		private void updateThresholds()
		{
			int indexOfSlider = findIndexOfSliderValue();

			if (indexOfSlider >= 0)
			{
				double ratio = (double)itemsInIndices[indexOfSlider] / (double)itemsInIndices[largestIndex];
				mediumThreshold = (int)Math.Round(baseMediumThreshold / (ratio));
				//fineThreshold = (int)Math.Round(baseFineThreshold * (1 + 3*Math.Pow(ratio,3.0)));
                fineThreshold = 1;
			}
		}

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
		/// This method generates a graphicsPath for the slider
		/// </summary>
		/// <param name="sliderX">The x value of the slider (left edge location)</param>
		/// <param name="sliderCenterY">the center of the slider heightwise</param>
		/// <param name="sliderWidth">the width of the slider</param>
		/// <returns>A rectangular graphics path</returns>
		private GraphicsPath generateSliderPath(float sliderX, float sliderCenterY, float sliderWidth)
		{
			GraphicsPath gp = new GraphicsPath();

			float leftX, rightX, topY, bottomY;

			leftX = sliderX;
			rightX = sliderX + sliderWidth;
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

		#endregion
	}
}
