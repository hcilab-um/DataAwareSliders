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
		private const int MINIMUM_ITEMS_IN_LIST = 5;

		private List<string> data = null;
        private bool valueRecentlyChanged = false;

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
                listBox.Hide();
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
			activeAreaSlider.MaxItemsPerSliderPixel = 7;
			activeAreaSlider.ValueChanged += new EventHandler(activeAreaSlider_ValueChanged);
			activeAreaSlider.StartMouseWheel += new EventHandler(activeAreaSlider_StartMouseWheel);
            activeAreaSlider.MouseUp += new MouseEventHandler(activeAreaSlider_MouseUp);
            activeAreaSlider.MouseDown += new MouseEventHandler(activeAreaSlider_MouseDown);
            activeAreaSlider.MouseLeave += new EventHandler(activeAreaSlider_MouseLeave);

            this.MouseClick += new MouseEventHandler(panel1_MouseClick);
            this.MouseLeave += new EventHandler(ActiveMultiSlider_MouseLeave);
			listBox.SelectedIndexChanged += new EventHandler(listBox_SelectedIndexChanged);
            listBox.MouseLeave += new EventHandler(listBox_MouseLeave);
			//activeAreaSlider.ItemsInIndices = new List<uint>(new uint[] { 1000, 5000, 2000, 4000, 3500, 1000, 5000, 2000, 4000, 3500, 1000, 5000, 2000, 4000, 3500, 1000, 5000, 2000, 4000, 3500 });

        }

        void activeAreaSlider_MouseLeave(object sender, EventArgs e)
        {
            ActiveMultiSlider_MouseLeave(sender, e);
        }

        void listBox_MouseLeave(object sender, EventArgs e)
        {
            ActiveMultiSlider_MouseLeave(sender, e);
        }

        void ActiveMultiSlider_MouseLeave(object sender, EventArgs e)
        {
            if (ClientRectangle.Contains(PointToClient( Cursor.Position)))
                return;
            else
                listBox.Hide();
        }

        void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!activeAreaSlider.ClientRectangle.Contains(e.Location))
            {
                listBox.Hide();
            }
        }

        void activeAreaSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if (activeAreaSlider.SliderGP.GetBounds().Contains(e.Location))
                listBox.Hide();
        }

        void activeAreaSlider_MouseUp(object sender, MouseEventArgs e)
        {
            if (valueRecentlyChanged || activeAreaSlider.SliderGP.GetBounds().Contains(e.Location))
            {
                listBox.Show();
            }
            else
                listBox.Hide();

            valueRecentlyChanged = false;
        }

		void activeAreaSlider_StartMouseWheel(object sender, EventArgs e)
		{
			int rollValueChange = 1;
            int itemsInList = listBox.Items.Count;//Math.Max(MINIMUM_ITEMS_IN_LIST, activeAreaSlider.ItemsPerSliderPixel);
			MouseEventArgs mouseInformation;

			if (e is MouseEventArgs)
			{
                listBox.Show();
				mouseInformation = (MouseEventArgs)e;

				if (mouseInformation.Delta > 0)
				{
                    if (Value == RangeOfValues[RangeOfValues.Count - 1])
                    {
                        rollValueChange = 1;
                    }
					else if (RangeOfValues[RangeOfValues.Count - 1] - Value < itemsInList)
					{
						rollValueChange = RangeOfValues[RangeOfValues.Count - 1] - Value;
					}
					else
					{
						rollValueChange = itemsInList - 1;
					}
				}
				else
				{
					if (Value == RangeOfValues[0])
					{
						//rollValueChange = itemsInList - 1;
                        rollValueChange = 1;
					}
					else if (Value - RangeOfValues[0] <= itemsInList)
					{
						rollValueChange = Value - RangeOfValues[0];
					}
					else
					{
                        rollValueChange = Math.Max(MINIMUM_ITEMS_IN_LIST, activeAreaSlider.ItemsPerSliderPixel) -1;
					}
				}
				
			}

			activeAreaSlider.RollChangeValue = rollValueChange;
		}

		void listBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			OnQueryChanged();
		}

		void activeAreaSlider_ValueChanged(object sender, EventArgs e)
		{
            Update();

			updateListBox();
			changeListBoxPosition();

            valueRecentlyChanged = true;
		}

		private void updateListBox()
		{
			if (data != null && data.Count > 0)
			{
                string itemBeingAdded;
                int columnWidth = 0;
                int stringWidth;
                Graphics g = listBox.CreateGraphics();

				listBox.BeginUpdate();
				listBox.Items.Clear();

				//listBox.Items.Add(data[activeAreaSlider.Value].ToString());
				for (int i = 0; i < Math.Max(activeAreaSlider.ItemsPerSliderPixel, MINIMUM_ITEMS_IN_LIST); i++)
				{
                    if (activeAreaSlider.Value + i <= activeAreaSlider.RangeOfValues[activeAreaSlider.RangeOfValues.Count - 1])
                    {
                        itemBeingAdded = data[activeAreaSlider.Value + i].ToString();

                        //stringWidth = (int)Math.Round(g.MeasureString(itemBeingAdded, listBox.Font).Width + 0.5);
                        //if (stringWidth > columnWidth)
                        //    columnWidth = stringWidth;

                        listBox.Items.Add(itemBeingAdded);
                    }
				}
				listBox.SelectedIndex = 0;

                //listBox.ColumnWidth = columnWidth;
               // listBox.HorizontalExtent = 2;
                //listBox.Width = (int)(Math.Round(listBox.Items.Count / 3.0 + 0.5)*(1.5*listBox.ColumnWidth));

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
                PointF sliderLocationPointF = activeAreaSlider.SliderGP.GetBounds().Location;
                int sliderX = (int)sliderLocationPointF.X + activeAreaSlider.Location.X;

				if (sliderX + activeAreaSlider.SliderGP.GetBounds().Width + DISTANCE_FROM_SLIDER_TO_LISTBOX + listBoxWidth > ClientRectangle.Width)
					newX = sliderX - DISTANCE_FROM_SLIDER_TO_LISTBOX - listBoxWidth;
				else
					newX = sliderX + (int)activeAreaSlider.SliderGP.GetBounds().Width + DISTANCE_FROM_SLIDER_TO_LISTBOX;
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
