namespace CustomSlider
{
	partial class IDListSlider
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDListSlider));
			this.label1 = new System.Windows.Forms.Label();
			this.listBox = new System.Windows.Forms.ListBox();
			this.IDMultiValueSlider = new CustomSlider.IDMultiValueSlider();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(47, 51);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "label1";
			// 
			// listBox1
			// 
			this.listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.listBox.BackColor = System.Drawing.SystemColors.Control;
			this.listBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox.FormattingEnabled = true;
			this.listBox.Location = new System.Drawing.Point(38, 3);
			this.listBox.Name = "listBox1";
			this.listBox.Size = new System.Drawing.Size(120, 78);
			this.listBox.TabIndex = 3;
			// 
			// multiValueSliderV21
			// 
			this.IDMultiValueSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IDMultiValueSlider.IndexCharacters = null;
			this.IDMultiValueSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("multiValueSliderV21.ItemsInIndices")));
			this.IDMultiValueSlider.Location = new System.Drawing.Point(3, 3);
			this.IDMultiValueSlider.Name = "multiValueSliderV21";
			this.IDMultiValueSlider.Size = new System.Drawing.Size(492, 45);
			this.IDMultiValueSlider.TabIndex = 0;
			this.IDMultiValueSlider.Text = "multiValueSliderV21";
			this.IDMultiValueSlider.Value = 0;
			// 
			// MultiValueSliderV3
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.listBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.IDMultiValueSlider);
			this.Name = "MultiValueSliderV3";
			this.Size = new System.Drawing.Size(498, 81);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private IDMultiValueSlider IDMultiValueSlider;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox listBox;
	}
}
