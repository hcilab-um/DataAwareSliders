using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication2
{
	public partial class Form2 : Form
	{
		public Form2()
		{
			InitializeComponent();
			activeAreaSliderv21.ValueChanged += new EventHandler(activeAreaSliderv21_ValueChanged);
			multiValueSliderV21.ValueChanged += new EventHandler(multiValueSliderV21_ValueChanged);
			mouseWheelSlider1.ValueChanged += new EventHandler(mouseWheelSlider1_ValueChanged);
			multiValueSliderV31.TextChanged += new EventHandler(multiValueSliderV31_TextChanged);


			activeAreaSliderv21.ItemsInIndices = new List<uint>(new uint[] { 10000, 5000, 2000, 4000, 3500 });


			List<string> list = new List<string>();
			try
			{
				// Create an instance of StreamReader to read from a file.
				// The using statement also closes the StreamReader.
				using (StreamReader sr = new StreamReader("dictionary.txt"))
				{
					String line;
					// Read and display lines from the file until the end of
					// the file is reached.
					while ((line = sr.ReadLine()) != null)
					{
						list.Add(line);
					}
				}
			}
			catch (Exception e)
			{
				// Let the user know what went wrong.
				Console.WriteLine("The file could not be read:");
				Console.WriteLine(e.Message);
			}
			//alphasliderV31.Data = list;
			alphasliderV31.ValueChanged += new EventHandler(alphasliderV31_ValueChanged);

			activeMultiSlider1.Data = list;

		}

		void alphasliderV31_ValueChanged(object sender, EventArgs e)
		{
			label1.Text = alphasliderV31.Value + "";
		}

		void multiValueSliderV31_TextChanged(object sender, EventArgs e)
		{
			label4.Text = multiValueSliderV31.Value + "";
		}

		void mouseWheelSlider1_ValueChanged(object sender, EventArgs e)
		{
			label6.Text = mouseWheelSlider1.Value + "";
			listBox1.BeginUpdate();
			listBox1.Items.Clear();
			foreach (int i in mouseWheelSlider1.RangeOfValues)
			{
				listBox1.Items.Add(i);
			}
			listBox1.EndUpdate();
		}

		void multiValueSliderV21_ValueChanged(object sender, EventArgs e)
		{
			//label4.Text = activeAreaSliderv21.Value + "";
			listBox1.BeginUpdate();
			listBox1.Items.Clear();
			foreach (int i in multiValueSliderV21.RangeOfValues)
			{
				listBox1.Items.Add(i);
			}
			listBox1.EndUpdate();
		}

		void activeAreaSliderv21_ValueChanged(object sender, EventArgs e)
		{
			label4.Text = activeAreaSliderv21.Value + "";
			listBox2.BeginUpdate();
			listBox2.Items.Clear();
			foreach (int i in activeAreaSliderv21.RangeOfValues)
			{
				listBox2.Items.Add(i);
			}
			listBox2.EndUpdate();
		}
	}
}
