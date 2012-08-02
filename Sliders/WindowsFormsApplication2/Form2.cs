using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CustomSlider.WindowsFormsApplication2
{
	public partial class Form2 : Form
	{
		public Form2()
		{
			InitializeComponent();
			activeAreaSliderv21.ValueChanged += new EventHandler(activeAreaSliderv21_ValueChanged);
			multiValueSliderV31.TextChanged += new EventHandler(multiValueSliderV31_TextChanged);
            idAlphaslider1.ValueChanged += idAlphaslider1_ValueChanged;

			activeAreaSliderv21.ItemsInIndices = new List<uint>(new uint[] { 10000, 5000, 2000, 4000, 3500 });
			alphasliderV31.ValueChanged += new EventHandler(alphasliderV31_ValueChanged);

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

            List<char> firstCharacters = new List<char>();
            List<uint> buckets = new List<uint>();
            char lastFirstLetter = '\0';
            int lastIndex = 0;

            buckets.Add(0); //filter equivalent of "All"
            lastFirstLetter = char.ToUpper(list[1][0]); //prime the loop and variables
            firstCharacters.Add(lastFirstLetter);
            for (int i = 1; i < list.Count; i++)
            {
                if (char.ToUpper(list[i][0]) == lastFirstLetter)
                {
                    buckets[lastIndex]++;
                }
                else
                {
                    lastFirstLetter = char.ToUpper(list[i][0]);
                    firstCharacters.Add(lastFirstLetter);
                    buckets.Add(1);

                    lastIndex++;
                }
            }

            activeAreaSliderv21.ItemsInIndices = buckets;
            activeAreaSliderv21.IndexCharacters = firstCharacters;

            activeMultiSlider1.ItemsInIndices = buckets;
            activeMultiSlider1.IndexCharacters = firstCharacters;
            activeMultiSlider1.Data = list;

            idAlphaslider1.ItemsInIndices = buckets;
            idAlphaslider1.IndexCharacters = firstCharacters;

            alphasliderV31.ItemsInIndices = buckets;
            alphasliderV31.IndexCharacters = firstCharacters;
		}

        void idAlphaslider1_ValueChanged(object sender, EventArgs e)
        {
            label3.Text = idAlphaslider1.Value.ToString();
        }

		void alphasliderV31_ValueChanged(object sender, EventArgs e)
		{
			label1.Text = alphasliderV31.Value + "";
		}

		void multiValueSliderV31_TextChanged(object sender, EventArgs e)
		{
			label2.Text = multiValueSliderV31.Value + "";
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

		private void resetButton_Click(object sender, EventArgs e)
		{
			activeAreaSliderv21.Value = 0;
			multiValueSliderV31.Value = 0;
		}
	}
}
