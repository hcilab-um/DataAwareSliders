namespace CustomSlider
{
	partial class Form1
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.idAlphaslider1 = new CustomSlider.IDAlphaslider();
			this.idActiveAreaSlider1 = new CustomSlider.IDActiveAreaSlider();
			this.SuspendLayout();
			// 
			// idAlphaslider1
			// 
			this.idAlphaslider1.IndexCharacters = ((System.Collections.Generic.List<char>)(resources.GetObject("idAlphaslider1.IndexCharacters")));
			this.idAlphaslider1.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("idAlphaslider1.ItemsInIndices")));
			this.idAlphaslider1.Location = new System.Drawing.Point(25, 53);
			this.idAlphaslider1.Name = "idAlphaslider1";
			this.idAlphaslider1.Size = new System.Drawing.Size(445, 112);
			this.idAlphaslider1.TabIndex = 0;
			this.idAlphaslider1.Text = "idAlphaslider1";
			this.idAlphaslider1.Value = 0;
			// 
			// idActiveAreaSlider1
			// 
			this.idActiveAreaSlider1.DrawSlider = true;
			this.idActiveAreaSlider1.IndexCharacters = ((System.Collections.Generic.List<char>)(resources.GetObject("idActiveAreaSlider1.IndexCharacters")));
			this.idActiveAreaSlider1.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("idActiveAreaSlider1.ItemsInIndices")));
			this.idActiveAreaSlider1.Location = new System.Drawing.Point(25, 171);
			this.idActiveAreaSlider1.MaxItemsPerSliderPixel = 2;
			this.idActiveAreaSlider1.Name = "idActiveAreaSlider1";
			this.idActiveAreaSlider1.RollChangeValue = 1;
			this.idActiveAreaSlider1.Size = new System.Drawing.Size(486, 79);
			this.idActiveAreaSlider1.TabIndex = 1;
			this.idActiveAreaSlider1.Text = "idActiveAreaSlider1";
			this.idActiveAreaSlider1.Value = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(523, 262);
			this.Controls.Add(this.idActiveAreaSlider1);
			this.Controls.Add(this.idAlphaslider1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private IDAlphaslider idAlphaslider1;
		private IDActiveAreaSlider idActiveAreaSlider1;

	}
}