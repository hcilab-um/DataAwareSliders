using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CustomSlider
{
    public partial class DDListSlider : UserControl
    {
        public new event EventHandler TextChanged;

		private List<string> list;
		private int distanceFromSliderToLabel = 10;
		private int distanceFromSliderToListBox = 10;
		private bool showLabel = false;
		private bool valueRecentlyChanged = false;

        #region Getters and Setters

        public List<string> List
		{
			get { return list; }
			set
			{
				list = value;
				Invalidate();
			}
		}

		public string LabelText
		{
			get { return label.Text; }
		}

		public List<int> RangeOfValues
		{
			get { return DDMultiValueSlider.RangeOfValues; }
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

		public List<uint> ItemsInIndices
		{
			set { DDMultiValueSlider.ItemsInIndices = value; }
		}

		public List<char> IndexNames
		{
			get { return DDMultiValueSlider.IndexCharacters; }
			set
			{
				DDMultiValueSlider.IndexCharacters = value;
			}
		}

		public int Value
		{
			get { return DDMultiValueSlider.Value; }
			set 
			{ 
				DDMultiValueSlider.Value = value;
				listBox.Hide();
			}
		}

		public ListBox ListBox
		{
			get { return listBox; }
		}

		public bool ShowLabel
		{
			get { return showLabel; }
			set { showLabel = value; }
		}

        #endregion

        public DDListSlider()
		{
			InitializeComponent();

			SetStyle(ControlStyles.Selectable | ControlStyles.ResizeRedraw | ControlStyles.UserMouse | ControlStyles.FixedHeight, true);
			DDMultiValueSlider.ValueChanged += new EventHandler(DDMultiValueSlider_ValueChanged);
			DDMultiValueSlider.MouseUp += new MouseEventHandler(DDMultiValueSlider_MouseUp);
			DDMultiValueSlider.MouseDown += new MouseEventHandler(DDMultiValueSlider_MouseDown);
			DDMultiValueSlider.MouseLeave += new EventHandler(DDMultiValueSlider_MouseLeave);

			this.MouseClick += new MouseEventHandler(DDListSlider_MouseClick);
			this.MouseLeave += new EventHandler(DDListSlider_MouseLeave);

			listBox.MouseLeave += new EventHandler(listBox_MouseLeave);

			label.TextChanged += new EventHandler(label1_TextChanged);

			if (!showLabel)
				label.Hide();

			changeListBoxPosition();

			initializeList();
			initializeListBox();

			
		}

		private void initializeListBox()
		{
			//listBox1.Hide();
			listBox.ScrollAlwaysVisible = true;
			listBox.SelectedIndexChanged += new EventHandler(listBox_SelectedIndexChanged);
			listBox.BackColor = Color.FromArgb(255, listBox.BackColor);
			//listBox1.
		}
		
		private void changeListBoxPosition()
		{
			int listBoxWidth = listBox.Width;
			int newX = listBox.Location.X;

			if (DDMultiValueSlider.SliderGP != null)
			{
				if (DDMultiValueSlider.SliderGP.GetBounds().Right + distanceFromSliderToListBox + listBoxWidth > ClientRectangle.Width)
					newX = (int)DDMultiValueSlider.SliderGP.GetBounds().X - distanceFromSliderToListBox - listBoxWidth;
				else
					newX = (int)DDMultiValueSlider.SliderGP.GetBounds().Right + distanceFromSliderToListBox;
			}
			listBox.Location = new Point(newX, listBox.Location.Y);
		}

		private void changeLabelPosition()
		{
			int labelWidth = label.Width;
			int newX = label.Location.X;

			if (DDMultiValueSlider.SliderGP != null)
			{
				if (DDMultiValueSlider.SliderGP.GetBounds().Right + distanceFromSliderToLabel + labelWidth > ClientRectangle.Width)
					newX = (int)DDMultiValueSlider.SliderGP.GetBounds().X - distanceFromSliderToLabel - labelWidth;
				else
					newX = (int)DDMultiValueSlider.SliderGP.GetBounds().Right + distanceFromSliderToLabel;
			}

			label.Location = new Point(newX, label.Location.Y);
		}

		private void updateLabelText()
		{
			label.Text = list[DDMultiValueSlider.Value].ToString();
		}

		private void updateListBoxConents()
		{
			listBox.BeginUpdate();
			listBox.Items.Clear();
			for (int i = DDMultiValueSlider.RangeOfValues[0]; i <= DDMultiValueSlider.RangeOfValues[DDMultiValueSlider.RangeOfValues.Count - 1]; i++)
			{
				listBox.Items.Add(list[i].ToString());
			}
			listBox.SelectedIndex = 0;
			listBox.EndUpdate();

			label1_TextChanged(this, new EventArgs());
		}

		private void initializeList()
		{
			list = new List<string>();

			int max = DDMultiValueSlider.calculateMax();
			for (int i = 0; i <= max; i++)
			{
				list.Add(i + "");
			}
		}

		#region Overridden Events

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			listBox.Show();

			listBox.BeginUpdate();

			if (e.Delta > 0)
			{
				if (listBox.SelectedIndex > 0)
					listBox.SelectedIndex--;
				else
				{
					if (DDMultiValueSlider.RangeOfValues[0] != 0)
					{
						RectangleF slider = DDMultiValueSlider.SliderGP.GetBounds();
						Point simulatedMouseLocation = new Point((int)Math.Round(slider.X + slider.Width / 2) - 1, (int)Math.Round(slider.Y + slider.Height / 2));
						DDMultiValueSlider.processMouseLocation(simulatedMouseLocation);
						listBox.SelectedIndex = listBox.Items.Count - 1;
					}
				}
			}
			else
			{
				if (listBox.SelectedIndex < listBox.Items.Count - 1)
					listBox.SelectedIndex++;
				else
				{
					if (DDMultiValueSlider.RangeOfValues[DDMultiValueSlider.RangeOfValues.Count - 1] != DDMultiValueSlider.calculateMax())
					{
						RectangleF slider = DDMultiValueSlider.SliderGP.GetBounds();
						Point simulatedMouseLocation = new Point((int)Math.Round(slider.X + slider.Width / 2) + 1, (int)Math.Round(slider.Y + slider.Height / 2));
						DDMultiValueSlider.processMouseLocation(simulatedMouseLocation);
					}
				}
			}

			listBox.EndUpdate();

			//Having "Invalidate();" wouldn't update the position of the scrollbar. A test with refresh did allow the scrollbar to move.
			Refresh();
		}

		void label1_TextChanged(object sender, EventArgs e)
		{
			if (TextChanged != null)
				TextChanged(this, new EventArgs());
		}

		void listBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			string tempString = listBox.Items[listBox.SelectedIndex].ToString();
			//if(showLabel) label1.Show();
			//listBox1.Hide();
			label.Text = tempString;
			//label1.Hide();
		}

		void DDMultiValueSlider_ValueChanged(object sender, EventArgs e)
		{
			DDMultiValueSlider.Update();

			updateListBoxConents();

			updateLabelText();

			//changeLabelPosition();
			changeListBoxPosition();
			Invalidate();

			valueRecentlyChanged = true;
		}

		void DDMultiValueSlider_MouseLeave(object sender, EventArgs e)
		{
			DDListSlider_MouseLeave(sender, e);
		}

		void listBox_MouseLeave(object sender, EventArgs e)
		{
			DDListSlider_MouseLeave(sender, e);
		}

		void DDListSlider_MouseLeave(object sender, EventArgs e)
		{
			if (ClientRectangle.Contains(PointToClient(Cursor.Position)))
				return;
			else
				listBox.Hide();
		}

		void DDListSlider_MouseClick(object sender, MouseEventArgs e)
		{
			if (!DDMultiValueSlider.ClientRectangle.Contains(e.Location))
			{
				listBox.Hide();
			}
		}

		void DDMultiValueSlider_MouseDown(object sender, MouseEventArgs e)
		{
			if (DDMultiValueSlider.SliderGP.GetBounds().Contains(e.Location))
				listBox.Show();
		}

		void DDMultiValueSlider_MouseUp(object sender, MouseEventArgs e)
		{
			if (valueRecentlyChanged || DDMultiValueSlider.SliderGP.GetBounds().Contains(e.Location))
			{
				listBox.Show();
			}
			else
				listBox.Hide();

			valueRecentlyChanged = false;
		}
		#endregion
    }
}
