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
	public partial class AlphasliderV3 : DensitySlider
	{
		public new event EventHandler ValueChanged;

		private GraphicsPath slideArea = null;
		private GraphicsPath sliderThumb = null;
		private GraphicsPath leftButton = null;
		private GraphicsPath rightButton = null;
		private GraphicsPath leftArrow = null;
		private GraphicsPath rightArrow = null;

		private List<string> data = null;
		private List<int> itemsPerIndex = null;
		private List<char> indexChars = null;
		private List<float> indexTickPixelLocation = null;
		private List<float[]> usedIndexPixels = null;
		private Font drawingFont = new Font(FontFamily.GenericMonospace, 12);
		private int buttonWidth = 20;
		private int arrowWidth = 10;
		private int arrowHeight = 10;
		private float fontWidth = 0;

		private int spaceForIndex = 20;
		private int min = 0;
		private int max = 100;
		private int range = 10000;
		private int value = 50;

		private int lastX = 0;
		private bool clickedOnTopHalf = false;
		private bool clickedOnBottomHalf = false;
		private bool redrawMouse = false;
		private int moveThreshold = 1;
		private int fineValueChange = 1;
		private int mediumValueChange = 10;

		public List<string> Data
		{
			get { return data; }
			set
			{
				if (value != null)
				{
					data = value;
					updateMax();
					Invalidate();
				}
			}
		}

		public new int Value
		{
			get { return value; }
			set
			{
				//int max = calculateMax();
				if (value >= min && value <= max)
				{
					this.value = value;
				}
				else if (value < min)
				{
					this.value = min;
				}
				else if (value > max)
				{
					this.value = max;
				}

				Debug.WriteLine(this.value.ToString());

				if (this.ValueChanged != null)
					this.ValueChanged(this, new EventArgs());

				Invalidate();

			}
		}

		public AlphasliderV3()
		{
			InitializeComponent();
			itemsPerIndex = new List<int>();
			indexChars = new List<char>();
			indexTickPixelLocation = new List<float>();
			usedIndexPixels = new List<float[]>();
			range = max - min;
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			//base.OnPaint(pe);
			Graphics g = pe.Graphics;
			fontWidth = g.MeasureString("G", drawingFont).Width;

			doPaintingMath();

			Pen blackPen = new Pen(Color.Black, 2);
			Brush blackBrush = new SolidBrush(Color.Black);

			trackXStart = 1 + buttonWidth;
			trackXEnd = ClientRectangle.Width - 1 - buttonWidth;
			trackWidth = trackXEnd - trackXStart;

			//Create slide area
			slideArea = makeSlideArea();

			//create slider thumb
			sliderThumb = makeSliderThumb();
			customSliderGP = sliderThumb;

			//create buttons and arrows
			leftButton = makeLeftButton();
			rightButton = makeRightButton();
			leftArrow = makeLeftArrow();
			rightArrow = makeRightArrow();

			//draw them all
			g.DrawPath(blackPen, slideArea);

			g.DrawPath(blackPen, sliderThumb);
			g.DrawLine(blackPen, sliderThumb.GetBounds().X, sliderThumb.GetBounds().Y + sliderThumb.GetBounds().Height / 2, sliderThumb.GetBounds().Right, sliderThumb.GetBounds().Y + sliderThumb.GetBounds().Height / 2);

			g.DrawPath(blackPen, leftButton);
			g.DrawPath(blackPen, rightButton);

			g.FillPath(blackBrush, rightArrow);
			g.FillPath(blackBrush, leftArrow);

			//g.DrawString("abcdefghijklmnopqrstuvwxyz", new Font(FontFamily.GenericMonospace, 12), blackBrush, new PointF(ClientRectangle.X + 5, slideArea.GetBounds().Bottom + 3));
			//Debug.WriteLine(g.MeasureString("i", new Font(FontFamily.GenericMonospace, 12)).ToString());
			//Debug.WriteLine(g.MeasureString("G", new Font(FontFamily.GenericMonospace, 12)).ToString());
			drawIndex(g);
			
			//Redraw mouse pointer over slider
			if (redrawMouse)
			{
				this.Cursor = new Cursor(Cursor.Current.Handle);
				Point tempPoint = new Point();
				tempPoint.X = (int)(sliderThumb.GetBounds().X + sliderThumb.GetBounds().Width / 2);
				if (clickedOnTopHalf)
					tempPoint.Y = (int)(sliderThumb.GetBounds().Top + sliderThumb.GetBounds().Height / 4);
				else if (clickedOnBottomHalf)
					tempPoint.Y = (int)(sliderThumb.GetBounds().Top + 3 * sliderThumb.GetBounds().Height / 4);

				Cursor.Position = PointToScreen(tempPoint);
				lastX = tempPoint.X;
			}


		}

		private void drawIndex(Graphics g)
		{
			if (data != null)
			{
				findItemsPerIndex();
				findIndexTickPixelLocations();

				//Font drawingFont = new Font(FontFamily.GenericMonospace, 12);
				Brush drawingBrush = new SolidBrush(Color.Black);
				sortIndexArrays();
				usedIndexPixels.Clear();
				//The following part is n^2 complexity but n will be <=36 so it shouldn't be too bad
				for (int i = 0; i < itemsPerIndex.Count; i++)
				{
					if (canDrawCharacter(indexTickPixelLocation[i], indexChars[i]))
					{
						g.DrawString(indexChars[i] + "", drawingFont, drawingBrush, indexTickPixelLocation[i] - fontWidth / 2, slideArea.GetBounds().Bottom);
						usedIndexPixels.Add(new float[] { indexTickPixelLocation[i] - fontWidth / 2, indexTickPixelLocation[i] + fontWidth / 2 });
					}
				}
			}
		}

		private bool canDrawCharacter(float location, char ch)
		{
			bool result = true;

			for (int i = 0; i < usedIndexPixels.Count && result; i++)
			{
				if ((location >= usedIndexPixels[i][0] && location <= usedIndexPixels[i][1])
					|| (location - fontWidth / 2 >= usedIndexPixels[i][0] && location - fontWidth / 2 <= usedIndexPixels[i][1])
					|| (location + fontWidth / 2 >= usedIndexPixels[i][0] && location + fontWidth / 2 <= usedIndexPixels[i][1]))
				{
					result = false;
					Debug.WriteLine("Location: {0}\t\tLocation - fontWidth / 2: {1}\t\tLocation + fontWidth / 2: {2}", location, location - fontWidth / 2, location + fontWidth / 2);
					Debug.WriteLine("Lower: {0}\t\tUpper: {1}", usedIndexPixels[i][0], usedIndexPixels[i][1]);
					Debug.WriteLine("");
				}
			}

			return result;
		}

		private void sortIndexArrays()
		{
			char prevChar;
			int prevInt;
			float prevFloat;

			for (int i = 0; i < itemsPerIndex.Count; i++)
			{
				for (int j = 0; j < itemsPerIndex.Count - i - 1; j++)
				{
					if (itemsPerIndex[j] < itemsPerIndex[j + 1])
					{
						//Switch indices
						prevInt = itemsPerIndex[j];
						itemsPerIndex.RemoveAt(j);
						itemsPerIndex.Insert(j + 1, prevInt);

						prevChar = indexChars[j];
						indexChars.RemoveAt(j);
						indexChars.Insert(j + 1, prevChar);

						prevFloat = indexTickPixelLocation[j];
						indexTickPixelLocation.RemoveAt(j);
						indexTickPixelLocation.Insert(j + 1, prevFloat);
					}
				}
			}
		}

		private void findItemsPerIndex()
		{
			if (data != null)
			{
				char lastChar = '\0';
				itemsPerIndex.Clear();
				indexChars.Clear();

				for (int i = 0; i < data.Count; i++)
				{
					if (char.ToUpper(data[i][0]) != lastChar)
					{
						itemsPerIndex.Add(1);
						lastChar = char.ToUpper(data[i][0]);
						indexChars.Add(lastChar);
					}
					else
					{
						itemsPerIndex[itemsPerIndex.Count - 1]++;
					}
				}
			}
		}

		private void findIndexTickPixelLocations()
		{
			float effectiveTrackWidth = trackWidth - sliderThumb.GetBounds().Width;
			float itemsPerPixel = (max - min + 1) / effectiveTrackWidth;
			//float currentTickPosition;
			//float previousTickPosition;

			indexTickPixelLocation.Clear();
			//usedIndexPixels.Clear();

			indexTickPixelLocation.Add(slideArea.GetBounds().X + sliderThumb.GetBounds().Width / 2);
			//usedIndexPixels.Add(new float[] { indexTickPixelLocation[0] - fontWidth / 2, indexTickPixelLocation[0] + fontWidth / 2 });
			for (int i = 1; i < itemsPerIndex.Count; i++)
			{
				indexTickPixelLocation.Add(indexTickPixelLocation[i - 1] + itemsPerIndex[i - 1] / itemsPerPixel);
			}
		}

		#region Grahpics Paths

		private GraphicsPath makeSlideArea()
		{
			GraphicsPath slideArea = new GraphicsPath();

			slideArea.AddLine(trackXStart, 1, trackXEnd, 1);
			slideArea.AddLine(trackXEnd, 1, trackXEnd, ClientRectangle.Height - 1 - spaceForIndex);
			slideArea.AddLine(trackXEnd, ClientRectangle.Height - 1 - spaceForIndex, trackXStart, ClientRectangle.Height - 1 - spaceForIndex);
			slideArea.AddLine(trackXStart, ClientRectangle.Height - 1 - spaceForIndex, trackXStart, 1);

			return slideArea;
		}

		private GraphicsPath makeSliderThumb()
		{
			GraphicsPath sliderThumb = new GraphicsPath();

			float sliderX = slideArea.GetBounds().Left;
			sliderX += (value - min) / (float)range * (slideArea.GetBounds().Width - sliderWidth);

			sliderThumb.AddLine(sliderX, slideArea.GetBounds().Top, sliderX + sliderWidth, slideArea.GetBounds().Top);
			sliderThumb.AddLine(sliderX + sliderWidth, slideArea.GetBounds().Top, sliderX + sliderWidth, slideArea.GetBounds().Bottom);
			sliderThumb.AddLine(sliderX + sliderWidth, slideArea.GetBounds().Bottom, sliderX, slideArea.GetBounds().Bottom);
			sliderThumb.AddLine(sliderX, slideArea.GetBounds().Bottom, sliderX, slideArea.GetBounds().Top);

			return sliderThumb;
		}

		private GraphicsPath makeLeftButton()
		{
			GraphicsPath leftButton = new GraphicsPath();

			leftButton.AddLine(ClientRectangle.X + 1, ClientRectangle.Y + 1, slideArea.GetBounds().Left, ClientRectangle.Y + 1);
			leftButton.AddLine(slideArea.GetBounds().Left, ClientRectangle.Y + 1, slideArea.GetBounds().Left, slideArea.GetBounds().Bottom);
			leftButton.AddLine(slideArea.GetBounds().Left, slideArea.GetBounds().Bottom, ClientRectangle.X + 1, slideArea.GetBounds().Bottom);
			leftButton.AddLine(ClientRectangle.X + 1, slideArea.GetBounds().Bottom, ClientRectangle.X + 1, ClientRectangle.Y + 1);

			return leftButton;
		}

		private GraphicsPath makeRightButton()
		{
			GraphicsPath rightButton = new GraphicsPath();

			rightButton.AddLine(slideArea.GetBounds().Right, ClientRectangle.Y + 1, ClientRectangle.Width - 1, ClientRectangle.Y + 1);
			rightButton.AddLine(ClientRectangle.Width - 1, ClientRectangle.Y + 1, ClientRectangle.Width - 1, slideArea.GetBounds().Bottom);
			rightButton.AddLine(ClientRectangle.Width - 1, slideArea.GetBounds().Bottom, slideArea.GetBounds().Right, slideArea.GetBounds().Bottom);
			rightButton.AddLine(slideArea.GetBounds().Right, slideArea.GetBounds().Bottom, slideArea.GetBounds().Right, ClientRectangle.Y + 1);

			return rightButton;
		}

		private GraphicsPath makeLeftArrow()
		{
			GraphicsPath leftArrow = new GraphicsPath();

			PointF buttonCenter = new PointF(leftButton.GetBounds().X + leftButton.GetBounds().Width / 2, leftButton.GetBounds().Y + leftButton.GetBounds().Height / 2);
			float arrowLeft = buttonCenter.X - arrowWidth / 2;
			float arrowRight = buttonCenter.X + arrowWidth / 2;
			float arrowTop = buttonCenter.Y - arrowHeight / 2;
			float arrowBottom = buttonCenter.Y + arrowHeight / 2;

			leftArrow.AddLine(arrowRight, arrowTop, arrowLeft, arrowTop + arrowHeight / 2);
			leftArrow.AddLine(arrowLeft, arrowTop + arrowHeight / 2, arrowRight, arrowBottom);
			leftArrow.AddLine(arrowRight, arrowBottom, arrowRight, arrowTop);


			return leftArrow;
		}

		private GraphicsPath makeRightArrow()
		{
			GraphicsPath rightArrow = new GraphicsPath();

			PointF buttonCenter = new PointF(rightButton.GetBounds().X + rightButton.GetBounds().Width / 2, rightButton.GetBounds().Y + rightButton.GetBounds().Height / 2);
			float arrowLeft = buttonCenter.X - arrowWidth / (float)2;
			float arrowRight = buttonCenter.X + arrowWidth / (float)2;
			float arrowTop = buttonCenter.Y - arrowHeight / (float)2;
			float arrowBottom = buttonCenter.Y + arrowHeight / (float)2;

			rightArrow.AddLine(arrowLeft, arrowTop, arrowRight, arrowTop + arrowHeight / 2);
			rightArrow.AddLine(arrowRight, arrowTop + arrowHeight / 2, arrowLeft, arrowBottom);
			rightArrow.AddLine(arrowLeft, arrowBottom, arrowLeft, arrowTop);


			return rightArrow;
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
			if (e.Button == MouseButtons.Left)
			{
				if (mouseInSliderRegion(e.Location))
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
					slowDownMouse();
				}
				else if (mouseInSlideArea(e.Location))
				{
					if (e.X < slideArea.GetBounds().X + sliderThumb.GetBounds().Width / 2)
						Value = 0;
					else if (e.X > slideArea.GetBounds().Right - sliderThumb.GetBounds().Width / 2)
						Value = max;
					else //The mouse is somewhere between the beginning and end of the track
					{
						//Find mouse penetration
						float penetration = (e.X - (sliderThumb.GetBounds().Width / 2 + buttonWidth)) / (trackWidth - sliderThumb.GetBounds().Width);

						//calculate value
						int tempValue = (int)(penetration * max);

						Value = tempValue;

					}
				}
				else if (leftButton.GetBounds().Contains(e.Location))
				{
					Value = Value - 1;
					Debug.WriteLine(Value.ToString());
				}
				else if (rightButton.GetBounds().Contains(e.Location))
				{
					Value = Value + 1;
					Debug.WriteLine(Value.ToString() + "helo");
				}
			}
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);
			if (rightButton.GetBounds().Contains(e.Location))
			{
				Value = Value + 1;
				Debug.WriteLine(Value.ToString() + "helo  22");
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
				int valueChangeOnMove;

				if (clickedOnTopHalf)
				{
					valueChangeOnMove = mediumValueChange;
				}
				else
				{
					valueChangeOnMove = fineValueChange;
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
					//for (int i = lastX; i < e.X; i += moveThreshold)
					//{
						Value = Value + (e.X - lastX) * valueChangeOnMove;
					//}
					lastX = e.X;
				}
				else if ((e.X - lastX) <= -1 * moveThreshold)
				{
					//for (int i = lastX; i > e.X; i -= moveThreshold)
					//{
						Value = Value - (lastX - e.X) * valueChangeOnMove;
					//}

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

			resetMouseSpeed();
		}

		#endregion

		/// <summary>
		/// This method checks to see if a point is in the slider's top half. It is assumed that the point has already been determined to be within the bounds of the slider.
		/// </summary>
		/// <param name="point">The point to be checked</param>
		/// <returns>Returns true if the point is in the top half of the slider. False if it isn't (this means that it must be in the bottom half)</returns>
		private bool mouseInTopHalf(Point point)
		{
			RectangleF sliderRectangle = sliderThumb.GetBounds();

			return point.Y <= sliderRectangle.Top + sliderRectangle.Height / 2;
		}

		private bool mouseInSlideArea(Point point)
		{
			return slideArea.GetBounds().Contains(point);
		}

		private void updateMax()
		{
			max = data.Count - 1;
			range = max - min;
		}
	}
}
