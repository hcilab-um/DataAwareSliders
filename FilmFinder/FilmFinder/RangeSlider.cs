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

namespace FilmFinder
{
	public partial class RangeSlider : Control
	{
		#region Event Stuff

		public event EventHandler BoundChanged;

		private void OnBoundChanged()
		{
			if (BoundChanged != null)
				BoundChanged(this, new EventArgs());
		}

		#endregion

		private int lowerBound = 0;
		private int upperBound = 100;
		private int lowerRange = 0;
		private int upperRange = 100;
		private int range = 101;
		private GraphicsPath lowerSliderGP = null;
		private GraphicsPath upperSliderGP = null;
		private bool clickedOnLowerSlider = false;
		private bool clickedOnUpperSlider = false;
		private bool lastClickedOnLowerSlider = false;
		//private bool lastClickedOnUpperSlider = false;
		private int trackWidth = 0;

		private const int SLIDER_WIDTH = 10;
		private const int SLIDER_HEIGHT = 10;


		public int LowerBound
		{
			get { return lowerBound; }
			set
			{
				//if (value != lowerBound)
				//{
					if (value > upperBound)
						lowerBound = upperBound;
					else if (value < lowerRange)
						lowerBound = lowerRange;
					else
						lowerBound = value;

					OnBoundChanged();
					Invalidate();
				//}
			}
		}

		public int UpperBound
		{
			get { return upperBound; }
			set
			{
				//if (value != upperBound)
				//{
					if (value < lowerBound)
						upperBound = lowerBound;
					else if (value > upperRange)
						upperBound = upperRange;
					else
						upperBound = value;

					OnBoundChanged();
					Invalidate();
				//}
			}
		}

		public int LowerRange
		{
			get { return lowerRange; }
			set
			{
				if (value != lowerRange)
				{
					if (value > upperRange)
						lowerRange = upperRange;
					else
						lowerRange = value;

					if (lowerRange > lowerBound)
						LowerBound = lowerRange; //call the setter, make sure the math is done properly

					updateRange();
					Invalidate();
				}
			}
		}

		public int UpperRange
		{
			get { return upperRange; }
			set
			{
				if (value != upperRange)
				{
					if (value < lowerRange)
						upperRange = lowerRange;
					else
						upperRange = value;

					if (upperRange < upperBound)
						UpperBound = upperRange; //call the setter, make sure the math is done properly

					updateRange();
					Invalidate();
				}
			}
		}


		public RangeSlider()
		{
			InitializeComponent();

			SetStyle(ControlStyles.UserPaint | ControlStyles.Selectable | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer
				| ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor, true);

		}

		#region Painting

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);

			Graphics g = pe.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			

			Pen thinRedPen = new Pen(Color.Red);
			Pen mediumBlackPen = new Pen(Color.Black, 2);
			Pen mediumWhitePen = new Pen(Color.White, 2);
			Pen thickPurplePen = new Pen(Color.Purple, 3);
			SolidBrush blackBrush = new SolidBrush(Color.Blue);

			//g.DrawRectangle(thinRedPen, 0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1); //outline the client rectangle

			int trackYValue = ClientRectangle.Height / 2;
			int trackLeftX = SLIDER_WIDTH;
			int trackRightX = ClientRectangle.Width - 1 - SLIDER_WIDTH;
			trackWidth = trackRightX - trackLeftX;
			g.DrawLine(mediumBlackPen, trackLeftX, trackYValue, trackRightX, trackYValue);

			//for (int i = trackLeftX; i <= trackRightX; i += (trackWidth / range))
			//{
			//    g.DrawLine(mediumBlackPen, i, trackYValue, i, trackYValue + 10);
			//}

			int lowerSliderRightX = (int)Math.Round(((lowerBound - lowerRange) * 1.0 / range) * trackWidth) + trackLeftX; //position of the rightmost x value of the lowerbound slider
			int upperSliderLeftX = (int)Math.Round((1 - (upperRange - upperBound) * 1.0 / range) * trackWidth) + trackLeftX; //position of the leftmost x value of the upperbound slider

			lowerSliderGP = generateLowerSliderGP(lowerSliderRightX, trackYValue);
			upperSliderGP = generateUpperSliderGP(upperSliderLeftX, trackYValue);

			g.FillPath(blackBrush, lowerSliderGP);
			g.FillPath(blackBrush, upperSliderGP);

			g.DrawLine(thickPurplePen, lowerSliderRightX, trackYValue, upperSliderLeftX, trackYValue);

			if (clickedOnLowerSlider || clickedOnUpperSlider)
			{
				this.Cursor = new Cursor(Cursor.Current.Handle);
				Point tempPoint = new Point();
				tempPoint.Y = PointToClient(Cursor.Position).Y;

				if (clickedOnLowerSlider)
				{
					tempPoint.X = lowerSliderRightX - SLIDER_WIDTH / 2;
				}
				else
				{
					tempPoint.X = upperSliderLeftX + SLIDER_WIDTH / 2;
				}

				//Cursor.Position = PointToScreen( tempPoint);
			}
		}

		private GraphicsPath generateUpperSliderGP(int upperSliderLeftX, int upperSliderVerticalCenter)
		{
			GraphicsPath sliderPath = new GraphicsPath();

			sliderPath.AddLine(upperSliderLeftX, upperSliderVerticalCenter, upperSliderLeftX + SLIDER_WIDTH, upperSliderVerticalCenter - SLIDER_HEIGHT / 2);
			//sliderPath.AddLine(upperSliderLeftX, upperSliderVerticalCenter, upperSliderLeftX + SLIDER_WIDTH, upperSliderVerticalCenter + SLIDER_HEIGHT / 2);
			sliderPath.AddLine(upperSliderLeftX + SLIDER_WIDTH, upperSliderVerticalCenter - SLIDER_HEIGHT / 2, upperSliderLeftX + SLIDER_WIDTH, upperSliderVerticalCenter + SLIDER_HEIGHT / 2);

			return sliderPath;
		}

		private GraphicsPath generateLowerSliderGP(int lowerSliderRightX, int lowerSliderVerticalCenter)
		{
			GraphicsPath sliderPath = new GraphicsPath();

			sliderPath.AddLine(lowerSliderRightX, lowerSliderVerticalCenter, lowerSliderRightX - SLIDER_WIDTH, lowerSliderVerticalCenter - SLIDER_HEIGHT / 2);
			//sliderPath.AddLine(lowerSliderRightX, lowerSliderVerticalCenter, lowerSliderRightX - SLIDER_WIDTH, lowerSliderVerticalCenter + SLIDER_HEIGHT / 2);
			sliderPath.AddLine(lowerSliderRightX - SLIDER_WIDTH, lowerSliderVerticalCenter - SLIDER_HEIGHT / 2, lowerSliderRightX - SLIDER_WIDTH, lowerSliderVerticalCenter + SLIDER_HEIGHT / 2);

			return sliderPath;
		}

		#endregion

		#region Overridden Events

		/// <summary>
		/// This method enables slider dragging
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left && (mouseInLowerSlider(e.Location) || mouseInUpperSlider(e.Location)))
			{
				Capture = true;

				if (mouseInLowerSlider(e.Location))
				{
					clickedOnLowerSlider = true;
					lastClickedOnLowerSlider = true;

					clickedOnUpperSlider = false;
					//lastClickedOnUpperSlider = false;
				}
				else if(mouseInUpperSlider(e.Location))
				{
					clickedOnUpperSlider = true;
					//lastClickedOnUpperSlider = true;

					clickedOnLowerSlider = false;
					lastClickedOnLowerSlider = false;
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
			if (Capture && (clickedOnLowerSlider || clickedOnUpperSlider))
			{
				int newValue;

				newValue = (int)Math.Round(((e.X - SLIDER_WIDTH) * 1.0 / trackWidth) * range) + lowerRange;

				if (clickedOnLowerSlider)
					LowerBound = newValue;
				else
					UpperBound = newValue;
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
			clickedOnLowerSlider = false;
			clickedOnUpperSlider = false;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			Debug.WriteLine("Key down!");
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
					if (lastClickedOnLowerSlider)
						LowerBound = lowerBound - 1;
					else
						UpperBound = upperBound - 1;
					break;
				case Keys.Right:
					if (lastClickedOnLowerSlider)
						LowerBound = lowerBound + 1;
					else
						UpperBound = upperBound + 1;
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

		private void updateRange()
		{
			range = upperRange - lowerRange;
		}

		private bool mouseInLowerSlider(Point point)
		{
			return lowerSliderGP.GetBounds().Contains(point);
		}

		private bool mouseInUpperSlider(Point point)
		{
			return upperSliderGP.GetBounds().Contains(point);
		}

		#endregion
	}
}
