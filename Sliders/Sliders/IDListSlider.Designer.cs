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
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.multiValueSliderV21 = new CustomSlider.IDMultiValueSlider();
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
			this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.listBox1.BackColor = System.Drawing.SystemColors.Control;
			this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(38, 3);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(120, 78);
			this.listBox1.TabIndex = 3;
			// 
			// multiValueSliderV21
			// 
			this.multiValueSliderV21.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.multiValueSliderV21.IndexCharacters = null;
			this.multiValueSliderV21.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("multiValueSliderV21.ItemsInIndices")));
			this.multiValueSliderV21.Location = new System.Drawing.Point(3, 3);
			this.multiValueSliderV21.Name = "multiValueSliderV21";
			this.multiValueSliderV21.Size = new System.Drawing.Size(492, 45);
			this.multiValueSliderV21.TabIndex = 0;
			this.multiValueSliderV21.Text = "multiValueSliderV21";
			this.multiValueSliderV21.Value = 0;
			// 
			// MultiValueSliderV3
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.multiValueSliderV21);
			this.Name = "MultiValueSliderV3";
			this.Size = new System.Drawing.Size(498, 81);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private IDMultiValueSlider multiValueSliderV21;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox listBox1;
	}
}
