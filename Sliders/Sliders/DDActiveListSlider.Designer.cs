namespace CustomSlider
{
    partial class DDActiveListSlider
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DDActiveListSlider));
            this.listBox = new System.Windows.Forms.ListBox();
            this.DDActiveAreaSlider = new CustomSlider.DDActiveAreaSlider();
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
            this.DDActiveAreaSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DDActiveAreaSlider.DrawSlider = true;
            this.DDActiveAreaSlider.IndexCharacters = null;
            this.DDActiveAreaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("activeAreaSlider.ItemsInIndices")));
            this.DDActiveAreaSlider.Location = new System.Drawing.Point(3, 12);
            this.DDActiveAreaSlider.MaxItemsPerSliderPixel = 2;
            this.DDActiveAreaSlider.Name = "activeAreaSlider";
            this.DDActiveAreaSlider.RollChangeValue = 1;
            this.DDActiveAreaSlider.Size = new System.Drawing.Size(442, 51);
            this.DDActiveAreaSlider.TabIndex = 0;
            this.DDActiveAreaSlider.Text = "activeAreaSliderv21";
            this.DDActiveAreaSlider.Value = 0;
            // 
            // DDActiveListSlider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.DDActiveAreaSlider);
            this.Name = "DDActiveListSlider";
            this.Size = new System.Drawing.Size(448, 103);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox;
        private DDActiveAreaSlider DDActiveAreaSlider;
    }
}
