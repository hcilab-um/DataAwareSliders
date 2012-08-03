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
			set { DDMultiValueSlider.Value = value; }
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
			DDMultiValueSlider.ValueChanged += new EventHandler(multiValueSliderV21_ValueChanged);
			DDMultiValueSlider.MouseMove += new MouseEventHandler(multiValueSliderV21_MouseMove);

			label.TextChanged += new EventHandler(label1_TextChanged);

			if (!showLabel)
				label.Hide();

			changeListBoxPosition();

			initializeList();
			initializeListBox();

			
		}
		

		void label1_TextChanged(object sender, EventArgs e)
		{
			if (TextChanged != null)
				TextChanged(this, new EventArgs());
		}

		private void initializeListBox()
		{
			//listBox1.Hide();
			listBox.ScrollAlwaysVisible = true;
			listBox.SelectedIndexChanged += new EventHandler(listBox1_SelectedIndexChanged);
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

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			//outline client rectangle
			//Pen outlinePen = new Pen(Color.Red);
			//e.Graphics.DrawRectangle(outlinePen, 0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
		}

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

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			//base.OnMouseLeave(e);
			//listBox1.Hide();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			listBox.Show();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			
			if (e.X < 0 || e.X > ClientRectangle.Width || e.Y < 0 || e.Y > ClientRectangle.Height)
			{
				//listBox1.Hide();
				if (showLabel)
					label.Show();
			}
			else
			{
				if (e.Button == MouseButtons.Left)
				{
					//listBox1.Hide();
				}
				else
				{
					changeListBoxPosition();
					listBox.Show();
				}
			}
			base.OnMouseMove(e);
		}

		void multiValueSliderV21_MouseMove(object sender, MouseEventArgs e)
		{
			listBox.Show();
		}


		void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			string tempString = listBox.Items[listBox.SelectedIndex].ToString();
			//if(showLabel) label1.Show();
			//listBox1.Hide();
			label.Text = tempString;
			//label1.Hide();
		}

		void multiValueSliderV21_ValueChanged(object sender, EventArgs e)
		{
			DDMultiValueSlider.Refresh();
			//multiValueSliderV21.Invalidate();

			updateListBoxConents();

			updateLabelText();

			//changeLabelPosition();
			changeListBoxPosition();
			Invalidate();
		}
		#endregion
    }
}
