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
	public abstract partial class InputDistortionSlider : Slider
	{
		#region Variables

		protected float histogramMaxHeight = 0;
		protected float histogramLowerY = 0;
		protected float histogramUpperY = 0;
		protected float spaceBetweenTicks = 0;

		protected int largestIndex = 0;
		protected int smallestIndex = 0;

		private int centerOfRange = 0;

		protected bool drawSlider = true;
        private Point lastMousePosition;
        private bool needToDoPaintingMath = true;

		#endregion

		#region Getters and Setters

        protected bool NeedToDoPaintingMath
        {
            get { return needToDoPaintingMath; }
            set { needToDoPaintingMath = value; }
        }

		public new int Value
		{
			get { return base.Value; }
            set
            {
                base.Value = value;

                //We only update the offset when the main slider is changing it's position
                //the center of our range of values also only changes when we (re)draw the slider
                if (drawSlider)
                {
                    centerOfRange = Value;
                    updateOffset(Value);
                }

                Invalidate();

                base.OnValueChanged();


                //Refresh();
            }
		}

		public new List<uint> ItemsInIndices
		{
			get { return base.ItemsInIndices; }
			set
			{
				base.ItemsInIndices = value;
				largestIndex = findLargestIndex();
				smallestIndex = findSmallestInex();
				Invalidate();
			}
		}

		public new List<int> RangeOfValues
		{
			get { return base.RangeOfValues; }
			protected set { base.RangeOfValues = value; }
		}

		public new List<char> IndexCharacters
		{
			get { return base.IndexCharacters; }
			set
			{
               base.IndexCharacters = value;
			}
		}

        protected Point LastMousePosition
        {
            get { return lastMousePosition; }
            set { lastMousePosition = value; }
        }

		#endregion

		public InputDistortionSlider()
		{
			InitializeComponent();

			largestIndex = findLargestIndex();
			smallestIndex = findSmallestInex();
			//doPaintingMath();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);

			Graphics g = pe.Graphics;

			doPaintingMath();


			//This following code will draw ticks and the histograms above them
			float histogramHeight;
			SolidBrush histogramBrush = new SolidBrush(Color.FromArgb(30,70,173));

			Pen blackPen = new Pen(Color.Black, 2);
			Pen redPen = new Pen(Color.Red, 2);
			int index = 0;
			for (float i = TrackXStart; i < TrackXEnd - 1; i += spaceBetweenTicks)
			{
				//draw ticks
				g.DrawLine(blackPen, i, TrackYValue, i, TrackYValue + 10); //ticks will be 10 pixels tall

				//draw histogram
				histogramHeight = getCurrHistogramHeight(index);
				g.FillRectangle(histogramBrush, i, histogramLowerY - histogramHeight, spaceBetweenTicks, histogramHeight);
				g.DrawRectangle(blackPen, i, histogramLowerY - histogramHeight, (float)(TrackWidth / (float)base.ItemsInIndices.Count), histogramHeight);

                if (base.IndexCharacters != null)
				{
                    g.DrawString(base.IndexCharacters[index] + "", new Font(FontFamily.GenericSerif, 10), new SolidBrush(Color.Black), i, histogramLowerY + 1);
				}

				index += 1;
			}

			//draw track
			g.DrawLine(blackPen, TrackXStart, TrackYValue, TrackXEnd, TrackYValue);

			//Deal with slider positioning
			GraphicsPath currSliderGP = SliderGP;

			if (CustomSliderGP == null)
			{
				int indexOfSlider = findIndexOfSliderValue();
				float sliderCenterX = TrackXStart;
				float proportion;
				if (indexOfSlider != -1)
				{
					//This will make the x value of the slider go to the right tick. From here it will be shifted over more
					//based on it's value and the number of items assocaited with that index
					for (int i = 0; i < indexOfSlider; i++)
					{
						sliderCenterX += spaceBetweenTicks;
					}

                    proportion = ((float)(base.Value - calculateSum(indexOfSlider - 1)) / (float)base.ItemsInIndices[indexOfSlider]);
					sliderCenterX += proportion * (spaceBetweenTicks);
				}
				if (drawSlider)
					SliderGP = generateSliderPath(sliderCenterX, TrackYValue);

				currSliderGP = SliderGP;
			}
			else
			{
				currSliderGP = CustomSliderGP;
			}

			g.DrawLine(redPen, currSliderGP.GetBounds().X + currSliderGP.GetBounds().Width / 2, histogramLowerY, currSliderGP.GetBounds().X + currSliderGP.GetBounds().Width / 2, histogramLowerY - getCurrHistogramHeight(findIndexOfSliderValue()));

			g.FillPath(new SolidBrush(Color.FromArgb(128, Color.Gray)), currSliderGP);
			g.DrawPath(blackPen, currSliderGP);

			SlideArea = generateSlideArea();

		}//end paint

        protected void doPaintingMath()
        {
            if (needToDoPaintingMath)
            {
                base.SliderWidth = 30;
                base.SliderHeight = 15;

                //track math
                base.TrackYValue = ClientRectangle.Height - base.SliderHeight; //the Y value of the start point of the track. The start and end points are the same since the track is a straight line. 0.2 was arbitrarily chosen
                base.TrackXStart = ClientRectangle.X + base.SliderWidth / 2 + 1; //the X value of the start point of the track
                base.TrackXEnd = ClientRectangle.Width - base.SliderWidth / 2 - 1; //the x value of the end point of the track
                base.TrackWidth = base.TrackXEnd - base.TrackXStart;

                //histogram math
                //histogramLowerY = TrackYValue - SliderHeight / 2 - 3;
                histogramLowerY = base.TrackYValue;
                histogramUpperY = ClientRectangle.Y + 1; //don't want histograms going all the way to the top of the ClientRectangle
                histogramMaxHeight = histogramLowerY - histogramUpperY;
                needToDoPaintingMath = false;
            }
            spaceBetweenTicks = (float)(base.TrackWidth / (float)this.ItemsInIndices.Count);
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
					GraphicsPath currSliderGP = SliderGP != null ? SliderGP : CustomSliderGP;
					int newXValue = (int)Math.Round(currSliderGP.GetBounds().X + currSliderGP.GetBounds().Width / 2);
					Cursor.Position = PointToScreen(new Point(newXValue, PointToClient( Cursor.Position).Y));
					lastMousePosition = Cursor.Position;

					Capture = true;
                    base.ClickedOnSlider = true;
					drawSlider = true;

					slowDownMouse();		
				}
				else if (SlideArea.GetBounds().Contains(e.Location))
				{
					//simulate a mouse move event
					Capture = true;
                    base.ClickedOnSlider = true;
					drawSlider = true;
                    processMouseLocation(e.Location);

					slowDownMouse();
				}
			}

            base.OnNewMouseDown(e);
		}

		/// <summary>
		/// This method allows the slider to be dragged, but only if it's been clicked on
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

            processMouseLocation(e.Location);

            base.OnNewMouseMove(e);

		}

        private void processMouseLocation(Point mouseLocation)
        {
            if (Capture && base.ClickedOnSlider && !lastMousePosition.Equals(Cursor.Position))
            {
                if (mouseLocation.X < 0)
                    Value = 0;
                else if (mouseLocation.X > ClientRectangle.Width)
                    Value = calculateMax();
                else //The mouse is somewhere between the beginning and end of the track
                {
                    //Need to find the index of the mouse (which histogram is the mouse under?)
                    //Then find out how far "into" the index the mouse is. Is it 1/4 past the beginning of the index? 1/2 way? 3/17?
                    //Based on the index of the mosue and it's "penetration" into that index I can calculate a value

                    //Find index of mouse
                    bool foundIndex = false;
                    int mouseIndex = -1;
                    GraphicsPath currSliderGP = CustomSliderGP == null ? SliderGP : CustomSliderGP;
                    for (float i = SliderWidth / 2 + spaceBetweenTicks; i < ClientRectangle.Width - 1 && !foundIndex; i += spaceBetweenTicks)
                    {
                        mouseIndex++;
                        if (mouseLocation.X < i)
                            foundIndex = true;
                    }

                    //Find mouse penetration
                    float penetration = (mouseLocation.X - (spaceBetweenTicks * mouseIndex) - SliderWidth / 2) / spaceBetweenTicks;

                    //calculate value
                    int tempValue = calculateSum(mouseIndex - 1);
                    tempValue += (int)(penetration * base.ItemsInIndices[mouseIndex]);

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

                base.DraggingSlider = true;
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
            base.ClickedOnSlider = false;
			drawSlider = false;
            base.DraggingSlider = false;

			resetMouseSpeed();

            base.OnNewMouseUp(e);
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
				OnMouseDown(new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, (int)(CustomSliderGP.GetBounds().Right) - 1, (int)TrackYValue, 0));
				OnMouseMove(new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, (int)(CustomSliderGP.GetBounds().Right) - 1, (int)TrackYValue, 0));
				OnMouseUp(new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, (int)(CustomSliderGP.GetBounds().Right) - 1, (int)TrackYValue, 0));
			}
			drawSlider = true;
			return base.ProcessDialogKey(keyData);
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// This method generates a graphicsPath for the slider
		/// </summary>
		/// <param name="sliderCenterX">The x value of the slider (left edge location)</param>
		/// <param name="sliderCenterY">the center of the slider heightwise</param>
		/// <param name="SliderWidth">the width of the slider</param>
		/// <returns>A rectangular graphics path</returns>
		protected override GraphicsPath generateSliderPath(float sliderCenterX, float sliderCenterY)
		{
			GraphicsPath gp = new GraphicsPath();

			float leftX, rightX, topY, bottomY;

			leftX = sliderCenterX - SliderWidth / 2;
			rightX = sliderCenterX + SliderWidth / 2;
			topY = sliderCenterY - SliderHeight / 2;
			bottomY = sliderCenterY + SliderHeight / 2;
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

		protected override GraphicsPath generateSlideArea()
		{
			GraphicsPath slideArea = new GraphicsPath();

			PointF topLeft = new PointF(base.TrackXStart, TrackYValue - SliderHeight);
			PointF topRight = new PointF(base.TrackXEnd, TrackYValue - SliderHeight);
            PointF bottomLeft = new PointF(base.TrackXStart, TrackYValue + SliderHeight);
            PointF bottomRight = new PointF(base.TrackXEnd, TrackYValue + SliderHeight);

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

            for (int i = 0; i < base.ItemsInIndices.Count && result == -1; i++)
			{
                sumSoFar += (int)base.ItemsInIndices[i];
				if (base.Value <= sumSoFar)
					result = i;
			}

			return result;
		}

		protected int findIndexOfValue(int value)
		{
			int sumSoFar = 0;
			int result = -1;

            for (int i = 0; i < base.ItemsInIndices.Count && result == -1; i++)
			{
                sumSoFar += (int)base.ItemsInIndices[i];
				if (value <= sumSoFar)
					result = i;
			}

			return result;
		}		

		/// <summary>
		/// Finds the index with that maps the largest number of items
		/// </summary>
		/// <returns>The value of the index that maps the largest number of items</returns>
		public int findLargestIndex()
		{
			int largest = 0;

            for (int i = 0; i < base.ItemsInIndices.Count; i++)
			{
                if (base.ItemsInIndices[i] > base.ItemsInIndices[largest])
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

            for (int i = 0; i < base.ItemsInIndices.Count; i++)
			{
                if (base.ItemsInIndices[i] < base.ItemsInIndices[smallest])
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
			if (CustomSliderGP == null)
				return SliderGP.GetBounds().Contains(point);
			else
				return CustomSliderGP.GetBounds().Contains(point);
		}

		protected override void updateOffset(int value)
		{
			if (spaceBetweenTicks > 0)
			{
                float itemsPerPixel = base.ItemsInIndices[findIndexOfValue(value)] / spaceBetweenTicks;

				if (itemsPerPixel <= (float)1)
				{
					base.Offset = 0;
				}
				else
				{
					base.Offset = (int)Math.Round(itemsPerPixel + 0.5);

					if (base.Offset % 2 == 1)
						base.Offset++;
				}

			}

			this.updateRangeAroundValues(value);
		}

		protected override void updateRangeAroundValues(int value)
		{
			List<int> temp = new List<int>();
			for (int i = base.Offset / 2; i > 0; i--)
			{
				if (Value - i >= 1)
					temp.Add(value - i);
			}

			temp.Add(value);

			for (int i = 1; i <= base.Offset / 2; i++)
			{
				if (Value + i <= calculateMax())
					temp.Add(value + i);
			}

			base.RangeOfValues = temp;
		}

		protected float getCurrHistogramHeight(int histogramIndex)
		{
            return (float)(histogramMaxHeight * (float)((float)base.ItemsInIndices[histogramIndex] / (float)base.ItemsInIndices[largestIndex]));
		}

		#endregion
	}//end class
}
