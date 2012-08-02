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
			get { return multiValueSliderV21.RangeOfValues; }
		}

		public int SelectedIndex
		{
			get { return listBox1.SelectedIndex; }
		}

		public string SelectedItem
		{
			get
			{
				if (listBox1.Items[listBox1.SelectedIndex] is string)
					return (string)listBox1.Items[listBox1.SelectedIndex];
				else
					return "Data in listbox cannot be cast to a string";
			}
		}

		public List<uint> ItemsInIndices
		{
			set { multiValueSliderV21.ItemsInIndices = value; }
		}

		public List<string> IndexNames
		{
			get { return multiValueSliderV21.IndexNames; }
			set
			{
				multiValueSliderV21.IndexNames = value;
			}
		}

		public int Value
		{
			get { return multiValueSliderV21.Value; }
			set { multiValueSliderV21.Value = value; }
		}

		public ListBox ListBox
		{
			get { return listBox1; }
		}

		public bool ShowLabel
		{
			get { return showLabel; }
			set { showLabel = value; }
		}

		public IDListSlider()
		{
			InitializeComponent();

			SetStyle(ControlStyles.Selectable | ControlStyles.ResizeRedraw | ControlStyles.UserMouse | ControlStyles.FixedHeight, true);
			multiValueSliderV21.ValueChanged += new EventHandler(multiValueSliderV21_ValueChanged);
			multiValueSliderV21.MouseMove += new MouseEventHandler(multiValueSliderV21_MouseMove);

			label1.TextChanged += new EventHandler(label1_TextChanged);

			if (!showLabel)
				label1.Hide();

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
			listBox1.ScrollAlwaysVisible = true;
			listBox1.SelectedIndexChanged += new EventHandler(listBox1_SelectedIndexChanged);
			listBox1.BackColor = Color.FromArgb(255, listBox1.BackColor);
			//listBox1.
		}
		
		private void changeListBoxPosition()
		{
			int listBoxWidth = listBox1.Width;
			int newX = listBox1.Location.X;

			if (multiValueSliderV21.SliderGP != null)
			{
				if (multiValueSliderV21.SliderGP.GetBounds().Right + distanceFromSliderToListBox + listBoxWidth > ClientRectangle.Width)
					newX = (int)multiValueSliderV21.SliderGP.GetBounds().X - distanceFromSliderToListBox - listBoxWidth;
				else
					newX = (int)multiValueSliderV21.SliderGP.GetBounds().Right + distanceFromSliderToListBox;
			}
			listBox1.Location = new Point(newX, listBox1.Location.Y);
		}

		private void changeLabelPosition()
		{
			int labelWidth = label1.Width;
			int newX = label1.Location.X;

			if (multiValueSliderV21.SliderGP != null)
			{
				if (multiValueSliderV21.SliderGP.GetBounds().Right + distanceFromSliderToLabel + labelWidth > ClientRectangle.Width)
					newX = (int)multiValueSliderV21.SliderGP.GetBounds().X - distanceFromSliderToLabel - labelWidth;
				else
					newX = (int)multiValueSliderV21.SliderGP.GetBounds().Right + distanceFromSliderToLabel;
			}

			label1.Location = new Point(newX, label1.Location.Y);
		}

		private void updateLabelText()
		{
			label1.Text = list[multiValueSliderV21.Value].ToString();
		}

		private void updateListBoxConents()
		{
			listBox1.BeginUpdate();
			listBox1.Items.Clear();
			for (int i = multiValueSliderV21.RangeOfValues[0]; i <= multiValueSliderV21.RangeOfValues[multiValueSliderV21.RangeOfValues.Count - 1]; i++)
			{
				listBox1.Items.Add(list[i].ToString());
			}
			listBox1.SelectedIndex = 0;
			listBox1.EndUpdate();

			label1_TextChanged(this, new EventArgs());
		}

		private void initializeList()
		{
			list = new List<string>();

			int max = multiValueSliderV21.calculateMax();
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
			listBox1.Show();
			listBox1.Focus();
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
			listBox1.Show();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			
			if (e.X < 0 || e.X > ClientRectangle.Width || e.Y < 0 || e.Y > ClientRectangle.Height)
			{
				//listBox1.Hide();
				if (showLabel)
					label1.Show();
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
					listBox1.Show();
				}
			}
			base.OnMouseMove(e);
		}

		void multiValueSliderV21_MouseMove(object sender, MouseEventArgs e)
		{
			listBox1.Show();
		}


		void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			string tempString = listBox1.Items[listBox1.SelectedIndex].ToString();
			//if(showLabel) label1.Show();
			//listBox1.Hide();
			label1.Text = tempString;
			//label1.Hide();
		}

		void multiValueSliderV21_ValueChanged(object sender, EventArgs e)
		{
			multiValueSliderV21.Refresh();
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
