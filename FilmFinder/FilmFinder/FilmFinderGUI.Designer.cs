namespace FilmFinder
{
	partial class FilmFinderGUI
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
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilmFinderGUI));
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.disableAllGenreButton = new System.Windows.Forms.Button();
			this.enableAllGenreButton = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.currentActorLabel = new System.Windows.Forms.Label();
			this.currentDirectorLabel = new System.Windows.Forms.Label();
			this.certifactionsPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.disableAllCertificationsButton = new System.Windows.Forms.Button();
			this.enableAllCertificationsButton = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.runningTimeRangeLabel = new System.Windows.Forms.Label();
			this.yearRangeLabel = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.ratingRangeLabel = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.nextSliderButton = new System.Windows.Forms.Button();
			this.currentActressLabel = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.previousSliderButton = new System.Windows.Forms.Button();
			this.startExperimentButton = new System.Windows.Forms.Button();
			this.pleaseFindLabel = new System.Windows.Forms.Label();
			this.searchLabel = new System.Windows.Forms.Label();
			this.startSearchButton = new System.Windows.Forms.Button();
			this.confirmSearchButton = new System.Windows.Forms.Button();
			this.searchConfirmLabel = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.actressActiveMultiSlider = new CustomSlider.ActiveMultiSlider();
			this.directorActiveMultiSlider = new CustomSlider.ActiveMultiSlider();
			this.actorActiveMultiSlider = new CustomSlider.ActiveMultiSlider();
			this.directorActiveAreaSlider = new CustomSlider.ActiveAreaSliderv2();
			this.actorAlphaSlider = new CustomSlider.AlphasliderV3();
			this.actressAlphaSlider = new CustomSlider.AlphasliderV3();
			this.directorAlphaSlider = new CustomSlider.AlphasliderV3();
			this.actorActiveAreaSlider = new CustomSlider.ActiveAreaSliderv2();
			this.actressActiveAreaSlider = new CustomSlider.ActiveAreaSliderv2();
			this.actorMVSv3 = new CustomSlider.MultiValueSliderV3();
			this.actressMVSv3 = new CustomSlider.MultiValueSliderV3();
			this.directorMVSc3 = new CustomSlider.MultiValueSliderV3();
			this.ratingRangeSlider = new FilmFinder.RangeSlider();
			this.yearRangeSlider = new FilmFinder.RangeSlider();
			this.runningTimeRangeSlider = new FilmFinder.RangeSlider();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			this.certifactionsPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// chart1
			// 
			chartArea1.Name = "ChartArea1";
			this.chart1.ChartAreas.Add(chartArea1);
			legend1.Name = "Legend1";
			this.chart1.Legends.Add(legend1);
			resources.ApplyResources(this.chart1, "chart1");
			this.chart1.Name = "chart1";
			series1.ChartArea = "ChartArea1";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
			series1.Legend = "Legend1";
			series1.MarkerColor = System.Drawing.Color.Red;
			series1.MarkerSize = 3;
			series1.Name = "Series1";
			this.chart1.Series.Add(series1);
			this.chart1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chart1_Click);
			// 
			// disableAllGenreButton
			// 
			resources.ApplyResources(this.disableAllGenreButton, "disableAllGenreButton");
			this.disableAllGenreButton.Name = "disableAllGenreButton";
			this.disableAllGenreButton.UseVisualStyleBackColor = true;
			this.disableAllGenreButton.Click += new System.EventHandler(this.disableAllGenreButton_Click);
			// 
			// enableAllGenreButton
			// 
			resources.ApplyResources(this.enableAllGenreButton, "enableAllGenreButton");
			this.enableAllGenreButton.Name = "enableAllGenreButton";
			this.enableAllGenreButton.UseVisualStyleBackColor = true;
			this.enableAllGenreButton.Click += new System.EventHandler(this.enableAllGenreButton_Click);
			// 
			// flowLayoutPanel1
			// 
			resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
			this.flowLayoutPanel1.Controls.Add(this.disableAllGenreButton);
			this.flowLayoutPanel1.Controls.Add(this.enableAllGenreButton);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// currentActorLabel
			// 
			resources.ApplyResources(this.currentActorLabel, "currentActorLabel");
			this.currentActorLabel.Name = "currentActorLabel";
			// 
			// currentDirectorLabel
			// 
			resources.ApplyResources(this.currentDirectorLabel, "currentDirectorLabel");
			this.currentDirectorLabel.Name = "currentDirectorLabel";
			// 
			// certifactionsPanel
			// 
			resources.ApplyResources(this.certifactionsPanel, "certifactionsPanel");
			this.certifactionsPanel.Controls.Add(this.disableAllCertificationsButton);
			this.certifactionsPanel.Controls.Add(this.enableAllCertificationsButton);
			this.certifactionsPanel.Name = "certifactionsPanel";
			// 
			// disableAllCertificationsButton
			// 
			resources.ApplyResources(this.disableAllCertificationsButton, "disableAllCertificationsButton");
			this.disableAllCertificationsButton.Name = "disableAllCertificationsButton";
			this.disableAllCertificationsButton.UseVisualStyleBackColor = true;
			this.disableAllCertificationsButton.Click += new System.EventHandler(this.disableAllCertificationsButton_Click);
			// 
			// enableAllCertificationsButton
			// 
			resources.ApplyResources(this.enableAllCertificationsButton, "enableAllCertificationsButton");
			this.enableAllCertificationsButton.Name = "enableAllCertificationsButton";
			this.enableAllCertificationsButton.UseVisualStyleBackColor = true;
			this.enableAllCertificationsButton.Click += new System.EventHandler(this.enableAllCertificationsButton_Click);
			// 
			// button1
			// 
			resources.ApplyResources(this.button1, "button1");
			this.button1.Name = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// runningTimeRangeLabel
			// 
			resources.ApplyResources(this.runningTimeRangeLabel, "runningTimeRangeLabel");
			this.runningTimeRangeLabel.Name = "runningTimeRangeLabel";
			// 
			// yearRangeLabel
			// 
			resources.ApplyResources(this.yearRangeLabel, "yearRangeLabel");
			this.yearRangeLabel.Name = "yearRangeLabel";
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			// 
			// ratingRangeLabel
			// 
			resources.ApplyResources(this.ratingRangeLabel, "ratingRangeLabel");
			this.ratingRangeLabel.Name = "ratingRangeLabel";
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// nextSliderButton
			// 
			resources.ApplyResources(this.nextSliderButton, "nextSliderButton");
			this.nextSliderButton.Name = "nextSliderButton";
			this.nextSliderButton.UseVisualStyleBackColor = true;
			this.nextSliderButton.Click += new System.EventHandler(this.nextSliderButton_Click);
			// 
			// currentActressLabel
			// 
			resources.ApplyResources(this.currentActressLabel, "currentActressLabel");
			this.currentActressLabel.Name = "currentActressLabel";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// previousSliderButton
			// 
			resources.ApplyResources(this.previousSliderButton, "previousSliderButton");
			this.previousSliderButton.Name = "previousSliderButton";
			this.previousSliderButton.UseVisualStyleBackColor = true;
			this.previousSliderButton.Click += new System.EventHandler(this.previousSliderButton_Click);
			// 
			// startExperimentButton
			// 
			resources.ApplyResources(this.startExperimentButton, "startExperimentButton");
			this.startExperimentButton.Name = "startExperimentButton";
			this.startExperimentButton.UseVisualStyleBackColor = true;
			this.startExperimentButton.Click += new System.EventHandler(this.startExperimentButton_Click);
			// 
			// pleaseFindLabel
			// 
			resources.ApplyResources(this.pleaseFindLabel, "pleaseFindLabel");
			this.pleaseFindLabel.Name = "pleaseFindLabel";
			// 
			// searchLabel
			// 
			resources.ApplyResources(this.searchLabel, "searchLabel");
			this.searchLabel.Name = "searchLabel";
			// 
			// startSearchButton
			// 
			resources.ApplyResources(this.startSearchButton, "startSearchButton");
			this.startSearchButton.Name = "startSearchButton";
			this.startSearchButton.UseVisualStyleBackColor = true;
			this.startSearchButton.Click += new System.EventHandler(this.startSearchButton_Click);
			// 
			// confirmSearchButton
			// 
			resources.ApplyResources(this.confirmSearchButton, "confirmSearchButton");
			this.confirmSearchButton.Name = "confirmSearchButton";
			this.confirmSearchButton.UseVisualStyleBackColor = true;
			this.confirmSearchButton.Click += new System.EventHandler(this.confirmSearchButton_Click);
			// 
			// searchConfirmLabel
			// 
			resources.ApplyResources(this.searchConfirmLabel, "searchConfirmLabel");
			this.searchConfirmLabel.BackColor = System.Drawing.Color.Red;
			this.searchConfirmLabel.Name = "searchConfirmLabel";
			// 
			// label9
			// 
			resources.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			// 
			// actressActiveMultiSlider
			// 
			this.actressActiveMultiSlider.Data = null;
			this.actressActiveMultiSlider.IndexNames = null;
			resources.ApplyResources(this.actressActiveMultiSlider, "actressActiveMultiSlider");
			this.actressActiveMultiSlider.Name = "actressActiveMultiSlider";
			this.actressActiveMultiSlider.Value = 0;
			// 
			// directorActiveMultiSlider
			// 
			this.directorActiveMultiSlider.Data = null;
			this.directorActiveMultiSlider.IndexNames = null;
			resources.ApplyResources(this.directorActiveMultiSlider, "directorActiveMultiSlider");
			this.directorActiveMultiSlider.Name = "directorActiveMultiSlider";
			this.directorActiveMultiSlider.Value = 0;
			// 
			// actorActiveMultiSlider
			// 
			this.actorActiveMultiSlider.Data = null;
			this.actorActiveMultiSlider.IndexNames = null;
			resources.ApplyResources(this.actorActiveMultiSlider, "actorActiveMultiSlider");
			this.actorActiveMultiSlider.Name = "actorActiveMultiSlider";
			this.actorActiveMultiSlider.Value = 0;
			// 
			// directorActiveAreaSlider
			// 
			resources.ApplyResources(this.directorActiveAreaSlider, "directorActiveAreaSlider");
			this.directorActiveAreaSlider.DrawSlider = true;
			this.directorActiveAreaSlider.IndexNames = null;
			this.directorActiveAreaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("directorActiveAreaSlider.ItemsInIndices")));
			this.directorActiveAreaSlider.MaxItemsPerSliderPixel = 2;
			this.directorActiveAreaSlider.Name = "directorActiveAreaSlider";
			this.directorActiveAreaSlider.RollChangeValue = 1;
			this.directorActiveAreaSlider.Value = 50;
			// 
			// actorAlphaSlider
			// 
			resources.ApplyResources(this.actorAlphaSlider, "actorAlphaSlider");
			this.actorAlphaSlider.Data = null;
			this.actorAlphaSlider.IndexNames = null;
			this.actorAlphaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("actorAlphaSlider.ItemsInIndices")));
			this.actorAlphaSlider.Name = "actorAlphaSlider";
			this.actorAlphaSlider.Value = 50;
			// 
			// actressAlphaSlider
			// 
			resources.ApplyResources(this.actressAlphaSlider, "actressAlphaSlider");
			this.actressAlphaSlider.Data = null;
			this.actressAlphaSlider.IndexNames = null;
			this.actressAlphaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("actressAlphaSlider.ItemsInIndices")));
			this.actressAlphaSlider.Name = "actressAlphaSlider";
			this.actressAlphaSlider.Value = 50;
			// 
			// directorAlphaSlider
			// 
			resources.ApplyResources(this.directorAlphaSlider, "directorAlphaSlider");
			this.directorAlphaSlider.Data = null;
			this.directorAlphaSlider.IndexNames = null;
			this.directorAlphaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("directorAlphaSlider.ItemsInIndices")));
			this.directorAlphaSlider.Name = "directorAlphaSlider";
			this.directorAlphaSlider.Value = 50;
			// 
			// actorActiveAreaSlider
			// 
			resources.ApplyResources(this.actorActiveAreaSlider, "actorActiveAreaSlider");
			this.actorActiveAreaSlider.DrawSlider = true;
			this.actorActiveAreaSlider.IndexNames = null;
			this.actorActiveAreaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("actorActiveAreaSlider.ItemsInIndices")));
			this.actorActiveAreaSlider.MaxItemsPerSliderPixel = 2;
			this.actorActiveAreaSlider.Name = "actorActiveAreaSlider";
			this.actorActiveAreaSlider.RollChangeValue = 1;
			this.actorActiveAreaSlider.Value = 50;
			// 
			// actressActiveAreaSlider
			// 
			resources.ApplyResources(this.actressActiveAreaSlider, "actressActiveAreaSlider");
			this.actressActiveAreaSlider.DrawSlider = true;
			this.actressActiveAreaSlider.IndexNames = null;
			this.actressActiveAreaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("actressActiveAreaSlider.ItemsInIndices")));
			this.actressActiveAreaSlider.MaxItemsPerSliderPixel = 2;
			this.actressActiveAreaSlider.Name = "actressActiveAreaSlider";
			this.actressActiveAreaSlider.RollChangeValue = 1;
			this.actressActiveAreaSlider.Value = 50;
			// 
			// actorMVSv3
			// 
			resources.ApplyResources(this.actorMVSv3, "actorMVSv3");
			this.actorMVSv3.BackColor = System.Drawing.Color.Transparent;
			this.actorMVSv3.IndexNames = null;
			this.actorMVSv3.List = ((System.Collections.Generic.List<string>)(resources.GetObject("actorMVSv3.List")));
			this.actorMVSv3.Name = "actorMVSv3";
			this.actorMVSv3.ShowLabel = false;
			this.actorMVSv3.Value = 0;
			// 
			// actressMVSv3
			// 
			resources.ApplyResources(this.actressMVSv3, "actressMVSv3");
			this.actressMVSv3.BackColor = System.Drawing.Color.Transparent;
			this.actressMVSv3.IndexNames = null;
			this.actressMVSv3.List = ((System.Collections.Generic.List<string>)(resources.GetObject("actressMVSv3.List")));
			this.actressMVSv3.Name = "actressMVSv3";
			this.actressMVSv3.ShowLabel = false;
			this.actressMVSv3.Value = 0;
			// 
			// directorMVSc3
			// 
			resources.ApplyResources(this.directorMVSc3, "directorMVSc3");
			this.directorMVSc3.BackColor = System.Drawing.Color.Transparent;
			this.directorMVSc3.IndexNames = null;
			this.directorMVSc3.List = ((System.Collections.Generic.List<string>)(resources.GetObject("directorMVSc3.List")));
			this.directorMVSc3.Name = "directorMVSc3";
			this.directorMVSc3.ShowLabel = false;
			this.directorMVSc3.Value = 0;
			// 
			// ratingRangeSlider
			// 
			resources.ApplyResources(this.ratingRangeSlider, "ratingRangeSlider");
			this.ratingRangeSlider.LowerBound = 0;
			this.ratingRangeSlider.LowerRange = 0;
			this.ratingRangeSlider.Name = "ratingRangeSlider";
			this.ratingRangeSlider.UpperBound = 100;
			this.ratingRangeSlider.UpperRange = 100;
			// 
			// yearRangeSlider
			// 
			resources.ApplyResources(this.yearRangeSlider, "yearRangeSlider");
			this.yearRangeSlider.LowerBound = 0;
			this.yearRangeSlider.LowerRange = 0;
			this.yearRangeSlider.Name = "yearRangeSlider";
			this.yearRangeSlider.UpperBound = 100;
			this.yearRangeSlider.UpperRange = 100;
			// 
			// runningTimeRangeSlider
			// 
			resources.ApplyResources(this.runningTimeRangeSlider, "runningTimeRangeSlider");
			this.runningTimeRangeSlider.LowerBound = 0;
			this.runningTimeRangeSlider.LowerRange = 0;
			this.runningTimeRangeSlider.Name = "runningTimeRangeSlider";
			this.runningTimeRangeSlider.UpperBound = 100;
			this.runningTimeRangeSlider.UpperRange = 100;
			// 
			// FilmFinderGUI
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.actressActiveMultiSlider);
			this.Controls.Add(this.directorActiveMultiSlider);
			this.Controls.Add(this.actorActiveMultiSlider);
			this.Controls.Add(this.searchConfirmLabel);
			this.Controls.Add(this.confirmSearchButton);
			this.Controls.Add(this.startSearchButton);
			this.Controls.Add(this.searchLabel);
			this.Controls.Add(this.pleaseFindLabel);
			this.Controls.Add(this.currentDirectorLabel);
			this.Controls.Add(this.currentActorLabel);
			this.Controls.Add(this.currentActressLabel);
			this.Controls.Add(this.startExperimentButton);
			this.Controls.Add(this.previousSliderButton);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.directorActiveAreaSlider);
			this.Controls.Add(this.actressActiveAreaSlider);
			this.Controls.Add(this.actorActiveAreaSlider);
			this.Controls.Add(this.nextSliderButton);
			this.Controls.Add(this.directorAlphaSlider);
			this.Controls.Add(this.actressAlphaSlider);
			this.Controls.Add(this.actorAlphaSlider);
			this.Controls.Add(this.ratingRangeLabel);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.ratingRangeSlider);
			this.Controls.Add(this.yearRangeLabel);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.yearRangeSlider);
			this.Controls.Add(this.runningTimeRangeLabel);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.runningTimeRangeSlider);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.certifactionsPanel);
			this.Controls.Add(this.actressMVSv3);
			this.Controls.Add(this.directorMVSc3);
			this.Controls.Add(this.actorMVSv3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.chart1);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.Name = "FilmFinderGUI";
			this.Load += new System.EventHandler(this.FilmFinderGUI_Load);
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.certifactionsPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		

		#endregion

		private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
		private System.Windows.Forms.Button disableAllGenreButton;
		private System.Windows.Forms.Button enableAllGenreButton;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label currentActorLabel;
		private System.Windows.Forms.Label currentDirectorLabel;
		private System.Windows.Forms.FlowLayoutPanel certifactionsPanel;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button disableAllCertificationsButton;
		private System.Windows.Forms.Button enableAllCertificationsButton;
		private RangeSlider runningTimeRangeSlider;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label runningTimeRangeLabel;
		private System.Windows.Forms.Label yearRangeLabel;
		private System.Windows.Forms.Label label8;
		private RangeSlider yearRangeSlider;
		private System.Windows.Forms.Label ratingRangeLabel;
		private System.Windows.Forms.Label label7;
		private RangeSlider ratingRangeSlider;
		private CustomSlider.AlphasliderV3 actorAlphaSlider;
		private CustomSlider.AlphasliderV3 actressAlphaSlider;
		private CustomSlider.AlphasliderV3 directorAlphaSlider;
		private System.Windows.Forms.Button nextSliderButton;
		private CustomSlider.ActiveAreaSliderv2 actorActiveAreaSlider;
		private CustomSlider.ActiveAreaSliderv2 actressActiveAreaSlider;
		private CustomSlider.ActiveAreaSliderv2 directorActiveAreaSlider;
		private System.Windows.Forms.Label currentActressLabel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label6;
		private CustomSlider.MultiValueSliderV3 actorMVSv3;
		private CustomSlider.MultiValueSliderV3 actressMVSv3;
		private CustomSlider.MultiValueSliderV3 directorMVSc3;
		private System.Windows.Forms.Button previousSliderButton;
		private System.Windows.Forms.Button startExperimentButton;
		private System.Windows.Forms.Label pleaseFindLabel;
		private System.Windows.Forms.Label searchLabel;
		private System.Windows.Forms.Button startSearchButton;
		private System.Windows.Forms.Button confirmSearchButton;
		private System.Windows.Forms.Label searchConfirmLabel;
		private System.Windows.Forms.Label label9;
		private CustomSlider.ActiveMultiSlider actorActiveMultiSlider;
		private CustomSlider.ActiveMultiSlider directorActiveMultiSlider;
		private CustomSlider.ActiveMultiSlider actressActiveMultiSlider;


	}
}

