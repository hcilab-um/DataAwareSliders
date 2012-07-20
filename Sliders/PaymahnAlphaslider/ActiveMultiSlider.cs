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

		private const int LISTBOX_Y = 0;
		private const int DISTANCE_FROM_SLIDER_TO_LISTBOX = 10;
		private const int LABEL_BOUND_WIDTH = 5; //how many characters of the upper bound and lower should be shown

		private List<string> data = null;

		#region Getters and setters

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
			set 
			{
				activeAreaSlider.DrawSlider = true;
				activeAreaSlider.Value = value;
			}
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

		#endregion

		public ActiveMultiSlider()
		{
			InitializeComponent();
			activeAreaSlider.ValueChanged += new EventHandler(activeAreaSlider_ValueChanged);
			listBox.SelectedIndexChanged += new EventHandler(listBox_SelectedIndexChanged);

			//activeAreaSlider.ItemsInIndices = new List<uint>(new uint[] { 1000, 5000, 2000, 4000, 3500, 1000, 5000, 2000, 4000, 3500, 1000, 5000, 2000, 4000, 3500, 1000, 5000, 2000, 4000, 3500 });
			currentRangeLabel.Hide();
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

			updateCurrentRangeLabel();
			changeCurrentRangeLabelPosition();

			this.Invalidate();
		}

		private void updateCurrentRangeLabel()
		{
			if (RangeOfValues != null && data != null)
			{
				string lowerBound = data[RangeOfValues[0]];
				string upperBound = data[RangeOfValues[RangeOfValues.Count - 1]];

				if (lowerBound.Length > LABEL_BOUND_WIDTH)
					lowerBound = lowerBound.Substring(0, LABEL_BOUND_WIDTH);

				if (upperBound.Length > LABEL_BOUND_WIDTH)
					upperBound = upperBound.Substring(0, LABEL_BOUND_WIDTH);

				currentRangeLabel.Text = lowerBound + " -\n" + upperBound;
			}
		}

		private void changeCurrentRangeLabelPosition()
		{
			//currentRangeLabel.Left = (int)activeAreaSlider.SliderGP.GetBounds().X;
			if (activeAreaSlider.SliderGP != null)
			{
				Rectangle labelRec = currentRangeLabel.ClientRectangle;
				Rectangle listboxRec = listBox.ClientRectangle;

				labelRec.Offset((int)activeAreaSlider.SliderGP.GetBounds().X, currentRangeLabel.Location.Y);
				listboxRec.Offset(listBox.Location);

				Debug.WriteLine("Intersection of label and listbox: {0} ", labelRec.IntersectsWith(listboxRec));

				while (labelRec.IntersectsWith(listboxRec))
				{
					labelRec.Offset(-1, 0);
				}

				while (labelRec.Right > ClientRectangle.Width)
				{
					labelRec.Offset(-1, 0);
				}

				currentRangeLabel.Location = labelRec.Location;
			}
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
				if (activeAreaSlider.SliderGP.GetBounds().Right + DISTANCE_FROM_SLIDER_TO_LISTBOX + listBoxWidth > ClientRectangle.Width)
					newX = (int)activeAreaSlider.SliderGP.GetBounds().X - DISTANCE_FROM_SLIDER_TO_LISTBOX - listBoxWidth;
				else
					newX = (int)activeAreaSlider.SliderGP.GetBounds().Right + DISTANCE_FROM_SLIDER_TO_LISTBOX;
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
