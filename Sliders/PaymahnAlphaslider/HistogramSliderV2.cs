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
	public partial class HistogramSliderV2 : DensitySlider
	{
		public event EventHandler TrueValueChanged;
		#region Variables

		GraphicsPath secondarySliderGP = null;
		private bool clickedOnSecondarySlider = false;
		private bool disableLaterlMovement = false;
		private bool rightButtonDown = false;
		private int trueValue = 0;

		#endregion

		public int TrueValue
		{
			get { return trueValue; }
			set
			{
				if (RangeOfValues != null)
				{
					if (value < RangeOfValues[0])
						trueValue = RangeOfValues[0];
					else if (value > RangeOfValues[RangeOfValues.Count - 1])
						trueValue = RangeOfValues[RangeOfValues.Count - 1];
					else
						trueValue = value;
				}
				else
					trueValue = value;

				if (TrueValueChanged != null)
					TrueValueChanged(this, new EventArgs());

				Invalidate();
			}
		}

		public HistogramSliderV2()
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

			doPaintingMath();

			//deal with secondary slider positioning
			float histogramHeight = base.getCurrHistogramHeight(base.findIndexOfSliderValue());
			float secondarySliderWidth = 10;
			float secondarySliderHeight = 10;
			//float secondarySliderX = sliderWidth / 2 + spaceBetweenTicks * findIndexOfSliderValue();
			float secondarySliderX = SliderGP.GetBounds().X + SliderGP.GetBounds().Width / 2;
			float secondarySliderVerticalCenter = histogramLowerY - ((trueValue - RangeOfValues[0]) / (RangeOfValues.Count * 1.0f - 1)) * histogramHeight;

			if(RangeOfValues.Count == 1)
				secondarySliderVerticalCenter = histogramLowerY - ((Value - RangeOfValues[0]) / (RangeOfValues.Count * 1.0f)) * histogramHeight;

			//draw secondarySlider
			secondarySliderGP = generateSecondarySliderPath(secondarySliderVerticalCenter, secondarySliderX, secondarySliderWidth, secondarySliderHeight);
			g.FillPath(new SolidBrush(Color.Red), secondarySliderGP);
			g.DrawPath(new Pen(Color.Black), secondarySliderGP);

		}

		#region Mouse events

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && mouseInSecondarySliderRegion(e.Location))
			{
				Capture = true;
				clickedOnSlider = false;
				clickedOnSecondarySlider = true;
				drawSlider = false;
			}
			//else
			//{
				
			//}
			base.OnMouseDown(e);
			//if (e.Button == MouseButtons.Right && clickedOnSlider)
			//    rightButtonDown = true;

			//if (clickedOnSlider && rightButtonDown)
			//{
			//    disableLaterlMovement = true;
			//    Capture = true;
			//    clickedOnSlider = true;
			//    clickedOnSecondarySlider = true;
			//    drawSlider = false;
			//}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			Debug.WriteLine("ClickedOnSlider: {0}\t\tClickedOnSecondary: {1}\t\tRightButton: {2}\t\tDisable: {3}\t\tCapture: {4}", clickedOnSlider, clickedOnSecondarySlider, rightButtonDown, disableLaterlMovement, Capture);

			if(clickedOnSlider)
				base.OnMouseMove(e);

			if (clickedOnSlider || clickedOnSecondarySlider)
			{
				float currHistogramHeight = getCurrHistogramHeight(findIndexOfSliderValue());

				if (e.Y < (int)(histogramLowerY - currHistogramHeight))
					TrueValue = RangeOfValues[RangeOfValues.Count - 1];
				else if (e.Y > (int)Math.Round(histogramLowerY))
					TrueValue = RangeOfValues[0];
				else //The mouse is somewhere between the beginning and end of the track
				{
					//Then find out how far "into" the index the mouse is. Is it 1/4 past the beginning of the index? 1/2 way? 3/17?
					//Based on the index of the mosue and it's "penetration" into that index I can calculate a value

					//Find mouse penetration
					float penetration = 1 - ((e.Y - (histogramLowerY - currHistogramHeight)) / currHistogramHeight);

					//Make sure our value for penetration is valid
					if (penetration <= 1 && penetration >= 0)
					{
						//calculate value
						int tempValue = RangeOfValues[0];
						tempValue += (int)(penetration * RangeOfValues.Count);
						TrueValue = tempValue;
					}
				}
			}

		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			//rightButtonDown = false;
			//disableLaterlMovement = false;
			//Capture = true;
			if (e.Button == MouseButtons.Left)
			{
				clickedOnSecondarySlider = false;
				base.OnMouseUp(e);
			}
			else
			{
				//OnMouseDown(e);
			}
		}

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
			else
				return base.ProcessDialogKey(keyData);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			//base.OnKeyDown(e);
			int tempValue;

			switch (e.KeyCode)
			{
				case Keys.Down:
				case Keys.Left:
					tempValue = trueValue - 1;
					if (tempValue < RangeOfValues[0])
					{
						Value = Value - 1;
					}
					TrueValue = tempValue;
					break;
				case Keys.Up:
				case Keys.Right:
					tempValue = trueValue + 1;
					if (tempValue > RangeOfValues[RangeOfValues.Count - 1])
					{
						Value = Value + 1;
					}
					TrueValue = tempValue;
					break;
			}
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			//base.OnKeyUp(e);

			//switch (e.KeyCode)
			//{
			//    case Keys.Down:
			//    case Keys.Left:
			//        TrueValue = TrueValue - 1;
			//        break;
			//    case Keys.Up:
			//    case Keys.Right:
			//        TrueValue = TrueValue + 1;
			//        break;
			//}
		}

		#endregion


		#region Helper Methods

		protected override void updateRangeOfValues()
		{
			List<int> temp = new List<int>();
			temp.Add(Value);
			for (int i = 1; i <= 2*offset; i++)
			{
				if (Value + i <= calculateMax())
					temp.Add(Value + i);
			}

			//temp.Add(Value);

			//for (int i = 1; i <= offset; i++)
			//{
			//    if (Value + i <= calculateMax())
			//        temp.Add(Value + i);
			//}

			RangeOfValues = temp;
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

			gp.AddLine(secondarySliderX, secondarySliderVerticalCenter, secondarySliderX + secondarySliderWidth, secondarySliderVerticalCenter - sliderHeight / 2);
			//
			// line from middle left to top right (top half of <)
			//

			gp.AddLine(secondarySliderX + secondarySliderWidth, secondarySliderVerticalCenter - sliderHeight / 2, secondarySliderX + secondarySliderWidth, secondarySliderVerticalCenter + sliderHeight / 2);
			//
			// 
			//

			gp.AddLine(secondarySliderX, secondarySliderVerticalCenter, secondarySliderX + secondarySliderWidth, secondarySliderVerticalCenter + sliderHeight / 2);
			//
			//  >
			//

			

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
