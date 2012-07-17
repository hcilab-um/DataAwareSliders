using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			button2.PerformClick();

			alphaSlider1.ValueChanged += new EventHandler(alphaSlider1_ValueChanged);
			alphaSlider2.ValueChanged += new EventHandler(alphaSlider2_ValueChanged);
			alphaSlider2.DensityAware = true;
			alphaSlider1.DrawHistograms = false;

			button1_Click(button1, new EventArgs());
			button3_Click(button3, new EventArgs());
			button7_Click(button7, new EventArgs());
			button7_Click(button7, new EventArgs());
			button8_Click(button8, new EventArgs());
			button8_Click(button8, new EventArgs());
		}

		void alphaSlider2_ValueChanged(object sender, EventArgs e)
		{
			label4.Text = "" + alphaSlider2.Value;
			label5.Text = "Fine Threshold: " + alphaSlider2.FineThreshold;
			label6.Text = "Medium Threshold: " + alphaSlider2.MediumThreshold;
		}

		void alphaSlider1_ValueChanged(object sender, EventArgs e)
		{
			label1.Text = "" + alphaSlider1.Value;
			label2.Text = "Fine Threshold: " + alphaSlider1.FineThreshold;
			label3.Text = "Medium Threshold: " + alphaSlider1.MediumThreshold;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			alphaSlider1.Value = 0;
			//listBox1.Items.Add(string.Format("changed value to {0}", alphaSlider1.Value));
		}

		private void button2_Click(object sender, EventArgs e)
		{
			alphaSlider1.Value = 10000;
			//listBox1.Items.Add(string.Format("changed value to {0}", alphaSlider1.Value));
		}

		private void button3_Click(object sender, EventArgs e)
		{
			alphaSlider2.Value = 0;
		}

		private void button4_Click(object sender, EventArgs e)
		{
			alphaSlider2.Value = 10000;
		}

		private void button5_Click(object sender, EventArgs e)
		{
			alphaSlider1.DrawHistograms = !alphaSlider1.DrawHistograms;
		}

		private void button6_Click(object sender, EventArgs e)
		{
			alphaSlider2.DrawHistograms = !alphaSlider2.DrawHistograms;
		}

		private void button7_Click(object sender, EventArgs e)
		{
			alphaSlider1.DensityAware = !alphaSlider1.DensityAware;
			label7.Text = "" + alphaSlider1.DensityAware;
		}

		private void button8_Click(object sender, EventArgs e)
		{
			alphaSlider2.DensityAware = !alphaSlider2.DensityAware;
			label8.Text = "" + alphaSlider2.DensityAware;
		}

		private void button9_Click(object sender, EventArgs e)
		{
			List<uint> newInfo = new List<uint>();
			string text = textBox1.Text;
			string[] parse = text.Split(' ');

			if (parse.Length > 0)
			{
				foreach (string str in parse)
				{
					newInfo.Add(uint.Parse(str));
				}

				alphaSlider1.ItemsInIndices = newInfo;
				alphaSlider2.ItemsInIndices = newInfo;
			}			
		}
	}
}
