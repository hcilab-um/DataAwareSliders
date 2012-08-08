using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CustomSlider
{
    public partial class DDActiveAreaSlider : DisplayDistortionSlider
    {
        #region Variables

		public event EventHandler StartMouseWheel;

		//private const int MINIMUM_SLIDER_WIDTH = 20;
		private float itemsPerHistogramPixel = 0;
		//private new int SliderWidth = 0;
		private int maxItemsPerSliderPixel = 2; //equivalent of max items per thumb pixel
		private int itemsPerSliderPixel = 0;
		private int rollChangeValue = 1;

		private bool clickedOnSecondarySlider = false;
		private bool rolledMouseWheel = false;
		//private bool drawSecondarySlider = 
		private GraphicsPath secondarySliderGP = null;
		//private GraphicsPath sliderGP;

		#endregion

		#region Getters and Setters

		public float ItemsPerHistogramPixel
		{
			get { return itemsPerHistogramPixel; }
		}

		public int ItemsPerSliderPixel
		{
			get { return itemsPerSliderPixel; }
		}

		public new GraphicsPath SliderGP
		{
			get { return base.SliderGP; }
		}

		public new bool DrawSlider
		{
			get { return base.DrawSlider; }
            set { base.DrawSlider = value; }
		}

		public int RollChangeValue
		{
			get { return rollChangeValue; }
			set
			{
				if (value > 0)
					rollChangeValue = value;
			}
		}

		public int MaxItemsPerSliderPixel
		{
			get { return maxItemsPerSliderPixel; }
			set
			{
				if (value > 0)
					maxItemsPerSliderPixel = value;
			}
		}

		#endregion

		public DDActiveAreaSlider()
		{
			InitializeComponent();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			doPaintingMath(); //this needs to be done so that values such as spacebetweenticks isn't 0 during the first paint

			//Deal with slider positioning only if the slider is being dragged
			//if (base.DrawSlider)
			//{
				float sliderCenterX = TrackXStart + base.SliderWidth / 2;

                float effectiveTrackWidth = TrackWidth - base.SliderWidth;
                float itemsPerPixel = (base.Maximum - base.Minimum + 1) / effectiveTrackWidth;

				double potentialWidth = Math.Round(itemsPerPixel + 0.5) / maxItemsPerSliderPixel;
				base.SliderWidth = (int)Math.Max(MINIMUM_SLIDER_WIDTH, potentialWidth);
				itemsPerSliderPixel = (int)Math.Round(itemsPerHistogramPixel / SliderWidth, MidpointRounding.ToEven);
				if (itemsPerSliderPixel < 2)
					itemsPerSliderPixel = 2;

				//base.SliderGP = generateSliderPath(sliderCenterX, TrackYValue, SliderWidth);
				
			//}
            base.OnPaint(e);

			//deal with secondary slider positioning
			float secondarySliderY = base.SliderGP.GetBounds().Y;
			float secondarySliderWidth = 15;
			float secondarySliderHeight = 12;
			float secondarySliderHorizontalCenter;

			if (clickedOnSecondarySlider || rolledMouseWheel || Value == 0 || Value == calculateMax())
			{
				secondarySliderHorizontalCenter = SliderGP.GetBounds().X + (Value - RangeOfValues[0]) / (RangeOfValues.Count * 1.0f - 1) * SliderGP.GetBounds().Width;

				if (RangeOfValues.Count == 1)
				{
					secondarySliderHorizontalCenter = SliderGP.GetBounds().X + (Value - RangeOfValues[0]) / (RangeOfValues.Count * 1.0f) * SliderGP.GetBounds().Width; //don't subtract 1
				}

                if (rolledMouseWheel)
                {
                    rolledMouseWheel = false;
                    base.DrawSlider = true;
                }
			}
            else if (base.DraggingSlider)
			{
				secondarySliderHorizontalCenter = SliderGP.GetBounds().X + SliderGP.GetBounds().Width / 2; //default the positioning of the secondary slider to the center of the main slider
                base.DraggingSlider = false;
            }
			else
			{
				secondarySliderHorizontalCenter = secondarySliderGP.GetBounds().X + secondarySliderGP.GetBounds().Width / 2; //if we meet none of the above conditions, don't move the secondary slider horizontally
			}

			//base.OnPaint(e);

			//draw secondarySlider
			secondarySliderGP = generateSecondarySliderPath(secondarySliderHorizontalCenter, secondarySliderY, secondarySliderWidth, secondarySliderHeight);
			if (secondarySliderGP != null)
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;

				g.FillPath(new SolidBrush(Color.FromArgb(191, 63, 65)), secondarySliderGP);
				g.DrawPath(new Pen(Color.Black), secondarySliderGP);

				g.SmoothingMode = SmoothingMode.Default;
			}

            NeedToDoPaintingMath = true;
		}

        #region Overriden events

        /// <summary>
		/// This method enables slider dragging
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && mouseInSecondarySliderRegion(e.Location))
			{
				Capture = true;
                base.ClickedOnSlider = false;
				clickedOnSecondarySlider = true;
                base.DrawSlider = false;
				slowDownMouse();
			}
			else
			{
				base.OnMouseDown(e);
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (clickedOnSecondarySlider)
			{
				if (e.X < (int)Math.Round(SliderGP.GetBounds().X))
					Value = RangeOfValues[0];
				else if (e.X > (int)Math.Round(SliderGP.GetBounds().Right))
					Value = RangeOfValues[RangeOfValues.Count - 1];
				else //The mouse is somewhere between the beginning and end of the track
				{
					//Then find out how far "into" the index the mouse is. Is it 1/4 past the beginning of the index? 1/2 way? 3/17?
					//Based on the index of the mosue and it's "penetration" into that index I can calculate a value

					//Find mouse penetration
					float penetration = (e.X - SliderGP.GetBounds().X) / SliderGP.GetBounds().Width;

					//calculate value
					if (penetration >= 0 && penetration <= 1)
					{
						int tempValue = RangeOfValues[0];
						tempValue += (int)(penetration * RangeOfValues.Count);

						Value = tempValue;
					}

				}
			}
			else
			{
				base.OnMouseMove(e);
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			clickedOnSecondarySlider = false;
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			//We've started rolling the mouse wheel
			if (StartMouseWheel != null)
				StartMouseWheel(this, e);

			base.OnMouseWheel(e);
			int newValue, tempValue;

			if (e.Delta < 0)
			{
				newValue = Value - rollChangeValue;
			}
			else
			{
				newValue = Value + rollChangeValue;
			}
            tempValue = Value;
			if (newValue < RangeOfValues[0] || newValue > RangeOfValues[RangeOfValues.Count - 1])
			{
                base.DrawSlider = true;
                
                if (newValue < RangeOfValues[0])
                {
                    while (tempValue > 0 && RangeOfValues[RangeOfValues.Count - 1] != newValue)
                    {
                        //Value--; //causes repeated invalidation which is slow

                        tempValue--;
                        updateRangeAroundValues(tempValue);
                    }
                }
                else
                {
                    while (tempValue < calculateMax() && RangeOfValues[0] != newValue)
                    {
                       // Value++;
                        tempValue++;
                        updateRangeAroundValues(tempValue);
                    }
                }

                Refresh();
			}

            base.DrawSlider = false;
            rolledMouseWheel = true;
			Value = newValue;
			
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			//base.OnKeyDown(e);

			//simulate mousewheel events
			switch (e.KeyCode)
			{
				case Keys.Down:
				case Keys.Left:
					OnMouseWheel(new MouseEventArgs(System.Windows.Forms.MouseButtons.None, 0, 0, 0, -120));
					break;
				case Keys.Up:
				case Keys.Right:
					OnMouseWheel(new MouseEventArgs(System.Windows.Forms.MouseButtons.None, 0, 0, 0, 120));
					break;
			}
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Left || keyData == Keys.Right || keyData == Keys.Up || keyData == Keys.Down)
			{
				//base.DraggingSlider = true; //This is the difference between parent implementation and this implementation
				OnKeyDown(new KeyEventArgs(keyData));
				return true;
			}

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
		private GraphicsPath generateSliderPath(float sliderCenterX, float sliderCenterY, float SliderWidth)
		{
			GraphicsPath gp = new GraphicsPath();

			float leftX, rightX, topY, bottomY;

			if (sliderCenterX - SliderWidth / 2 > ClientRectangle.X + 1)
				leftX = sliderCenterX - SliderWidth / 2;
			else
				leftX = ClientRectangle.X + 1;

			if (sliderCenterX + SliderWidth / 2 < ClientRectangle.Width - 1)
				rightX = sliderCenterX + SliderWidth / 2;
			else
				rightX = ClientRectangle.Width - 1;

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

		/// <summary>
		/// Generates a graphics path for the little triangle slider below the main slider
		/// </summary>
		/// <param name="secondarySliderHorizontalCenter">The horizontal center of the slider relative to the client rectangle. NOT relative to the screen</param>
		/// <param name="secondarySliderY">The top of the triangle relative to client rectangle</param>
		/// <param name="secondarySliderWidth">Width of the traingle slider</param>
		/// <param name="secondarySliderHeight">Height of the triangle slider</param>
		/// <returns>Returns a graphics path outlining a traingle</returns>
		private GraphicsPath generateSecondarySliderPath(float secondarySliderHorizontalCenter, float secondarySliderY, float secondarySliderWidth, float secondarySliderHeight)
		{
			GraphicsPath gp = new GraphicsPath();

			PointF bottomCenter = new PointF(secondarySliderHorizontalCenter, secondarySliderY);
			PointF topLeft = new PointF(secondarySliderHorizontalCenter - secondarySliderWidth / 2, secondarySliderY - secondarySliderHeight);
			PointF topRight = new PointF(secondarySliderHorizontalCenter + secondarySliderWidth / 2, secondarySliderY - secondarySliderHeight);

			gp.AddLine(bottomCenter, topLeft);
			gp.AddLine(topLeft, topRight);
			gp.AddLine(topRight, bottomCenter);

			return gp;
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
