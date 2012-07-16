namespace FilmFinder
{
	partial class testingRangeSlider
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.rangeSlider1 = new FilmFinder.RangeSlider();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// rangeSlider1
			// 
			this.rangeSlider1.Location = new System.Drawing.Point(12, 39);
			this.rangeSlider1.LowerBound = 0;
			this.rangeSlider1.LowerRange = 0;
			this.rangeSlider1.Name = "rangeSlider1";
			this.rangeSlider1.Size = new System.Drawing.Size(1807, 72);
			this.rangeSlider1.TabIndex = 0;
			this.rangeSlider1.Text = "rangeSlider1";
			this.rangeSlider1.UpperBound = 100;
			this.rangeSlider1.UpperRange = 100;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(88, 146);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			// 
			// testingRangeSlider
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1831, 448);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.rangeSlider1);
			this.Name = "testingRangeSlider";
			this.Text = "testingRangeSlider";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private RangeSlider rangeSlider1;
		private System.Windows.Forms.Label label1;
	}
}