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
	public partial class MultiValueSliderV2 : DensitySlider
	{
		public bool ClickedOnSlider
		{
			get { return clickedOnSlider; }
		}

		public new GraphicsPath SliderGP
		{
			get { return base.SliderGP; }
		}

		public MultiValueSliderV2()
		{
			InitializeComponent();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
		}

		public new int calculateMax()
		{
			return base.calculateMax();
		}
	}
}
