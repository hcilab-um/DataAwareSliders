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
	public partial class IDListSlider : UserControl
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
			get { return label1.Text; }
		}

		public List<int> RangeOfValues
		{
			get { return IDMultiValueSlider.RangeOfValues; }
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
			set { IDMultiValueSlider.ItemsInIndices = value; }
		}

		public List<char> IndexNames
		{
			get { return IDMultiValueSlider.IndexCharacters; }
			set
			{
				IDMultiValueSlider.IndexCharacters = value;
			}
		}

		public int Value
		{
			get { return IDMultiValueSlider.Value; }
			set 
			{ 
				IDMultiValueSlider.Value = value;
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

        public IDListSlider()
		{
			InitializeComponent();

			SetStyle(ControlStyles.Selectable | ControlStyles.ResizeRedraw | ControlStyles.UserMouse | ControlStyles.FixedHeight, true);
			IDMultiValueSlider.ValueChanged += new EventHandler(IDMultiValueSlider_ValueChanged);
			IDMultiValueSlider.MouseUp += new MouseEventHandler(IDMultiValueSlider_MouseUp);
			IDMultiValueSlider.MouseDown += new MouseEventHandler(IDMultiValueSlider_MouseDown);
			IDMultiValueSlider.MouseLeave += new EventHandler(IDMultiValueSlider_MouseLeave);

			this.MouseClick += new MouseEventHandler(IDListSlider_MouseClick);
			this.MouseLeave += new EventHandler(IDListSlider_MouseLeave);

			listBox.MouseLeave += new EventHandler(listBox_MouseLeave);

			label1.TextChanged += new EventHandler(label1_TextChanged);

			if (!showLabel)
				label1.Hide();

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

			if (IDMultiValueSlider.SliderGP != null)
			{
				if (IDMultiValueSlider.SliderGP.GetBounds().Right + distanceFromSliderToListBox + listBoxWidth > ClientRectangle.Width)
					newX = (int)IDMultiValueSlider.SliderGP.GetBounds().X - distanceFromSliderToListBox - listBoxWidth;
				else
					newX = (int)IDMultiValueSlider.SliderGP.GetBounds().Right + distanceFromSliderToListBox;
			}
			listBox.Location = new Point(newX, listBox.Location.Y);
		}

		private void changeLabelPosition()
		{
			int labelWidth = label1.Width;
			int newX = label1.Location.X;

			if (IDMultiValueSlider.SliderGP != null)
			{
				if (IDMultiValueSlider.SliderGP.GetBounds().Right + distanceFromSliderToLabel + labelWidth > ClientRectangle.Width)
					newX = (int)IDMultiValueSlider.SliderGP.GetBounds().X - distanceFromSliderToLabel - labelWidth;
				else
					newX = (int)IDMultiValueSlider.SliderGP.GetBounds().Right + distanceFromSliderToLabel;
			}

			label1.Location = new Point(newX, label1.Location.Y);
		}

		private void updateLabelText()
		{
			label1.Text = list[IDMultiValueSlider.Value].ToString();
		}

		private void updateListBoxConents()
		{
			listBox.BeginUpdate();
			listBox.Items.Clear();
			for (int i = IDMultiValueSlider.RangeOfValues[0]; i <= IDMultiValueSlider.RangeOfValues[IDMultiValueSlider.RangeOfValues.Count - 1]; i++)
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

			int max = IDMultiValueSlider.calculateMax();
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
			listBox.Focus();
			base.OnMouseWheel(e);
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
			label1.Text = tempString;
			//label1.Hide();
		}

		void IDMultiValueSlider_ValueChanged(object sender, EventArgs e)
		{
			IDMultiValueSlider.Update();

			updateListBoxConents();

			updateLabelText();

			//changeLabelPosition();
			changeListBoxPosition();
			Invalidate();

			valueRecentlyChanged = true;
		}

		void IDMultiValueSlider_MouseLeave(object sender, EventArgs e)
		{
			IDListSlider_MouseLeave(sender, e);
		}

		void listBox_MouseLeave(object sender, EventArgs e)
		{
			IDListSlider_MouseLeave(sender, e);
		}

		void IDListSlider_MouseLeave(object sender, EventArgs e)
		{
			if (ClientRectangle.Contains(PointToClient(Cursor.Position)))
				return;
			else
				listBox.Hide();
		}

		void IDListSlider_MouseClick(object sender, MouseEventArgs e)
		{
			if (!IDMultiValueSlider.ClientRectangle.Contains(e.Location))
			{
				listBox.Hide();
			}
		}

		void IDMultiValueSlider_MouseDown(object sender, MouseEventArgs e)
		{
			if (IDMultiValueSlider.SliderGP.GetBounds().Contains(e.Location))
				listBox.Show();
		}

		void IDMultiValueSlider_MouseUp(object sender, MouseEventArgs e)
		{
			if (valueRecentlyChanged || IDMultiValueSlider.SliderGP.GetBounds().Contains(e.Location))
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
