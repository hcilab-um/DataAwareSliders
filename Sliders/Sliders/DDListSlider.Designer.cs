namespace CustomSlider
{
    partial class DDListSlider
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DDListSlider));
            this.label = new System.Windows.Forms.Label();
            this.listBox = new System.Windows.Forms.ListBox();
            this.DDMultiValueSlider = new CustomSlider.DDMultiValueSlider();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(47, 51);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(35, 13);
            this.label.TabIndex = 2;
            this.label.Text = "label1";
            // 
            // listBox
            // 
            this.listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.listBox.BackColor = System.Drawing.SystemColors.Control;
            this.listBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(38, 3);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(120, 78);
            this.listBox.TabIndex = 3;
            // 
            // DDMultiValueSlider
            // 
            this.DDMultiValueSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DDMultiValueSlider.IndexCharacters = null;
            this.DDMultiValueSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("DDMultiValueSlider.ItemsInIndices")));
            this.DDMultiValueSlider.Location = new System.Drawing.Point(3, 3);
            this.DDMultiValueSlider.Name = "DDMultiValueSlider";
            this.DDMultiValueSlider.Size = new System.Drawing.Size(492, 45);
            this.DDMultiValueSlider.TabIndex = 0;
            this.DDMultiValueSlider.Text = "multiValueSliderV21";
            this.DDMultiValueSlider.Value = 0;
            // 
            // DDListSlider
            // 
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.label);
            this.Controls.Add(this.DDMultiValueSlider);
            this.Name = "DDListSlider";
            this.Size = new System.Drawing.Size(498, 81);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DDMultiValueSlider DDMultiValueSlider;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.ListBox listBox;
    }
}
