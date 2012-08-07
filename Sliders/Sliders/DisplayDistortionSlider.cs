using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CustomSlider
{
    public abstract partial class DisplayDistortionSlider : Slider
    {
        #region Variables

        private List<uint> itemsPerIndex; //local copy
        private List<char> indexCharacters; //local copy
        private List<float> indexTickPixelLocation = null;
        private List<float[]> usedIndexPixels = null;
        private Font drawingFont = new Font(FontFamily.GenericMonospace, 12);
        private float fontWidth = 0;
        private float spaceForIndex = 20;

        private int minimum = 0;
        private int maximum;

        Point lastMousePosition;

        private bool drawSlider = true;
        private bool needToDoPaintingMath = true;

        #endregion

        #region Getters and Setters

        protected bool DrawSlider
        {
            get { return drawSlider; }
            set { drawSlider = value; }
        }

        protected bool NeedToDoPaintingMath
        {
            get { return needToDoPaintingMath; }
            set { needToDoPaintingMath = value; }
        }

        protected int Minimum
        {
            get { return minimum; }
        }

        protected int Maximum
        {
            get { return maximum; }
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
                    updateOffset(Value);
                }

                Invalidate();

                base.OnValueChanged();
            }
        }

        public new List<uint> ItemsInIndices
        {
            get { return base.ItemsInIndices; }
            set
            {
                base.ItemsInIndices = value;
                maximum = calculateMax();
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

        public DisplayDistortionSlider()
        {
            InitializeComponent();
            indexTickPixelLocation = new List<float>();
            usedIndexPixels = new List<float[]>();

            maximum = base.calculateMax();
        }

        #region Painting

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            Graphics g = pe.Graphics;
            fontWidth = g.MeasureString("G", drawingFont).Width;

            doPaintingMath();

            Pen blackPen = new Pen(Color.Black, 2);
            Brush blackBrush = new SolidBrush(Color.Black);

            //Create slide area
            base.SlideArea = generateSlideArea();
            g.DrawPath(blackPen, base.SlideArea);

            //Deal with slider positioning
            GraphicsPath currSliderGP = SliderGP;
            if (base.CustomSliderGP == null)
            {
                float sliderCenterY = base.SlideArea.GetBounds().Y + base.SlideArea.GetBounds().Height / 2;
                float sliderCenterX = base.SlideArea.GetBounds().Left + base.SliderWidth / 2;
                sliderCenterX += (base.Value - minimum) / (float)maximum * (base.SlideArea.GetBounds().Width - SliderWidth);
                
                if (drawSlider)
                    base.SliderGP = generateSliderPath(sliderCenterX, sliderCenterY);

                currSliderGP = SliderGP;
            }
            else
            {
                currSliderGP = CustomSliderGP;
            }

            g.FillPath(new SolidBrush(Color.FromArgb(128, Color.Gray)), currSliderGP);
            g.DrawPath(blackPen, currSliderGP);

            drawIndex(g);

        }

        protected void doPaintingMath()
        {
            if (needToDoPaintingMath)
            {
                base.SliderWidth = 30;
                base.SliderHeight = 15;

                base.TrackXStart = ClientRectangle.X + 1;
                base.TrackXEnd = ClientRectangle.Width - 1;
                base.TrackWidth = base.TrackXEnd - base.TrackXStart;

                needToDoPaintingMath = false;
            }
        }

        private void drawIndex(Graphics g)
        {
            if (base.IndexCharacters != null)
            {
                itemsPerIndex = new List<uint>(base.ItemsInIndices);
                indexCharacters = new List<char>(base.IndexCharacters);
                findIndexTickPixelLocations();

                //Font drawingFont = new Font(FontFamily.GenericMonospace, 12);
                Brush drawingBrush = new SolidBrush(Color.Black);
                sortIndexArrays();
                usedIndexPixels.Clear();

                //The following part is n^2 complexity but n will be <=36 so it shouldn't be too bad
                for (int i = 0; i < itemsPerIndex.Count; i++)
                {
                    if (canDrawCharacter(indexTickPixelLocation[i], indexCharacters[i]))
                    {
                        g.DrawString(indexCharacters[i] + "", drawingFont, drawingBrush, indexTickPixelLocation[i] - fontWidth / 2, base.SlideArea.GetBounds().Bottom);
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
            uint prevInt;
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

                        prevChar = indexCharacters[j];
                        indexCharacters.RemoveAt(j);
                        indexCharacters.Insert(j + 1, prevChar);

                        prevFloat = indexTickPixelLocation[j];
                        indexTickPixelLocation.RemoveAt(j);
                        indexTickPixelLocation.Insert(j + 1, prevFloat);
                    }
                }
            }
        }

        private void findIndexTickPixelLocations()
        {
            float effectiveTrackWidth = TrackWidth - base.SliderWidth;
            float itemsPerPixel = (maximum - minimum + 1) / effectiveTrackWidth;

            indexTickPixelLocation.Clear();
            //usedIndexPixels.Clear();

            indexTickPixelLocation.Add(base.SlideArea.GetBounds().X + base.SliderWidth / 2);
            //usedIndexPixels.Add(new float[] { indexTickPixelLocation[0] - fontWidth / 2, indexTickPixelLocation[0] + fontWidth / 2 });
            for (int i = 1; i < itemsPerIndex.Count; i++)
            {
                indexTickPixelLocation.Add(indexTickPixelLocation[i - 1] + itemsPerIndex[i - 1] / itemsPerPixel);
            }
        }

        #endregion

        #region Overidden Events

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
                    Cursor.Position = PointToScreen(new Point(newXValue, PointToClient(Cursor.Position).Y));
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

        private void processMouseLocation(Point e)
        {
            if (Capture && base.ClickedOnSlider && !lastMousePosition.Equals(Cursor.Position))
            {
                if (e.X < base.SlideArea.GetBounds().X + base.SliderWidth / 2)
                    Value = minimum;
                else if (e.X > base.SlideArea.GetBounds().Right - base.SliderWidth / 2)
                    Value = maximum;
                else //The mouse is somewhere between the beginning and end of the track
                {
                    //Find mouse penetration
                    float penetration = (e.X - (base.TrackXStart + base.SliderWidth / 2)) / (TrackWidth - base.SliderWidth);

                    //calculate value
                    int tempValue = (int)(penetration * maximum);

                    Value = tempValue;

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
            //drawSlider = true;
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

            float effectiveTrackWidth = TrackWidth - base.SliderWidth;
            float itemsPerPixel = (maximum - minimum + 1) / effectiveTrackWidth;

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

        protected override GraphicsPath generateSlideArea()
        {
            GraphicsPath slideArea = new GraphicsPath();

            PointF topLeft = new PointF(TrackXStart, ClientRectangle.Height - 1 - spaceForIndex - base.SliderHeight);
            PointF topRight = new PointF(TrackXEnd, ClientRectangle.Height - 1 - spaceForIndex - base.SliderHeight);
            PointF bottomLeft = new PointF(TrackXStart, ClientRectangle.Height - 1 - spaceForIndex);
            PointF bottomRight = new PointF(TrackXEnd, ClientRectangle.Height - 1 - spaceForIndex);


            slideArea.AddLine(topLeft, topRight);
            slideArea.AddLine(topRight, bottomRight);
            slideArea.AddLine(bottomRight, bottomLeft);
            slideArea.AddLine(bottomLeft, topLeft);

            return slideArea;
        }

        protected override GraphicsPath generateSliderPath(float sliderCenterX, float sliderCenterY)
        {
            GraphicsPath gp = new GraphicsPath();

            float leftX, rightX, topY, bottomY;

            //base.SliderHeight = (int)Math.Round(base.SlideArea.GetBounds().Height - base.SlideArea.GetBounds().Y);

            leftX = sliderCenterX - base.SliderWidth / 2;
            rightX = sliderCenterX + base.SliderWidth / 2;
            topY = sliderCenterY - base.SliderHeight / 2;
            bottomY = sliderCenterY + base.SliderHeight / 2;
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

        #endregion
    }
}
