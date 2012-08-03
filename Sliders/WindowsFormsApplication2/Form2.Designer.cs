namespace CustomSlider.WindowsFormsApplication2
{
	partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.resetButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ddListSlider1 = new CustomSlider.DDListSlider();
            this.ddActiveAreaSlider1 = new CustomSlider.DDActiveAreaSlider();
            this.idAlphaslider1 = new CustomSlider.IDAlphaslider();
            this.activeMultiSlider1 = new CustomSlider.IDActiveListSlider();
            this.activeAreaSliderv21 = new CustomSlider.IDActiveAreaSlider();
            this.alphasliderV31 = new CustomSlider.DDAlphaslider();
            this.multiValueSliderV31 = new CustomSlider.IDListSlider();
            this.ddActiveListSlider1 = new CustomSlider.DDActiveListSlider();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(622, 453);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(622, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "label4";
            // 
            // listBox2
            // 
            this.listBox2.ColumnWidth = 40;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(12, 78);
            this.listBox2.MultiColumn = true;
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(598, 95);
            this.listBox2.TabIndex = 12;
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(651, 616);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.TabIndex = 23;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(625, 214);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(625, 538);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "label3";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1076, 106);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "label5";
            // 
            // ddListSlider1
            // 
            this.ddListSlider1.IndexNames = null;
            this.ddListSlider1.List = ((System.Collections.Generic.List<string>)(resources.GetObject("ddListSlider1.List")));
            this.ddListSlider1.Location = new System.Drawing.Point(686, 189);
            this.ddListSlider1.Name = "ddListSlider1";
            this.ddListSlider1.ShowLabel = false;
            this.ddListSlider1.Size = new System.Drawing.Size(467, 81);
            this.ddListSlider1.TabIndex = 29;
            this.ddListSlider1.Value = 0;
            // 
            // ddActiveAreaSlider1
            // 
            this.ddActiveAreaSlider1.DrawSlider = true;
            this.ddActiveAreaSlider1.IndexCharacters = null;
            this.ddActiveAreaSlider1.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("ddActiveAreaSlider1.ItemsInIndices")));
            this.ddActiveAreaSlider1.Location = new System.Drawing.Point(716, 78);
            this.ddActiveAreaSlider1.MaxItemsPerSliderPixel = 2;
            this.ddActiveAreaSlider1.Name = "ddActiveAreaSlider1";
            this.ddActiveAreaSlider1.RollChangeValue = 1;
            this.ddActiveAreaSlider1.Size = new System.Drawing.Size(335, 61);
            this.ddActiveAreaSlider1.TabIndex = 27;
            this.ddActiveAreaSlider1.Text = "ddActiveAreaSlider1";
            this.ddActiveAreaSlider1.Value = 0;
            // 
            // idAlphaslider1
            // 
            this.idAlphaslider1.IndexCharacters = null;
            this.idAlphaslider1.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("idAlphaslider1.ItemsInIndices")));
            this.idAlphaslider1.Location = new System.Drawing.Point(12, 516);
            this.idAlphaslider1.Name = "idAlphaslider1";
            this.idAlphaslider1.Size = new System.Drawing.Size(598, 52);
            this.idAlphaslider1.TabIndex = 25;
            this.idAlphaslider1.Text = "idAlphaslider1";
            this.idAlphaslider1.Value = 0;
            // 
            // activeMultiSlider1
            // 
            this.activeMultiSlider1.Data = null;
            this.activeMultiSlider1.IndexCharacters = null;
            this.activeMultiSlider1.IndexNames = null;
            this.activeMultiSlider1.Location = new System.Drawing.Point(12, 306);
            this.activeMultiSlider1.Name = "activeMultiSlider1";
            this.activeMultiSlider1.Size = new System.Drawing.Size(598, 97);
            this.activeMultiSlider1.TabIndex = 22;
            this.activeMultiSlider1.Value = 0;
            // 
            // activeAreaSliderv21
            // 
            this.activeAreaSliderv21.DrawSlider = true;
            this.activeAreaSliderv21.IndexCharacters = null;
            this.activeAreaSliderv21.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("activeAreaSliderv21.ItemsInIndices")));
            this.activeAreaSliderv21.Location = new System.Drawing.Point(12, 12);
            this.activeAreaSliderv21.MaxItemsPerSliderPixel = 2;
            this.activeAreaSliderv21.Name = "activeAreaSliderv21";
            this.activeAreaSliderv21.RollChangeValue = 1;
            this.activeAreaSliderv21.Size = new System.Drawing.Size(598, 60);
            this.activeAreaSliderv21.TabIndex = 21;
            this.activeAreaSliderv21.Text = "activeAreaSliderv21";
            this.activeAreaSliderv21.Value = 0;
            // 
            // alphasliderV31
            // 
            this.alphasliderV31.IndexCharacters = null;
            this.alphasliderV31.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("alphasliderV31.ItemsInIndices")));
            this.alphasliderV31.Location = new System.Drawing.Point(12, 443);
            this.alphasliderV31.Name = "alphasliderV31";
            this.alphasliderV31.Size = new System.Drawing.Size(598, 42);
            this.alphasliderV31.TabIndex = 20;
            this.alphasliderV31.Text = "alphasliderV31";
            this.alphasliderV31.Value = 1;
            // 
            // multiValueSliderV31
            // 
            this.multiValueSliderV31.BackColor = System.Drawing.Color.Transparent;
            this.multiValueSliderV31.IndexNames = null;
            this.multiValueSliderV31.List = ((System.Collections.Generic.List<string>)(resources.GetObject("multiValueSliderV31.List")));
            this.multiValueSliderV31.Location = new System.Drawing.Point(12, 189);
            this.multiValueSliderV31.Name = "multiValueSliderV31";
            this.multiValueSliderV31.ShowLabel = false;
            this.multiValueSliderV31.Size = new System.Drawing.Size(598, 111);
            this.multiValueSliderV31.TabIndex = 19;
            this.multiValueSliderV31.Value = 0;
            // 
            // ddActiveListSlider1
            // 
            this.ddActiveListSlider1.Data = null;
            this.ddActiveListSlider1.IndexCharacters = null;
            this.ddActiveListSlider1.IndexNames = null;
            this.ddActiveListSlider1.Location = new System.Drawing.Point(684, 306);
            this.ddActiveListSlider1.Name = "ddActiveListSlider1";
            this.ddActiveListSlider1.Size = new System.Drawing.Size(458, 103);
            this.ddActiveListSlider1.TabIndex = 30;
            this.ddActiveListSlider1.Value = 0;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1154, 639);
            this.Controls.Add(this.ddActiveListSlider1);
            this.Controls.Add(this.ddListSlider1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ddActiveAreaSlider1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.idAlphaslider1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.activeMultiSlider1);
            this.Controls.Add(this.activeAreaSliderv21);
            this.Controls.Add(this.alphasliderV31);
            this.Controls.Add(this.multiValueSliderV31);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBox2;
		private CustomSlider.IDListSlider multiValueSliderV31;
        private CustomSlider.DDAlphaslider alphasliderV31;
		private CustomSlider.IDActiveAreaSlider activeAreaSliderv21;
		private CustomSlider.IDActiveListSlider activeMultiSlider1;
		private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Label label2;
        private CustomSlider.IDAlphaslider idAlphaslider1;
        private System.Windows.Forms.Label label3;
        private DDActiveAreaSlider ddActiveAreaSlider1;
        private System.Windows.Forms.Label label5;
        private DDListSlider ddListSlider1;
        private DDActiveListSlider ddActiveListSlider1;

	}
}