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
	public partial class AlphaSliderV2 : DensitySlider
	{
		#region Variables

		private GraphicsPath sliderGP;

		private int baseFineThreshold = 1;
		private int fineThreshold = 1;				//how far the mouse has to move when dragging the "fine" portion before the value of the slider is changed
		private int fineValueChange = 1;
		private int baseMediumThreshold = 1;
		private int mediumThreshold = 1;			//same as above but for the "medium" portion of the slider
		private int mediumValueChange = 10;
		
		private int lastX;							//previous x value. When checking to see if we've crossed a threshold the current mouse position is compared against this value
		private bool clickedOnTopHalf = false;		//bool value indicating if the user mouse down-ed on the top half of slider
		private bool clickedOnBottomHalf = false;	//bool value indicating if the user mouse down-ed on the bottom half of slider
		private bool densityAware = true;
		private bool drawHistograms = true;
		private bool redrawMouse = false;
		#endregion

		#region Getters and Setters


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
				Value = Value;
			}
		}

		public bool DrawHistograms
		{
			get { return drawHistograms; }
			set { drawHistograms = value; Invalidate(); }
		}

		#endregion


		public AlphaSliderV2()
		{
			InitializeComponent();
			base.customSliderGP = new GraphicsPath();
		}

		#region Paint

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics g = e.Graphics;

			//Deal with slider positioning
			int indexOfSlider = findIndexOfSliderValue();
			float sliderX = trackXStart - sliderWidth/2;
			float proportion;
			if (indexOfSlider != -1)
			{
				//This will make the x value of the slider go to the right tick. From here it will be shifted over more
				//based on it's value and the number of items assocaited with that index
				for (int i = 0; i < indexOfSlider; i++)
				{
					sliderX += (int)(trackWidth / (float)ItemsInIndices.Count);
				}

				proportion = ((float)(Value - calculateSum(indexOfSlider - 1)) / (float)ItemsInIndices[indexOfSlider]);
				sliderX += (int)(proportion * ((float)(trackWidth / (float)ItemsInIndices.Count)));
			}

			//draw the slider
			SolidBrush sliderBrush = new SolidBrush(Color.Gray);
			Pen blackPen = new Pen(Color.Black);

			sliderGP = generateSliderPath(sliderX, trackYValue, sliderWidth);
			g.FillPath(sliderBrush, sliderGP);

			//outline slider
			g.DrawPath(blackPen, sliderGP);
			g.DrawLine(blackPen, sliderX, trackYValue, sliderX + sliderWidth, trackYValue);

			//Redraw mouse pointer over slider
			if (redrawMouse)
			{
				this.Cursor = new Cursor(Cursor.Current.Handle);
				Point tempPoint = new Point();
				tempPoint.X = (int)(sliderX + sliderWidth / 2);
				if (clickedOnTopHalf)
					tempPoint.Y = (int)trackYValue - (int)sliderHeight / 4;
				else if (clickedOnBottomHalf)
					tempPoint.Y = (int)trackYValue + (int)sliderHeight / 4;

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
			//base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left && mouseInSliderRegion(e.Location))
			{
				//Debug.WriteLine("Clicked inside mouse region");
				lastX = e.Location.X;
				Capture = true;
				clickedOnSlider = true;
				redrawMouse = true;

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
			//base.OnMouseMove(e);
			if (Capture && clickedOnSlider && (clickedOnTopHalf || clickedOnBottomHalf))
			{
				redrawMouse = true;

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
						Value = Value + valueChangeOnMove;
					}
					lastX = e.X;
				}
				else if ((e.X - lastX) <= -1 * moveThreshold)
				{
					for (int i = lastX; i > e.X; i -= moveThreshold)
					{
						Value = Value - valueChangeOnMove;
					}

					lastX = e.X;
				}

				//if (e.X < ClientRectangle.X + sliderWidth || e.X > ClientRectangle.Right - sliderWidth)
				//	redrawMouse = false;


			}

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
			clickedOnTopHalf = false;
			clickedOnBottomHalf = false;
			redrawMouse = false;
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
		/// This method updates the thresholds for fine and medium granularity dragging.
		/// </summary>
		private void updateThresholds()
		{
			int indexOfSlider = findIndexOfSliderValue();

			if (indexOfSlider >= 0)
			{
				double ratio = (double)ItemsInIndices[indexOfSlider] / (double)ItemsInIndices[largestIndex];
				mediumThreshold = (int)Math.Round(baseMediumThreshold / (ratio));
				//fineThreshold = (int)Math.Round(baseFineThreshold * (1 + 3*Math.Pow(ratio,3.0)));
                fineThreshold = 1;
			}
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
		/// This method checks to see if a certain point is within the bounds of the slider
		/// </summary>
		/// <param name="point">The point that is being checked</param>
		/// <returns>Returns true if the point is in the slider region, false otherwise</returns>
		protected new bool mouseInSliderRegion(Point point)
		{
			return sliderGP.GetBounds().Contains(point);
		}

		#endregion
	}
}
