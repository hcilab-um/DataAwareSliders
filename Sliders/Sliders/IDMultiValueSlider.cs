using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CustomSlider
{
	public partial class IDMultiValueSlider : InputDistortionSlider
	{
		public new bool ClickedOnSlider
		{
			get { return base.ClickedOnSlider; }
		}

		public new GraphicsPath SliderGP
		{
			get { return base.SliderGP; }
		}

		public IDMultiValueSlider()
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
	}
}
