namespace CustomSlider
{
	partial class IDActiveListSlider
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDActiveListSlider));
            this.listBox = new System.Windows.Forms.ListBox();
            this.IDActiveAreaSlider = new CustomSlider.IDActiveAreaSlider();
            this.SuspendLayout();
            // 
            // listBox
            // 
            this.listBox.BackColor = System.Drawing.SystemColors.Control;
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(44, 3);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(87, 95);
            this.listBox.TabIndex = 1;
            // 
            // activeAreaSlider
            // 
            this.IDActiveAreaSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.IDActiveAreaSlider.DrawSlider = true;
            this.IDActiveAreaSlider.IndexCharacters = null;
            this.IDActiveAreaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("activeAreaSlider.ItemsInIndices")));
            this.IDActiveAreaSlider.Location = new System.Drawing.Point(8, 12);
            this.IDActiveAreaSlider.MaxItemsPerSliderPixel = 2;
            this.IDActiveAreaSlider.Name = "activeAreaSlider";
            this.IDActiveAreaSlider.RollChangeValue = 1;
            this.IDActiveAreaSlider.Size = new System.Drawing.Size(440, 46);
            this.IDActiveAreaSlider.TabIndex = 0;
            this.IDActiveAreaSlider.Text = "activeAreaSliderv21";
            this.IDActiveAreaSlider.Value = 0;
            // 
            // IDActiveListSlider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.IDActiveAreaSlider);
            this.Name = "IDActiveListSlider";
            this.Size = new System.Drawing.Size(458, 103);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.ListBox listBox;
        private IDActiveAreaSlider IDActiveAreaSlider;

    }
}
