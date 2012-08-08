using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			idActiveAreaSlider1.ItemsInIndices = new List<uint> { 100, 10, 1000, 200, 300, 10, 400 };
			idActiveAreaSlider1.ValueChanged += new EventHandler(idActiveAreaSlider1_ValueChanged);
		}

		void idActiveAreaSlider1_ValueChanged(object sender, EventArgs e)
		{
			label1.Text = idActiveAreaSlider1.Value + "";
			label2.Text = idActiveAreaSlider1.RangeOfValues[0] + " - " + idActiveAreaSlider1.RangeOfValues[idActiveAreaSlider1.RangeOfValues.Count - 1];
		}
	}
}
