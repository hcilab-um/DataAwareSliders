namespace Test
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
			this.label1 = new System.Windows.Forms.Label();
			this.ddActiveListSlider1 = new CustomSlider.DDActiveListSlider();
			this.idActiveListSlider1 = new CustomSlider.IDActiveListSlider();
			this.ddActiveAreaSlider1 = new CustomSlider.DDActiveAreaSlider();
			this.ddListSlider1 = new CustomSlider.DDListSlider();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(808, 107);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "label1";
			// 
			// ddActiveListSlider1
			// 
			this.ddActiveListSlider1.Data = null;
			this.ddActiveListSlider1.IndexCharacters = null;
			this.ddActiveListSlider1.IndexNames = null;
			this.ddActiveListSlider1.Location = new System.Drawing.Point(23, 12);
			this.ddActiveListSlider1.Name = "ddActiveListSlider1";
			this.ddActiveListSlider1.Size = new System.Drawing.Size(448, 103);
			this.ddActiveListSlider1.TabIndex = 3;
			this.ddActiveListSlider1.Value = 0;
			// 
			// idActiveListSlider1
			// 
			this.idActiveListSlider1.Data = null;
			this.idActiveListSlider1.IndexCharacters = null;
			this.idActiveListSlider1.IndexNames = null;
			this.idActiveListSlider1.Location = new System.Drawing.Point(23, 131);
			this.idActiveListSlider1.Name = "idActiveListSlider1";
			this.idActiveListSlider1.Size = new System.Drawing.Size(458, 103);
			this.idActiveListSlider1.TabIndex = 4;
			this.idActiveListSlider1.Value = 0;
			// 
			// ddActiveAreaSlider1
			// 
			this.ddActiveAreaSlider1.DrawSlider = true;
			this.ddActiveAreaSlider1.IndexCharacters = ((System.Collections.Generic.List<char>)(resources.GetObject("ddActiveAreaSlider1.IndexCharacters")));
			this.ddActiveAreaSlider1.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("ddActiveAreaSlider1.ItemsInIndices")));
			this.ddActiveAreaSlider1.Location = new System.Drawing.Point(12, 252);
			this.ddActiveAreaSlider1.MaxItemsPerSliderPixel = 2;
			this.ddActiveAreaSlider1.Name = "ddActiveAreaSlider1";
			this.ddActiveAreaSlider1.RollChangeValue = 1;
			this.ddActiveAreaSlider1.Size = new System.Drawing.Size(469, 65);
			this.ddActiveAreaSlider1.TabIndex = 5;
			this.ddActiveAreaSlider1.Text = "ddActiveAreaSlider1";
			this.ddActiveAreaSlider1.Value = 0;
			// 
			// ddListSlider1
			// 
			this.ddListSlider1.IndexNames = null;
			this.ddListSlider1.List = ((System.Collections.Generic.List<string>)(resources.GetObject("ddListSlider1.List")));
			this.ddListSlider1.Location = new System.Drawing.Point(12, 325);
			this.ddListSlider1.Name = "ddListSlider1";
			this.ddListSlider1.ShowLabel = false;
			this.ddListSlider1.Size = new System.Drawing.Size(498, 81);
			this.ddListSlider1.TabIndex = 6;
			this.ddListSlider1.Value = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(869, 418);
			this.Controls.Add(this.ddListSlider1);
			this.Controls.Add(this.ddActiveAreaSlider1);
			this.Controls.Add(this.idActiveListSlider1);
			this.Controls.Add(this.ddActiveListSlider1);
			this.Controls.Add(this.label1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Label label1;
		private CustomSlider.DDActiveListSlider ddActiveListSlider1;
		private CustomSlider.IDActiveListSlider idActiveListSlider1;
		private CustomSlider.DDActiveAreaSlider ddActiveAreaSlider1;
		private CustomSlider.DDListSlider ddListSlider1;
    }
}

