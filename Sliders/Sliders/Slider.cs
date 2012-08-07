using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace CustomSlider
{
    public abstract partial class Slider : Control
    {
        #region Variables

        public event EventHandler ValueChanged;
        public new event MouseEventHandler MouseUp;
        public new event MouseEventHandler MouseMove;
        public new event MouseEventHandler MouseDown;

        public const int MINIMUM_SLIDER_WIDTH = 20;

        //The following is for changing the pointer speed
        private UInt32 defaultPointerSpeed = 10;
        private UInt32 slowedPointSpeed = 1;
        public const UInt32 SPI_SETMOUSESPEED = 0x0071;
        public const UInt32 SPI_GETMOUSESPEED = 0x0070;
        [DllImport("User32.dll")]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);
        [DllImport("User32.dll")]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref uint pvParam, uint fWinIni);

        private GraphicsPath sliderGP = null;
        private GraphicsPath customSliderGP = null;
        private GraphicsPath slideArea = null;
        private int sliderHeight = 0;
        private int sliderWidth = 0;

        private float trackWidth = 0;
        private float trackXStart = 0;
        private float trackXEnd = 0;
        private float trackYValue = 0;

        private int sliderValue = 0;
        private List<uint> itemsInIndices = new List<uint>(new uint[] { 100, 500, 900, 150, 330, 205, 506 }); //multipurpose. The count of this List indicates how many indices there are
        //and the value of each element indicates the number of elements associated with that index
        private List<char> indexCharacters = new List<char>(new char[] { 'a', 'b', 'c', 'd','e', 'f','g'});
        private List<int> rangeOfValues;
        private int offset = 0;

        private bool clickedOnSlider = false;
        private bool dragginSlider = true;

        #endregion

        #region Getters and Setters

        protected bool DraggingSlider
        {
            get { return dragginSlider; }
            set { dragginSlider = value; }
        }

        protected List<char> IndexCharacters
        {
            get { return indexCharacters; }
            set
            {
                indexCharacters = value;
            }
        }

        protected int Offset
        {
            get { return offset; }
            set 
            {
                if (value >= 0)
                    offset = value;
                else
                    offset = 0;
            }
        }

        protected List<int> RangeOfValues
        {
            get { return rangeOfValues; }
            set { rangeOfValues = value; }
        }

        protected bool ClickedOnSlider
        {
            get { return clickedOnSlider; }
            set { clickedOnSlider = value; }
        }

        public List<uint> ItemsInIndices
        {
            get { return itemsInIndices; }
            set { itemsInIndices = value; }
        }

        protected int Value
        {
            get { return sliderValue; }
            set
            {
                int max = calculateMax();
                if (value >= 0 && value <= max)
                {
                    sliderValue = value;
                }
                else if (value < 0)
                {
                    sliderValue = 0;
                }
                else if (value > max)
                {
                    sliderValue = max;
                }
            }
        }

        protected float TrackYValue
        {
            get { return trackYValue; }
            set { trackYValue = value; }
        }

        protected float TrackXEnd
        {
            get { return trackXEnd; }
            set { trackXEnd = value; }
        }

        protected float TrackXStart
        {
            get { return trackXStart; }
            set { trackXStart = value; }
        }

        protected float TrackWidth
        {
            get { return trackWidth; }
            set { trackWidth = value; }
        }

        protected GraphicsPath SliderGP
        {
            get { return sliderGP; }
            set { sliderGP = value; }
        }

        protected GraphicsPath CustomSliderGP
        {
            get { return customSliderGP; }
            set { customSliderGP = value; }
        }

        protected GraphicsPath SlideArea
        {
            get { return slideArea; }
            set { slideArea = value; }
        }

        protected int SliderHeight
        {
            get { return sliderHeight; }
            set { sliderHeight = value; }
        }

        protected int SliderWidth
        {
            get { return sliderWidth; }
            set { sliderWidth = value; }
        }

        #endregion

        #region Abstract Methods

        protected abstract void updateOffset(int value);
        protected abstract void updateRangeAroundValues(int value);
        protected abstract GraphicsPath generateSlideArea();
        protected abstract GraphicsPath generateSliderPath(float sliderCenterX, float sliderCenterY);

        #endregion

        public Slider()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.Selectable |
                     ControlStyles.SupportsTransparentBackColor | ControlStyles.UserMouse |
                     ControlStyles.UserPaint, true); //I got this directly from another example I found on the internet

            SystemParametersInfo(SPI_GETMOUSESPEED, 0, ref defaultPointerSpeed, 0);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        #region Event creators

        protected void OnNewMouseUp(MouseEventArgs e)
        {
            if (MouseUp != null)
                MouseUp(this, e);
        }

        protected void OnNewMouseDown(MouseEventArgs e)
        {
            if (MouseDown != null)
                MouseDown(this, e);
        }

        protected void OnNewMouseMove(MouseEventArgs e)
        {
            if (MouseMove != null)
                MouseMove(this, e);
        }

        protected void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, new EventArgs());
        }

        #endregion

        #region Mouse Gain

        protected void slowDownMouse()
        {
            //slow down the mouse
            SystemParametersInfo(SPI_SETMOUSESPEED, 0, slowedPointSpeed, 0);
        }

        protected void resetMouseSpeed()
        {
            //reset the speed of the mouse
            SystemParametersInfo(SPI_SETMOUSESPEED, 0, defaultPointerSpeed, 0);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// This method calculates the total number of items being mapped by the slider.
        /// This is done by looping through the itemsInIndices List and adding the value of each element
        /// </summary>
        /// <returns>An int representing the sum of values in itemsInIndices</returns>
        protected int calculateMax()
        {
            return calculateSum(itemsInIndices.Count - 1);
        }

        /// <summary>
        /// Calculates the sum of values up to and including a zero based index
        /// </summary>
        /// <param name="index">The index to calculate up to. This should be zero based meaning that to calculate up to the 3rd index of the list you should pass 2</param>
        /// <returns></returns>
        protected int calculateSum(int index)
        {
            int sum = 0;
            if (index < 0)
                return 0;
            else
            {
                for (int i = 0; i <= index; i++)
                {
                    sum += (int)itemsInIndices[i];
                }

                return sum;
            }
        }
        #endregion
    }
}
