using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace CustomSlider
{
	public partial class ActiveMultiSlider : UserControl
	{
		public event EventHandler QueryChanged;

		private const int ListBoxY = 0;
		private const int distanceFromSliderToListBox = 10;

		private List<string> data = null;

		public List<string> Data
		{
			get { return data; }
			set
			{
				data = value;
				Invalidate();
			}
		}

		public int Value
		{
			get { return activeAreaSlider.Value; }
			set { activeAreaSlider.Value = value; }
		}

		public List<uint> ItemsInIndices
		{
			set { activeAreaSlider.ItemsInIndices = value; }
		}

		public List<string> IndexNames
		{
			get { return activeAreaSlider.IndexNames; }
			set
			{
				activeAreaSlider.IndexNames = value;
			}
		}

		public List<int> RangeOfValues
		{
			get { return activeAreaSlider.RangeOfValues; }
		}

		public int SelectedIndex
		{
			get { return listBox.SelectedIndex; }
		}

		public string SelectedItem
		{
			get 
			{
				if (listBox.Items[listBox.SelectedIndex] is string)
					return (string)listBox.Items[listBox.SelectedIndex];
				else
					return "Data in listbox cannot be cast to a string";
			}
		}

		public ActiveMultiSlider()
		{
			InitializeComponent();
			activeAreaSlider.ValueChanged += new EventHandler(activeAreaSlider_ValueChanged);
			listBox.SelectedIndexChanged += new EventHandler(listBox_SelectedIndexChanged);

			//activeAreaSlider.ItemsInIndices = new List<uint>(new uint[] { 1000, 5000, 2000, 4000, 3500, 1000, 5000, 2000, 4000, 3500, 1000, 5000, 2000, 4000, 3500, 1000, 5000, 2000, 4000, 3500 });
		}

		void listBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			OnQueryChanged();
		}

		void activeAreaSlider_ValueChanged(object sender, EventArgs e)
		{
			activeAreaSlider.Refresh();
			updateListBox();
			changeListBoxPosition();
		}

		private void updateListBox()
		{
			if (data != null && data.Count > 0)
			{
				listBox.BeginUpdate();
				listBox.Items.Clear();

				listBox.Items.Add(data[activeAreaSlider.Value].ToString());
				for (int i = 1; i < activeAreaSlider.ItemsPerSliderPixel; i++)
				{
					if(activeAreaSlider.Value + i <= activeAreaSlider.RangeOfValues[activeAreaSlider.RangeOfValues.Count - 1])
						listBox.Items.Add(data[activeAreaSlider.Value + i].ToString());
				}
				listBox.SelectedIndex = 0;
				listBox.EndUpdate();

				OnQueryChanged();
			}
		}

		private void changeListBoxPosition()
		{
			int listBoxWidth = listBox.Width;
			int newX = listBox.Location.X;

			if (activeAreaSlider.SliderGP != null)
			{
				if (activeAreaSlider.SliderGP.GetBounds().Right + distanceFromSliderToListBox + listBoxWidth > ClientRectangle.Width)
					newX = (int)activeAreaSlider.SliderGP.GetBounds().X - distanceFromSliderToListBox - listBoxWidth;
				else
					newX = (int)activeAreaSlider.SliderGP.GetBounds().Right + distanceFromSliderToListBox;
			}
			listBox.Location = new Point(newX, listBox.Location.Y);
		}

		private void OnQueryChanged()
		{
			if (QueryChanged != null)
				QueryChanged(this, new EventArgs());
		}
	}
}
