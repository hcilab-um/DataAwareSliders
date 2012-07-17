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
	public partial class HistogramSlider : Control, IDensitySlider
	{
		#region Events

		public event EventHandler ValueChanged;

		#endregion

		#region Consants - help with customizability in the future

		private const float TRACK_BAR_PROPORTION_FROM_BOTTOM = (float)0.2;
		private const float SLIDER_WIDTH_TRACK_BAR_WIDTH_RATIO = (float)0.05;
		private const int SLIDER_HEIGHT = 15;
		private const int DISTANCE_FROM_TRACK_TO_HISTOGRAM = 15;
		private const int SECONARY_SLIDER_WIDTH = 10;

		#endregion

		#region Variables
		GraphicsPath sliderGP = null;
		GraphicsPath secondarySliderGP = null;
		private int sliderValue = 50;
		private List<uint> itemsInIndices = new List<uint>(new uint[] { 100, 500, 900, 150, 330, 205, 506 }); //multipurpose. The count of this List indicates how many indices there are
		//and the value of each element indicates the number of elements associated with that index
		//defaults to two indices of size 50 each
		private int largestIndex = 0;
		private int lastX = 0;							//previous x value. When checking to see if we've crossed a threshold the current mouse position is compared against this value
		private int lastY = 0;
		private bool clickedOnSlider = false;
		private bool clickedOnSecondarySlider = false;
		private float spaceBetweenTicks = 0;
        private int histogramMaxHeight = 0;
        private int histogramLowerY;
        private int histogramUpperY;


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
				Invalidate();
			}
		}

		#endregion

		public HistogramSlider()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
					 ControlStyles.ResizeRedraw | ControlStyles.Selectable |
					 ControlStyles.SupportsTransparentBackColor | ControlStyles.UserMouse |
					 ControlStyles.UserPaint, true); //I got this directly from another example I found on the internet

			ItemsInIndices = new List<uint>(new uint[] { 100, 500, 900, 150, 330, 205, 506 });
			
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics g = e.Graphics;

			//outline client rectangle
			Pen outlinePen = new Pen(Color.Red);
			//g.DrawRectangle(outlinePen, 0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1);

			//calculate track
			int trackYValue = (int)(ClientRectangle.Height - TRACK_BAR_PROPORTION_FROM_BOTTOM * ClientRectangle.Height); //the Y value of the start point of the track. The start and end points are the same since the track is a straight line. 0.2 was arbitrarily chosen
			int trackXStart = ClientRectangle.X + 1;
			int trackXEnd = ClientRectangle.Width - 1;
			int trackWidth = trackXEnd - trackXStart;
			spaceBetweenTicks = trackWidth / (float)itemsInIndices.Count;

			//draw track
			Pen blackPen = new Pen(Color.Black, 2);
			g.DrawLine(blackPen, trackXStart, trackYValue, trackXEnd, trackYValue);

			//This following code will draw ticks and the histograms above them
			histogramLowerY = trackYValue - SLIDER_HEIGHT / 2 - 5;
			histogramUpperY = ClientRectangle.Y + 1 + 5; //don't want histograms going all the way to the top of the ClientRectangle
			histogramMaxHeight = histogramLowerY - histogramUpperY;
			float histogramHeight;
			float tickPosition;
			SolidBrush histogramBrush = new SolidBrush(Color.Blue);
			SolidBrush activeHistogramBrush = new SolidBrush(Color.Red);
			int index = 0;
			int indexOfSlider = findIndexOfSliderValue();

			for (float i = trackXStart; i < trackXEnd - 1; i += spaceBetweenTicks)
			{
				//draw ticks
				tickPosition = i + spaceBetweenTicks / 2;
				g.DrawLine(blackPen, tickPosition, trackYValue, tickPosition, trackYValue + 10); //ticks will be 10 pixels tall

				//determine histogram height and draw histogram
				histogramHeight = (float)(histogramMaxHeight * (float)((float)itemsInIndices[index] / (float)itemsInIndices[largestIndex]));
				if (index == indexOfSlider)
				{
					g.FillRectangle(activeHistogramBrush, i + SECONARY_SLIDER_WIDTH, histogramLowerY - histogramHeight, spaceBetweenTicks - SECONARY_SLIDER_WIDTH, histogramHeight);
					g.DrawRectangle(blackPen, i + SECONARY_SLIDER_WIDTH, histogramLowerY - histogramHeight, spaceBetweenTicks - SECONARY_SLIDER_WIDTH, histogramHeight);
				}
				else
				{
					g.FillRectangle(histogramBrush, i, histogramLowerY - histogramHeight, spaceBetweenTicks, histogramHeight);
					g.DrawRectangle(blackPen, i, histogramLowerY - histogramHeight, spaceBetweenTicks, histogramHeight);
				}

				////draw size # of items in index inside histogram
				//string toDraw = "" + itemsInIndices[index];
				//Font font = new Font("Arial", 8);
				//g.DrawString(toDraw, font, new SolidBrush(Color.White), i, histogramLowerY - histogramHeight + 1);

				index += 1;
			}

			//if (Focused)
			//{
			//    string toDraw = "I'm focused";
			//    Font font = new Font("Arial", 8);
			//    g.DrawString(toDraw, font, new SolidBrush(Color.Black), 2, 2);
			//}

			//Deal with slider positioning
			float sliderHorizontalCenter = spaceBetweenTicks / 2;
			for (int i = 0; i < indexOfSlider; i++)
			{
				sliderHorizontalCenter += spaceBetweenTicks;
			}
			float sliderWidth = spaceBetweenTicks - 1;

			//draw the slider
			SolidBrush sliderBrush = new SolidBrush(Color.Gray);
			sliderGP = generateSliderPath(sliderHorizontalCenter - sliderWidth / 2, trackYValue, sliderWidth);
			g.FillPath(sliderBrush, sliderGP);

			//outline slider
			g.DrawPath(blackPen, sliderGP);

			//deal with secondary slider positioning
			histogramHeight = (int)(histogramMaxHeight * (float)((float)itemsInIndices[indexOfSlider] / (float)itemsInIndices[largestIndex]));
			float secondarySliderVerticalCenter = histogramLowerY - ((float)(sliderValue - calculateSum(findIndexOfSliderValue() - 1)) / (float)itemsInIndices[findIndexOfSliderValue()]) * histogramHeight;
			float secondarySliderX = sliderHorizontalCenter - sliderWidth / 2;
			float secondarySliderWidth = 10;
			float secondarySliderHeight = 10;

			//draw secondarySlider
			secondarySliderGP = generateSecondarySliderPath(secondarySliderVerticalCenter, secondarySliderX, secondarySliderWidth, secondarySliderHeight);
			g.FillPath(sliderBrush, secondarySliderGP);
			g.DrawPath(blackPen, secondarySliderGP);

			//Redraw mouse pointer over slider
			if (clickedOnSlider)
			{
                //this.Cursor = new Cursor(Cursor.Current.Handle);
                //Point newPoint = PointToScreen(new Point((int)sliderHorizontalCenter, (int)trackYValue));
                //Cursor.Position = newPoint;
                //lastX = (int)sliderHorizontalCenter;
			}
			else if (clickedOnSecondarySlider)
			{
				this.Cursor = new Cursor(Cursor.Current.Handle);
				Point newPoint = PointToScreen(new Point((int)(secondarySliderX + secondarySliderWidth / 2), (int)secondarySliderVerticalCenter));
				Cursor.Position = newPoint;
				lastY = (int)secondarySliderVerticalCenter;
			}


		}

		#region Mouse events

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseClick(e);
			if (e.Button == MouseButtons.Left)
			{
				if (mouseInSliderRegion(e.Location))
				{
					lastX = e.X;
					Capture = true;
					clickedOnSlider = true;
					OnMouseMove(e);
				}
				else if (mouseInSecondarySliderRegion(e.Location))
				{
					lastY = e.Y;
					Capture = true;
					clickedOnSlider = false;
					clickedOnSecondarySlider = true;
					OnMouseMove(e);
				}
			}
		}

        Point lastHandledLocation = new Point(-1, -1);
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
            //To be honest, I'm not entirely sure why I need this outer if statement. It has something to do with
			//mousemove events being queued. By having this statement the the line after it a bug is solved.
            if (lastHandledLocation == (new Point(-1, -1)) || (lastHandledLocation != e.Location))
            {
                lastHandledLocation = e.Location;
                if (Capture && (clickedOnSlider || clickedOnSecondarySlider))
                {
                    int moveThreshold;
                    if (clickedOnSlider && !clickedOnSecondarySlider)
                    {
                        //if the mouse is to the left of the slider, go to preivous index
                        if (e.X < sliderGP.GetBounds().Location.X)
                        {
							if (findIndexOfSliderValue() > 0) //prevents mouse from jumping back to slider when the slider is at the beginning and the mouse is dragged away
							{
								int temp = calculateSum(findIndexOfSliderValue() - 2);
								Value = temp + (int)itemsInIndices[findIndexOfSliderValue() - 1] / 2;
							}
                        }
                        //if the mouse is to the right of the slider, go to the next index
                        else if (e.X > (sliderGP.GetBounds().Location.X + sliderGP.GetBounds().Width))
                        {
                            if (findIndexOfSliderValue() < itemsInIndices.Count - 1) //prevents mouse from jumping back to slider when the slider is at the end and the mouse is dragged away
                            {
                                int currIndex = findIndexOfSliderValue();
                                int nextIndex = currIndex + 1;
                                Value = calculateSum(currIndex) + (int)itemsInIndices[nextIndex] / 2;
                            }

                        }
                        else //other the mouse is in the bounds of the slider
                        {
							float proportion;
							int tempValue;
							int addition;

							float currHistogramHeight = histogramMaxHeight * (float)((float)itemsInIndices[findIndexOfSliderValue()] / (float)itemsInIndices[largestIndex]);

							if (e.Y > histogramLowerY)
								proportion = 0;
							else if (e.Y < histogramMaxHeight - currHistogramHeight)
								proportion = 1;
							else
							{
								proportion = 1 - (e.Y - (histogramLowerY - currHistogramHeight)) / currHistogramHeight;
							}

							tempValue = calculateSum(findIndexOfSliderValue() - 1) + 1;
							addition = (int)(proportion * itemsInIndices[findIndexOfSliderValue()]);

							if (addition >= 0 && addition <= itemsInIndices[findIndexOfSliderValue()])
							{
								tempValue += addition;

								if (proportion == 1)
									tempValue--;

								Value = tempValue;
							}
							
                        }
                        
                    }
                    else if (!clickedOnSlider && clickedOnSecondarySlider)
                    {
                        moveThreshold = 1;
                        if ((e.Y - lastY) >= moveThreshold)
                        {
                            int temp = Value - 1;
                            if (temp <= calculateSum(findIndexOfSliderValue()) && temp > calculateSum(findIndexOfSliderValue() - 1))
                            {
                                Value = Value - 1;
                                lastY = e.Y;
                            }
                        }
                        else if ((e.Y - lastY) <= -1 * moveThreshold)
                        {
                            int temp = Value + 1;
                            if (temp <= calculateSum(findIndexOfSliderValue()) && temp > calculateSum(findIndexOfSliderValue() - 1))
                            {
                                Value = Value + 1;
                                lastY = e.Y;
                            }
                        }
                    }
                }
            }
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			Capture = false;
			clickedOnSlider = false;
			clickedOnSecondarySlider = false;
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
			base.OnEnter(e);
			Focus();
			this.Invalidate();
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			this.Invalidate();
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);

			switch (e.KeyCode)
			{
				case Keys.Down:
					Value = Value - 1;
					break;
				case Keys.Up:
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
		/// Generates a graphics path for the little triangle slider below the main slider
		/// </summary>
		/// <param name="secondarySliderHorizontalCenter">The horizontal center of the slider relative to the client rectangle. NOT relative to the screen</param>
		/// <param name="secondarySliderY">The top of the triangle relative to client rectangle</param>
		/// <param name="secondarySliderWidth">Width of the traingle slider</param>
		/// <param name="secondarySliderHeight">Height of the triangle slider</param>
		/// <returns>Returns a graphics path outlining a traingle</returns>
		private GraphicsPath generateSecondarySliderPath(float secondarySliderVerticalCenter, float secondarySliderX, float secondarySliderWidth, float secondarySliderHeight)
		{
			GraphicsPath gp = new GraphicsPath();

			gp.AddLine(secondarySliderX, secondarySliderVerticalCenter - secondarySliderHeight / 2, secondarySliderX + secondarySliderWidth, secondarySliderVerticalCenter);
			//
			// line from top left to middle right (top half of >)
			//

			gp.AddLine(secondarySliderX, secondarySliderVerticalCenter + secondarySliderHeight / 2, secondarySliderX + secondarySliderWidth, secondarySliderVerticalCenter);
			//
			//  >
			//

			gp.AddLine(secondarySliderX, secondarySliderVerticalCenter - secondarySliderHeight / 2, secondarySliderX, secondarySliderVerticalCenter + secondarySliderHeight / 2);
			//
			// |>
			//

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
				return -1;
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
		/// This method checks to see if a certain point is within the bounds of the slider
		/// </summary>
		/// <param name="point">The point that is being checked</param>
		/// <returns>Returns true if the point is in the slider region, false otherwise</returns>
		private bool mouseInSliderRegion(Point point)
		{
			return sliderGP.GetBounds().Contains(point);
		}

		/// <summary>
		/// This method checks to see if a certain point is within the bounds of the secondary slider
		/// </summary>
		/// <param name="point">The point that is being checked</param>
		/// <returns>Returns true if the point is in the secondary slider region, false otherwise</returns>
		private bool mouseInSecondarySliderRegion(Point point)
		{
			return secondarySliderGP.GetBounds().Contains(point);
		}

		#endregion
	}
}
