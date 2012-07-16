using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FilmFinder
{
	public partial class testingRangeSlider : Form
	{
		public testingRangeSlider()
		{
			InitializeComponent();
			rangeSlider1.BoundChanged += new EventHandler(rangeSlider1_BoundChanged);
			//rangeSlider1.UpperRange = 1000;
			//rangeSlider1.UpperBound = 1000;
			//rangeSlider1.LowerRange = 900;
		}

		void rangeSlider1_BoundChanged(object sender, EventArgs e)
		{
			label1.Text = rangeSlider1.LowerBound.ToString() + " to " + rangeSlider1.UpperBound.ToString();
		}
	}
}
