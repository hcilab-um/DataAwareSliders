using System;
using System.Runtime.InteropServices;
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
	public abstract partial class DensitySlider : Control
	{
		#region Variables

		public event EventHandler ValueChanged;

		//The following is for changing the pointer speed
		private UInt32 defaultPointerSpeed = 10;
		private UInt32 slowedPointSpeed = 1;
		public const UInt32 SPI_SETMOUSESPEED = 0x0071;
		public const UInt32 SPI_GETMOUSESPEED = 0x0070;
		[DllImport("User32.dll")]
		static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);
		[DllImport("User32.dll")]
		static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref uint pvParam, uint fWinIni);

		private GraphicsPath sliderGP = null; 
		protected GraphicsPath customSliderGP = null;
		private GraphicsPath sliderArea = null;
		protected int sliderHeight = 0;
		protected int sliderWidth = 0;
		protected float histogramMaxHeight = 0;
		protected float histogramLowerY = 0;
		protected float histogramUpperY = 0;
		protected float trackWidth = 0;
		protected float trackXStart = 0;
		protected float trackXEnd = 0;
		protected float trackYValue = 0;
		protected float spaceBetweenTicks = 0;


		protected int largestIndex = 0;
		protected int smallestIndex = 0;

		private int sliderValue = 0;
		private int centerOfRange = 0;
		private List<uint> itemsInIndices = new List<uint>(new uint[] { 100, 500, 900, 150, 330, 205, 506 }); //multipurpose. The count of this List indicates how many indices there are
		//and the value of each element indicates the number of elements associated with that index
		//defaults to two indices of size 50 each
		List<string> indexNames = null;
		private List<int> rangeOfValues;
		protected int offset = 0;
		private int pixelSensitivity = 3;

		protected bool clickedOnSlider = false;
		protected bool drawSlider = true;
		private Point lastMousePosition;

		#endregion

		#region Getters and Setters

		public int Value
		{
			get { return sliderValue; }
			set
			{
				int max = calculateMax();
				if (value >= 0 && value <= max)
				{
					sliderValue = value;
				}
				else if (value < 0)
				{
					sliderValue = 0;
				}
				else if (value > max)
				{
					sliderValue = max;
				}
				Invalidate();

				//We only update the offset when the main slider is changing it's position
				//the center of our range of values also only changes when we (re)draw the slider
				if (drawSlider)
				{
					centerOfRange = Value;
					updateOffset(Value);
				}
				

				if (ValueChanged != null)
					ValueChanged(this, new EventArgs());

				//Refresh();
			}
		}

		public List<uint> ItemsInIndices
		{
			get { return itemsInIndices; }
			set
			{
				itemsInIndices = value;
				largestIndex = findLargestIndex();
				smallestIndex = findSmallestInex();
				Invalidate();
			}
		}

		public List<int> RangeOfValues
		{
			get { return rangeOfValues; }
			protected set { rangeOfValues = value; }
		}

		public List<string> IndexNames
		{
			get { return indexNames; }
			set
			{
				indexNames = value;
			}
		}

		protected GraphicsPath SliderGP
		{
			get { return sliderGP; }
		}

		protected int PixelSensitivity
		{
			get { return pixelSensitivity; }
			set
			{
				if (value >= 1 && value % 2 == 1)
					pixelSensitivity = value;
			}
		}

		#endregion

		public DensitySlider()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
					 ControlStyles.ResizeRedraw | ControlStyles.Selectable |
					 ControlStyles.SupportsTransparentBackColor | ControlStyles.UserMouse |
					 ControlStyles.UserPaint, true); //I got this directly from another example I found on the internet


			largestIndex = findLargestIndex();
			smallestIndex = findSmallestInex();
			doPaintingMath();

			SystemParametersInfo(SPI_GETMOUSESPEED, 0, ref defaultPointerSpeed, 0);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);

			Graphics g = pe.Graphics;

			////outline client rectangle
			//Pen outlinePen = new Pen(Color.Red);
			//g.DrawRectangle(outlinePen, 0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1);

			doPaintingMath();

			

			//This following code will draw ticks and the histograms above them
			float histogramHeight;
			SolidBrush histogramBrush = new SolidBrush(Color.Blue);

			Pen blackPen = new Pen(Color.Black, 2);
			Pen redPen = new Pen(Color.Red, 2);
			int index = 0;
			for (float i = trackXStart; i < trackXEnd - 1; i += spaceBetweenTicks)
			{
				//draw ticks
				g.DrawLine(blackPen, i, trackYValue, i, trackYValue + 10); //ticks will be 10 pixels tall

				//draw histogram
				histogramHeight = getCurrHistogramHeight(index);
				g.FillRectangle(histogramBrush, i, histogramLowerY - histogramHeight, spaceBetweenTicks, histogramHeight);
				g.DrawRectangle(blackPen, i, histogramLowerY - histogramHeight, (float)(trackWidth / (float)itemsInIndices.Count), histogramHeight);

				if (indexNames != null)
				{
					g.DrawString(indexNames[index], new Font(FontFamily.GenericSerif, 10), new SolidBrush(Color.Black), i, histogramLowerY + 1);
				}

				index += 1;
			}

			//draw track
			g.DrawLine(blackPen, trackXStart, trackYValue, trackXEnd, trackYValue);

			//Deal with slider positioning
			GraphicsPath currSliderGP = sliderGP;

			if (customSliderGP == null)
			{
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
				if (drawSlider)
					sliderGP = generateSliderPath(sliderCenterX, trackYValue);

				currSliderGP = sliderGP;
			}
			else
			{
				currSliderGP = customSliderGP;
			}

			int queriedPixelX = PointToClient(Cursor.Position).X;

			if (queriedPixelX < trackXStart)
				queriedPixelX = (int)trackXStart;
			else if(queriedPixelX > trackXEnd)
				queriedPixelX = (int)trackXEnd;

			g.DrawLine(redPen, currSliderGP.GetBounds().X + currSliderGP.GetBounds().Width / 2, histogramLowerY, currSliderGP.GetBounds().X + currSliderGP.GetBounds().Width / 2, histogramLowerY - getCurrHistogramHeight(findIndexOfSliderValue()));

			g.FillPath(new SolidBrush(Color.FromArgb(128, Color.Gray)), currSliderGP);
			g.DrawPath(blackPen, currSliderGP);

			sliderArea = generateSlideArea();

		}//end paint

		protected void doPaintingMath()
		{
			sliderWidth = 30;
			sliderHeight = 15;

			//track math
			trackWidth = ClientRectangle.Width - 1 - sliderWidth;
			trackYValue = ClientRectangle.Height - sliderHeight; //the Y value of the start point of the track. The start and end points are the same since the track is a straight line. 0.2 was arbitrarily chosen
			trackXStart = ClientRectangle.X + sliderWidth / 2 + 1; //the X value of the start point of the track
			trackXEnd = ClientRectangle.Width - sliderWidth / 2; //the x value of the end point of the track

			//histogram math
			//histogramLowerY = trackYValue - sliderHeight / 2 - 3;
			histogramLowerY = trackYValue;
			histogramUpperY = ClientRectangle.Y + 1; //don't want histograms going all the way to the top of the ClientRectangle
			histogramMaxHeight = histogramLowerY - histogramUpperY;

			spaceBetweenTicks = (float)(trackWidth / (float)itemsInIndices.Count);
		}

		#region Overridden Events

		/// <summary>
		/// This method enables slider dragging
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				if (mouseInSliderRegion(e.Location))
				{
					GraphicsPath currSliderGP = sliderGP != null ? sliderGP : customSliderGP;
					int newXValue = (int)Math.Round(currSliderGP.GetBounds().X + currSliderGP.GetBounds().Width / 2);
					Cursor.Position = PointToScreen(new Point(newXValue, (int)trackYValue));
					lastMousePosition = Cursor.Position;

					Capture = true;
					clickedOnSlider = true;
					drawSlider = true;

					slowDownMouse();		
				}
				else if (sliderArea.GetBounds().Contains(e.Location))
				{
					//simulate a mouse move event
					Capture = true;
					clickedOnSlider = true;
					drawSlider = true;
					OnMouseMove(e);

					slowDownMouse();
				}
			}
		}

		/// <summary>
		/// This method allows the slider to be dragged, but only if it's been clicked on
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (Capture && clickedOnSlider && !lastMousePosition.Equals(Cursor.Position))
			{
				int effectiveXLocation;

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
					GraphicsPath currSliderGP = customSliderGP == null ? sliderGP : customSliderGP;
					for (float i = sliderWidth / 2 + spaceBetweenTicks; i < ClientRectangle.Width - 1 && !foundIndex; i += spaceBetweenTicks)
					{
						mouseIndex++;
						if (e.X < i)
							foundIndex = true;
					}

					effectiveXLocation = e.X - (int)Math.Round(trackXStart);
					effectiveXLocation += pixelSensitivity / 2 - effectiveXLocation % pixelSensitivity;
					effectiveXLocation += (int)Math.Round(trackXStart);

					if (effectiveXLocation < trackXStart)
						effectiveXLocation = (int)trackXStart;
					if (effectiveXLocation > trackXEnd)
						effectiveXLocation = (int)trackXEnd;

					if (effectiveXLocation - e.X < 0)
						Debug.Write("mouse shifted back");
					else if(effectiveXLocation - e.X == 0)
						Debug.Write("mouse not shifted");
					else
						Debug.Write("mouse shifted forward");

					Debug.WriteLine("\t{0}  {1}", effectiveXLocation, e.X);

					//Find mouse penetration
					float penetration = (effectiveXLocation - (spaceBetweenTicks * mouseIndex) - sliderWidth / 2) / spaceBetweenTicks;

					//calculate value
					int tempValue = calculateSum(mouseIndex - 1);
					tempValue += (int)(penetration * itemsInIndices[mouseIndex]);

					//Value = tempValue;
					//Through some experimentation I've found that the above code finds the value at the "top" of the pixel.
					//This code corrects for that by pretending to set the value and get the appropriate updated range and then
					//setting the true Value accordingly.
					if (tempValue > 0 && tempValue < calculateMax())
					{
						updateOffset(tempValue);
						Value = RangeOfValues[0];
					}
					else
					{
						Value = tempValue;
					}

				}
			}

			//if (redrawMouse)
			//    redrawMouse = false;

		}

		/// <summary>
		/// This method disables slider movement
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			//base.OnMouseUp(e);
			Capture = false;
			clickedOnSlider = false;
			//drawSlider = true;

			resetMouseSpeed();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			drawSlider = true;
			base.OnKeyDown(e);
			this.Focus();

			switch (e.KeyCode)
			{
				case Keys.Down:
				case Keys.Left:
					Value = Value - 1;
					break;
				case Keys.Up:
				case Keys.Right:
					Value = Value + 1;
					break;
			}
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			this.Invalidate();
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			this.Invalidate();
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
			//if (keyData == Keys.Tab)
			//    return base.ProcessDialogKey(keyData);
			//else
			//{
			//    OnKeyDown(new KeyEventArgs(keyData));
			//    return true;
			//}

			if (keyData == Keys.Left || keyData == Keys.Right || keyData == Keys.Up || keyData == Keys.Down)
			{
				OnKeyDown(new KeyEventArgs(keyData));
				return true;
			}
			if (keyData == Keys.G)
			{
				OnMouseDown(new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, (int)(customSliderGP.GetBounds().Right) - 1, (int)trackYValue, 0));
				OnMouseMove(new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, (int)(customSliderGP.GetBounds().Right) - 1, (int)trackYValue, 0));
				OnMouseUp(new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, (int)(customSliderGP.GetBounds().Right) - 1, (int)trackYValue, 0));
			}
			drawSlider = true;
			return base.ProcessDialogKey(keyData);
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// This method calculates the total number of items being mapped by the slider.
		/// This is done by looping through the itemsInIndices List and adding the value of each element
		/// </summary>
		/// <returns>An int representing the sum of values in itemsInIndices</returns>
		protected int calculateMax()
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
		private GraphicsPath generateSliderPath(float sliderCenterX, float sliderCenterY)
		{
			GraphicsPath gp = new GraphicsPath();

			float leftX, rightX, topY, bottomY;

			leftX = sliderCenterX - sliderWidth / 2;
			rightX = sliderCenterX + sliderWidth / 2;
			topY = sliderCenterY - sliderHeight / 2;
			bottomY = sliderCenterY + sliderHeight / 2;
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

		private GraphicsPath generateSlideArea()
		{
			GraphicsPath slideArea = new GraphicsPath();

			PointF topLeft = new PointF(ClientRectangle.X + 1, trackYValue - sliderHeight);
			PointF topRight = new PointF(ClientRectangle.Width - 1, trackYValue - sliderHeight);
			PointF bottomLeft = new PointF(ClientRectangle.X + 1, trackYValue + sliderHeight);
			PointF bottomRight = new PointF(ClientRectangle.Width - 1, trackYValue + sliderHeight);

			slideArea.AddLine(topLeft, topRight);
			slideArea.AddLine(topRight, bottomRight);
			slideArea.AddLine(bottomRight, bottomLeft);
			slideArea.AddLine(bottomLeft, topLeft);

			return slideArea;
		}

		/// <summary>
		/// This method finds which index the slider falls in
		/// </summary>
		/// <returns>Returns an integer indicating that index. -1 if the index could not be determined</returns>
		protected int findIndexOfSliderValue()
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

		protected int findIndexOfValue(int value)
		{
			int sumSoFar = 0;
			int result = -1;

			for (int i = 0; i < itemsInIndices.Count && result == -1; i++)
			{
				sumSoFar += (int)itemsInIndices[i];
				if (value <= sumSoFar)
					result = i;
			}

			return result;
		}

		/// <summary>
		/// Calculates the sum of values up to and including a zero based index
		/// </summary>
		/// <param name="index">The index to calculate up to. This should be zero based meaning that to calculate up to the 3rd index of the list you should pass 2</param>
		/// <returns></returns>
		protected int calculateSum(int index)
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
		public int findLargestIndex()
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
		protected int findSmallestInex()
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
		protected bool mouseInSliderRegion(Point point)
		{
			if (customSliderGP == null)
				return sliderGP.GetBounds().Contains(point);
			else
				return customSliderGP.GetBounds().Contains(point);
		}

		private void updateOffset(int value)
		{
			if (spaceBetweenTicks > 0)
			{
				float itemsPerPixel = itemsInIndices[findIndexOfValue(value)] / spaceBetweenTicks;

				if (itemsPerPixel <= (float)1)
				{
					offset = 0;
				}
				else
				{
					offset = (int)Math.Round(itemsPerPixel + 0.5);

					if (offset % 2 == 1)
						offset++;
				}

				offset *= pixelSensitivity;
			}

			this.updateRangeAroundValues(value);
		}

		protected virtual void updateRangeAroundValues(int value)
		{
			List<int> temp = new List<int>();
			for (int i = offset / 2; i > 0; i--)
			{
				if (Value - i >= 1)
					temp.Add(value - i);
			}

			temp.Add(value);

			for (int i = 1; i <= offset / 2; i++)
			{
				if (Value + i <= calculateMax())
					temp.Add(value + i);
			}

			rangeOfValues = temp;
		}

		protected float getCurrHistogramHeight(int histogramIndex)
		{
			return (float)(histogramMaxHeight * (float)((float)itemsInIndices[histogramIndex] / (float)itemsInIndices[largestIndex]));
		}

		protected void slowDownMouse()
		{
			//slow down the mouse
			SystemParametersInfo(SPI_SETMOUSESPEED, 0, slowedPointSpeed, 0);
		}

		protected void resetMouseSpeed()
		{
			//reset the speed of the mouse
			SystemParametersInfo(SPI_SETMOUSESPEED, 0, defaultPointerSpeed, 0);
		}

		#endregion
	}//end class
}
