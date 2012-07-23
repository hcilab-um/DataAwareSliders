namespace CustomSlider
{
	partial class ActiveMultiSlider
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActiveMultiSlider));
			this.listBox = new System.Windows.Forms.ListBox();
			this.currentRangeLabel = new System.Windows.Forms.Label();
			this.activeAreaSlider = new CustomSlider.ActiveAreaSliderv2();
			this.SuspendLayout();
			// 
			// listBox
			// 
			this.listBox.BackColor = System.Drawing.SystemColors.Control;
			this.listBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listBox.FormattingEnabled = true;
			this.listBox.ItemHeight = 16;
			this.listBox.Location = new System.Drawing.Point(44, 3);
			this.listBox.Name = "listBox";
			this.listBox.Size = new System.Drawing.Size(105, 100);
			this.listBox.TabIndex = 1;
			// 
			// currentRangeLabel
			// 
			this.currentRangeLabel.AutoSize = true;
			this.currentRangeLabel.Location = new System.Drawing.Point(3, 62);
			this.currentRangeLabel.Name = "currentRangeLabel";
			this.currentRangeLabel.Size = new System.Drawing.Size(35, 13);
			this.currentRangeLabel.TabIndex = 2;
			this.currentRangeLabel.Text = "label1";
			// 
			// activeAreaSlider
			// 
			this.activeAreaSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.activeAreaSlider.DrawSlider = true;
			this.activeAreaSlider.IndexNames = null;
			this.activeAreaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("activeAreaSlider.ItemsInIndices")));
			this.activeAreaSlider.Location = new System.Drawing.Point(3, 4);
			this.activeAreaSlider.MaxItemsPerSliderPixel = 1;
			this.activeAreaSlider.Name = "activeAreaSlider";
			this.activeAreaSlider.RollChangeValue = 1;
			this.activeAreaSlider.Size = new System.Drawing.Size(452, 45);
			this.activeAreaSlider.TabIndex = 0;
			this.activeAreaSlider.Text = "activeAreaSliderv21";
			this.activeAreaSlider.Value = 0;
			// 
			// ActiveMultiSlider
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.currentRangeLabel);
			this.Controls.Add(this.listBox);
			this.Controls.Add(this.activeAreaSlider);
			this.Name = "ActiveMultiSlider";
			this.Size = new System.Drawing.Size(458, 109);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ActiveAreaSliderv2 activeAreaSlider;
		private System.Windows.Forms.ListBox listBox;
		private System.Windows.Forms.Label currentRangeLabel;
	}
}
