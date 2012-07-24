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
using System.Messaging;

namespace CustomSlider
{
	public partial class ActiveAreaSliderv2 : DensitySlider, IDensitySlider
	{
		#region Variables
		private const int MINIMUM_SLIDER_WIDTH = 20;
		private float itemsPerHistogramPixel = 0;
		private new int sliderWidth = 0;
		private int maxItemsPerSliderPixel = 2; //equivalent of max items per thumb pixel
		private int itemsPerSliderPixel = 0;
		private int rollChangeValue = 1;

		private bool clickedOnSecondarySlider = false;
		private bool rolledMouseWheel = false;
		private bool firstTimeBeingDrawn = true;
		//private bool drawSecondarySlider = 
		private GraphicsPath secondarySliderGP = null;
		private GraphicsPath sliderGP;

		#endregion

		#region Getters and Setters

		public int SliderWidth
		{
			get { return sliderWidth; }
		}

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
			get { return this.sliderGP; }
		}

		public bool DrawSlider
		{
			get { return drawSlider; }
			set { drawSlider = value; }
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

		public ActiveAreaSliderv2()
		{
			InitializeComponent();
			customSliderGP = new GraphicsPath();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			doPaintingMath(); //this needs to be done so that values such as spacebetweenticks isn't 0 during the first paint


			//Deal with slider positioning only if the slider is being dragged
			if (drawSlider)
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

					proportion = ((float)(Value - calculateSum(indexOfSlider - 1)) / (float)ItemsInIndices[indexOfSlider]);
					sliderCenterX += proportion * (spaceBetweenTicks);
				}

				itemsPerHistogramPixel = (float)ItemsInIndices[indexOfSlider] / spaceBetweenTicks;
				double potentialWidth = Math.Round(itemsPerHistogramPixel + 0.5) / maxItemsPerSliderPixel;
				sliderWidth = (int)Math.Max(MINIMUM_SLIDER_WIDTH, potentialWidth);
				itemsPerSliderPixel = (int)Math.Round(itemsPerHistogramPixel / sliderWidth, MidpointRounding.ToEven);
				if (itemsPerSliderPixel < 2)
					itemsPerSliderPixel = 2;

				sliderGP = generateSliderPath(sliderCenterX, trackYValue, sliderWidth);
				
			}

			customSliderGP = sliderGP;

			//deal with secondary slider positioning
			float secondarySliderY = trackYValue - sliderHeight / 2;
			float secondarySliderWidth = 15;
			float secondarySliderHeight = 12;
			float secondarySliderHorizontalCenter;

			if (clickedOnSecondarySlider || rolledMouseWheel || Value == 0 || Value == calculateMax())
			{
				secondarySliderHorizontalCenter = sliderGP.GetBounds().X + (Value - RangeOfValues[0]) / (RangeOfValues.Count * 1.0f - 1) * sliderGP.GetBounds().Width;

				if (RangeOfValues.Count == 1)
				{
					secondarySliderHorizontalCenter = sliderGP.GetBounds().X + (Value - RangeOfValues[0]) / (RangeOfValues.Count * 1.0f) * sliderGP.GetBounds().Width; //don't subtract 1
				}

				rolledMouseWheel = false;
			}
			else if (/*firstTimeBeingDrawn || */clickedOnSlider || drawSlider)
			{
				secondarySliderHorizontalCenter = sliderGP.GetBounds().X + sliderGP.GetBounds().Width / 2; //default the positioning of the secondary slider to the center of the main slider
				firstTimeBeingDrawn = false;
			}
			
			else
			{
				secondarySliderHorizontalCenter = secondarySliderGP.GetBounds().X + secondarySliderGP.GetBounds().Width / 2; //if we meet none of the above conditions, don't move the secondary slider horizontally
			}

			base.OnPaint(e);

			//draw secondarySlider
			secondarySliderGP = generateSecondarySliderPath(secondarySliderHorizontalCenter, secondarySliderY, secondarySliderWidth, secondarySliderHeight);
			if (secondarySliderGP != null)
			{
				g.FillPath(new SolidBrush(Color.Red), secondarySliderGP);
				g.DrawPath(new Pen(Color.Black), secondarySliderGP);
			}


			
			//base.OnPaint(e);
		}

		/// <summary>
		/// This method enables slider dragging
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && mouseInSecondarySliderRegion(e.Location))
			{
				Capture = true;
				clickedOnSlider = false;
				clickedOnSecondarySlider = true;
				drawSlider = false;
				slowDownMouse();
			}
			else
			{
				base.OnMouseDown(e);
			}
		}

		public void simulateMouseWheel(MouseEventArgs e)
		{
			OnMouseWheel(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (!clickedOnSecondarySlider)
			{
				base.OnMouseMove(e);
			}
			else
			{
				if (e.X < (int)Math.Round(sliderGP.GetBounds().X))
					Value = RangeOfValues[0];
				else if (e.X > (int)Math.Round(sliderGP.GetBounds().Right))
					Value = RangeOfValues[RangeOfValues.Count - 1];
				else //The mouse is somewhere between the beginning and end of the track
				{
					//Then find out how far "into" the index the mouse is. Is it 1/4 past the beginning of the index? 1/2 way? 3/17?
					//Based on the index of the mosue and it's "penetration" into that index I can calculate a value

					//Find mouse penetration
					float penetration = (e.X - sliderGP.GetBounds().X) / sliderGP.GetBounds().Width;

					//calculate value
					if (penetration >= 0 && penetration <= 1)
					{
						int tempValue = RangeOfValues[0];
						tempValue += (int)(penetration * RangeOfValues.Count);

						Value = tempValue;
					}

				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			clickedOnSecondarySlider = false;
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			drawSlider = false;
			rolledMouseWheel = true;
			int newValue;

			Console.WriteLine(itemsPerSliderPixel);

			if (e.Delta < 0)
			{
				newValue = Value - rollChangeValue;
			}
			else
			{
				newValue = Value + rollChangeValue;
			}

			if (newValue < RangeOfValues[0] || newValue > RangeOfValues[RangeOfValues.Count - 1])
				drawSlider = true;
			Value = newValue;
			
		}

		#region Helper Methods

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

			if (sliderCenterX - sliderWidth / 2 > ClientRectangle.X + 1)
				leftX = sliderCenterX - sliderWidth / 2;
			else
				leftX = ClientRectangle.X + 1;

			if (sliderCenterX + sliderWidth / 2 < ClientRectangle.Width - 1)
				rightX = sliderCenterX + sliderWidth / 2;
			else
				rightX = ClientRectangle.Width - 1;

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
