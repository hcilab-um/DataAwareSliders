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
    public partial class IDAlphaslider : InputDistortionSlider
    {
        private GraphicsPath leftButton = null;
        private GraphicsPath rightButton = null;
        private GraphicsPath leftArrow = null;
        private GraphicsPath rightArrow = null;

        private int buttonWidth = 20;
        private int buttonHeight = 20;
        private int arrowWidth = 10;
        private int arrowHeight = 10;

        private int lastX;
        private int fineValueChange = 1;
        private int coarseValueChange = 10;
        private int moveThreshold = 1;

        private bool redrawMouse = false;
        private bool clickedOnTopHalf = false;
        private bool clickedOnBottomHalf = false;

        public IDAlphaslider()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;

            doPaintingMath();

            Pen blackPen = new Pen(Color.Black, 2);
            Brush blackBrush = new SolidBrush(Color.Black);

            base.TrackXStart += buttonWidth;
            base.TrackXEnd -= buttonWidth;
            base.TrackWidth = base.TrackXEnd - base.TrackXStart;

            base.OnPaint(pe);

            //create buttons and arrows
            leftButton = makeLeftButton();
            rightButton = makeRightButton();
            leftArrow = makeLeftArrow();
            rightArrow = makeRightArrow();

            //draw them all
            g.DrawPath(blackPen, leftButton);
            g.DrawPath(blackPen, rightButton);

            g.FillPath(blackBrush, rightArrow);
            g.FillPath(blackBrush, leftArrow);

            RectangleF sliderRectangle = base.SliderGP.GetBounds();

            pe.Graphics.DrawLine(new Pen(Color.Black, 2), sliderRectangle.X, sliderRectangle.Y + sliderRectangle.Height / 2,
                sliderRectangle.Right, sliderRectangle.Y + sliderRectangle.Height / 2);

            //Redraw mouse pointer over slider
            if (redrawMouse)
            {
                this.Cursor = new Cursor(Cursor.Current.Handle);
                Point tempPoint = new Point();
                tempPoint.X = (int)(base.SliderGP.GetBounds().X + base.SliderGP.GetBounds().Width / 2);
                if (clickedOnTopHalf)
                    tempPoint.Y = (int)(base.SliderGP.GetBounds().Top + base.SliderGP.GetBounds().Height / 4);
                else if (clickedOnBottomHalf)
                    tempPoint.Y = (int)(base.SliderGP.GetBounds().Top + 3 * base.SliderGP.GetBounds().Height / 4);

                Cursor.Position = PointToScreen(tempPoint);
                lastX = tempPoint.X;

                base.LastMousePosition = Cursor.Position;
            }

            NeedToDoPaintingMath = true;
        }

        #region Overridden Mouse Event

        /// <summary>
        /// This method enables slider dragging
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (ClickedOnSlider)
            {
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

                redrawMouse = true;
                lastX = PointToClient(Cursor.Position).X; //the mouse is snapped to the center of the slider
            }
            else if (leftButton.GetBounds().Contains(e.Location))
            {
                Value = Value - 1;
            }
            else if (rightButton.GetBounds().Contains(e.Location))
            {
                Value = Value + 1;
            }

            

        }

        /// <summary>
        /// This method allows the slider to be dragged, but only if it's been clicked on
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            //base.OnMouseMove(e);
            if (base.ClickedOnSlider && (clickedOnTopHalf || clickedOnBottomHalf) && !base.LastMousePosition.Equals(Cursor.Position))
            {
                int valueChangePerMouseMovement;
                int actualValueChange = 0;

                if (clickedOnTopHalf)
                {
                    valueChangePerMouseMovement = coarseValueChange;
                }
                else
                {
                    valueChangePerMouseMovement = fineValueChange;
                }

                if ((e.X - lastX) >= moveThreshold)
                {
                    /** 
                     * If the user jerks the mouse the pointer might move 100 pixels and only register a 
                     * single event instead of 10 (or 100) events that are expected.
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
                    actualValueChange = (e.X - lastX) * valueChangePerMouseMovement;
                }
                else if ((e.X - lastX) <= -1 * moveThreshold)
                {
                    actualValueChange =  -1*(lastX - e.X) * valueChangePerMouseMovement;
                }

                Value += actualValueChange;
                lastX = e.X;

            }
        }

        /// <summary>
        /// This method disables slider movement
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            redrawMouse = false;
            base.OnMouseUp(e);
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
            RectangleF sliderRectangle = base.SliderGP.GetBounds();

            return point.Y <= sliderRectangle.Top + sliderRectangle.Height / 2;
        }

        private GraphicsPath makeLeftButton()
        {
            GraphicsPath leftButton = new GraphicsPath();

            PointF topLeft = new PointF(ClientRectangle.X + 1, base.TrackYValue - buttonHeight / 2);
            PointF topRight = new PointF(base.SlideArea.GetBounds().Left - base.SliderWidth / 2, base.TrackYValue - buttonHeight / 2);
            PointF bottomLeft = new PointF(ClientRectangle.X + 1, base.TrackYValue + buttonHeight / 2);
            PointF bottomRight = new PointF(base.SlideArea.GetBounds().Left - base.SliderWidth / 2, base.TrackYValue + buttonHeight / 2);

            leftButton.AddLine(topLeft, topRight);
            leftButton.AddLine(topRight, bottomRight);
            leftButton.AddLine(bottomRight, bottomLeft);
            leftButton.AddLine(bottomLeft, topLeft);

            return leftButton;
        }

        private GraphicsPath makeRightButton()
        {
            GraphicsPath rightButton = new GraphicsPath();

            PointF topLeft = new PointF(base.SlideArea.GetBounds().Right + base.SliderWidth / 2, base.TrackYValue - buttonHeight / 2);
            PointF topRight = new PointF(ClientRectangle.Width - 1, base.TrackYValue - buttonHeight / 2);
            PointF bottomLeft = new PointF(base.SlideArea.GetBounds().Right + base.SliderWidth / 2, base.TrackYValue + buttonHeight / 2);
            PointF bottomRight = new PointF(ClientRectangle.Width - 1, base.TrackYValue + buttonHeight / 2);

            rightButton.AddLine(topLeft, topRight); 
            rightButton.AddLine(topRight, bottomRight);
            rightButton.AddLine(bottomRight, bottomLeft);
            rightButton.AddLine(bottomLeft, topLeft);

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
    }
}
