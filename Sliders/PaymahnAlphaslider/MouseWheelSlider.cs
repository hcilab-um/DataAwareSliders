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
	public partial class MouseWheelSlider : DensitySlider
	{
		private GraphicsPath sliderGP;
		private bool rollingMouseWheel = false;

		public MouseWheelSlider()
		{
			InitializeComponent();
			customSliderGP = new GraphicsPath();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			Graphics g = pe.Graphics;
			doPaintingMath();

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
				float sliderWidth = spaceBetweenTicks * ItemsInIndices[indexOfSlider] / ItemsInIndices[largestIndex];
				sliderGP = generateSliderPath(sliderCenterX, trackYValue, (int)sliderWidth);
			}

			customSliderGP = sliderGP;

			//deal with secondary slider positioning
			float innerLineTopY = trackYValue + sliderHeight / 2 + 3; //Make the line jut out of the slider
			float innerLineBottomY = trackYValue - sliderHeight / 2 - 3;
			float innerLineX = sliderGP.GetBounds().X + sliderGP.GetBounds().Width / 2; //default the positioning of the secondary slider to the center of the main slider
			if (rollingMouseWheel)
			{
				innerLineX = sliderGP.GetBounds().X + (Value - RangeOfValues[0]) / (RangeOfValues.Count * 1.0f - 1) * sliderGP.GetBounds().Width;

				if (RangeOfValues.Count == 1)
				{
					innerLineX = sliderGP.GetBounds().X + (Value - RangeOfValues[0]) / (RangeOfValues.Count * 1.0f) * sliderGP.GetBounds().Width; //don't subtract 1
				}

				rollingMouseWheel = false;
			}

			if (innerLineX > sliderGP.GetBounds().X + sliderGP.GetBounds().Width)
			{ }
			//g.FillPath(sliderBrush, secondarySliderGP);
			

			base.OnPaint(pe);

			g.DrawLine(new Pen(Color.Red, 3), innerLineX, innerLineTopY, innerLineX, innerLineBottomY);
			drawSlider = true;
		}


		protected override void OnMouseWheel(MouseEventArgs e)
		{
			rollingMouseWheel = true;
			drawSlider = false;
			int tempValue = Value;

			if (e.Delta < 0)
				tempValue--;
			else if (e.Delta > 0)
				tempValue++;

			if (tempValue < RangeOfValues[0])
				tempValue = RangeOfValues[0];
			else if (tempValue > RangeOfValues[RangeOfValues.Count - 1])
				tempValue = RangeOfValues[RangeOfValues.Count - 1];

			Value = tempValue;

			base.OnMouseWheel(e);

			//drawSlider = true;
			//rollingMouseWheel = false;
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
	}



}
