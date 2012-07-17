namespace TestingSlider
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
			this.alphaSlider1 = new CustomSlider.AlphaSlider();
			this.SuspendLayout();
			// 
			// alphaSlider1
			// 
			this.alphaSlider1.FineThreshold = ((uint)(10u));
			this.alphaSlider1.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("alphaSlider1.ItemsInIndices")));
			this.alphaSlider1.Location = new System.Drawing.Point(166, 76);
			this.alphaSlider1.MediumThreshold = ((uint)(10u));
			this.alphaSlider1.Name = "alphaSlider1";
			this.alphaSlider1.Size = new System.Drawing.Size(75, 23);
			this.alphaSlider1.TabIndex = 0;
			this.alphaSlider1.Text = "alphaSlider1";
			this.alphaSlider1.ThumbShape = null;
			this.alphaSlider1.Value = 50;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.alphaSlider1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private CustomSlider.AlphaSlider alphaSlider1;
	}
}

