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
            this.idAlphaslider1 = new CustomSlider.IDAlphaslider();
            this.idActiveAreaSlider1 = new CustomSlider.IDActiveAreaSlider();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // idAlphaslider1
            // 
            this.idAlphaslider1.IndexCharacters = ((System.Collections.Generic.List<char>)(resources.GetObject("idAlphaslider1.IndexCharacters")));
            this.idAlphaslider1.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("idAlphaslider1.ItemsInIndices")));
            this.idAlphaslider1.Location = new System.Drawing.Point(12, 12);
            this.idAlphaslider1.Name = "idAlphaslider1";
            this.idAlphaslider1.Size = new System.Drawing.Size(728, 142);
            this.idAlphaslider1.TabIndex = 0;
            this.idAlphaslider1.Text = "idAlphaslider1";
            this.idAlphaslider1.Value = 0;
            // 
            // idActiveAreaSlider1
            // 
            this.idActiveAreaSlider1.DrawSlider = true;
            this.idActiveAreaSlider1.IndexCharacters = ((System.Collections.Generic.List<char>)(resources.GetObject("idActiveAreaSlider1.IndexCharacters")));
            this.idActiveAreaSlider1.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("idActiveAreaSlider1.ItemsInIndices")));
            this.idActiveAreaSlider1.Location = new System.Drawing.Point(12, 174);
            this.idActiveAreaSlider1.MaxItemsPerSliderPixel = 2;
            this.idActiveAreaSlider1.Name = "idActiveAreaSlider1";
            this.idActiveAreaSlider1.RollChangeValue = 1;
            this.idActiveAreaSlider1.Size = new System.Drawing.Size(720, 76);
            this.idActiveAreaSlider1.TabIndex = 1;
            this.idActiveAreaSlider1.Text = "idActiveAreaSlider1";
            this.idActiveAreaSlider1.Value = 0;
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 262);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.idActiveAreaSlider1);
            this.Controls.Add(this.idAlphaslider1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomSlider.IDAlphaslider idAlphaslider1;
        private CustomSlider.IDActiveAreaSlider idActiveAreaSlider1;
        private System.Windows.Forms.Label label1;
    }
}

