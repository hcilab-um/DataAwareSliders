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
    public partial class DDMultiValueSlider : DisplayDistortionSlider
    {
        public new bool ClickedOnSlider
		{
			get { return base.ClickedOnSlider; }
		}

		public new GraphicsPath SliderGP
		{
			get { return base.SliderGP; }
		}

		public DDMultiValueSlider()
		{
			InitializeComponent();
		}

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            NeedToDoPaintingMath = true;
        }

		public new int calculateMax()
		{
			return base.calculateMax();
		}

		public new void processMouseLocation(Point mouseLocation)
		{
			base.ClickedOnSlider = true;
			base.processMouseLocation(mouseLocation);
			base.ClickedOnSlider = false;
		}
    }
}
