using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CustomSlider
{
    public abstract partial class DisplayDistortionSlider : Slider
    {
        public DisplayDistortionSlider()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }


        protected override void updateOffset(int value)
        {
            throw new NotImplementedException();
        }

        protected override void updateRangeAroundValues(int value)
        {
            throw new NotImplementedException();
        }

        protected override System.Drawing.Drawing2D.GraphicsPath generateSlideArea()
        {
            throw new NotImplementedException();
        }
    }
}
