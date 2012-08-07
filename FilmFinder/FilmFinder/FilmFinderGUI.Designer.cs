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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilmFinderGUI));
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            this.newDataButton = new System.Windows.Forms.Button();
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
            this.actressDDActiveAreaSlider = new CustomSlider.DDActiveAreaSlider();
            this.actorDDActiveAreaSlider = new CustomSlider.DDActiveAreaSlider();
            this.actressIDActiveListSlider = new CustomSlider.IDActiveListSlider();
            this.directorIDActiveListSlider = new CustomSlider.IDActiveListSlider();
            this.actorIDActiveListSlider = new CustomSlider.IDActiveListSlider();
            this.directorIDActiveAreaSlider = new CustomSlider.IDActiveAreaSlider();
            this.actorDDAlphaSlider = new CustomSlider.DDAlphaslider();
            this.actressDDAlphaSlider = new CustomSlider.DDAlphaslider();
            this.directorDDAlphaSlider = new CustomSlider.DDAlphaslider();
            this.actorIDActiveAreaSlider = new CustomSlider.IDActiveAreaSlider();
            this.actressIDActiveAreaSlider = new CustomSlider.IDActiveAreaSlider();
            this.actorIDListSlider = new CustomSlider.IDListSlider();
            this.actressIDListSlider = new CustomSlider.IDListSlider();
            this.directorIDListSlider = new CustomSlider.IDListSlider();
            this.directorDDActiveAreaSlider = new CustomSlider.DDActiveAreaSlider();
            this.actorDDListSlider = new CustomSlider.DDListSlider();
            this.actressDDListSlider = new CustomSlider.DDListSlider();
            this.directorDDListSlider = new CustomSlider.DDListSlider();
            this.ratingRangeSlider = new FilmFinder.RangeSlider();
            this.yearRangeSlider = new FilmFinder.RangeSlider();
            this.runningTimeRangeSlider = new FilmFinder.RangeSlider();
            this.actorDDActiveListSlider = new CustomSlider.DDActiveListSlider();
            this.actressDDActiveListSlider = new CustomSlider.DDActiveListSlider();
            this.directorDDActiveListSlider = new CustomSlider.DDActiveListSlider();
            this.actorIDAlphaSlider = new CustomSlider.IDAlphaslider();
            this.directorIDAlphaSlider = new CustomSlider.IDAlphaslider();
            this.actressIDAlphaSlider = new CustomSlider.IDAlphaslider();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.certifactionsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            resources.ApplyResources(this.chart1, "chart1");
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series2.Legend = "Legend1";
            series2.MarkerColor = System.Drawing.Color.Red;
            series2.MarkerSize = 3;
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
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
            // newDataButton
            // 
            resources.ApplyResources(this.newDataButton, "newDataButton");
            this.newDataButton.Name = "newDataButton";
            this.newDataButton.UseVisualStyleBackColor = true;
            this.newDataButton.Click += new System.EventHandler(this.newDataButton_Click);
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
            // actressDDActiveAreaSlider
            // 
            this.actressDDActiveAreaSlider.DrawSlider = true;
            this.actressDDActiveAreaSlider.IndexCharacters = ((System.Collections.Generic.List<char>)(resources.GetObject("actressDDActiveAreaSlider.IndexCharacters")));
            this.actressDDActiveAreaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("actressDDActiveAreaSlider.ItemsInIndices")));
            resources.ApplyResources(this.actressDDActiveAreaSlider, "actressDDActiveAreaSlider");
            this.actressDDActiveAreaSlider.MaxItemsPerSliderPixel = 2;
            this.actressDDActiveAreaSlider.Name = "actressDDActiveAreaSlider";
            this.actressDDActiveAreaSlider.RollChangeValue = 1;
            this.actressDDActiveAreaSlider.Value = 0;
            // 
            // actorDDActiveAreaSlider
            // 
            this.actorDDActiveAreaSlider.DrawSlider = true;
            this.actorDDActiveAreaSlider.IndexCharacters = ((System.Collections.Generic.List<char>)(resources.GetObject("actorDDActiveAreaSlider.IndexCharacters")));
            this.actorDDActiveAreaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("actorDDActiveAreaSlider.ItemsInIndices")));
            resources.ApplyResources(this.actorDDActiveAreaSlider, "actorDDActiveAreaSlider");
            this.actorDDActiveAreaSlider.MaxItemsPerSliderPixel = 2;
            this.actorDDActiveAreaSlider.Name = "actorDDActiveAreaSlider";
            this.actorDDActiveAreaSlider.RollChangeValue = 1;
            this.actorDDActiveAreaSlider.Value = 0;
            // 
            // actressIDActiveListSlider
            // 
            this.actressIDActiveListSlider.Data = null;
            this.actressIDActiveListSlider.IndexCharacters = null;
            this.actressIDActiveListSlider.IndexNames = null;
            resources.ApplyResources(this.actressIDActiveListSlider, "actressIDActiveListSlider");
            this.actressIDActiveListSlider.Name = "actressIDActiveListSlider";
            this.actressIDActiveListSlider.Value = 0;
            // 
            // directorIDActiveListSlider
            // 
            this.directorIDActiveListSlider.Data = null;
            this.directorIDActiveListSlider.IndexCharacters = null;
            this.directorIDActiveListSlider.IndexNames = null;
            resources.ApplyResources(this.directorIDActiveListSlider, "directorIDActiveListSlider");
            this.directorIDActiveListSlider.Name = "directorIDActiveListSlider";
            this.directorIDActiveListSlider.Value = 0;
            // 
            // actorIDActiveListSlider
            // 
            this.actorIDActiveListSlider.Data = null;
            this.actorIDActiveListSlider.IndexCharacters = null;
            this.actorIDActiveListSlider.IndexNames = null;
            resources.ApplyResources(this.actorIDActiveListSlider, "actorIDActiveListSlider");
            this.actorIDActiveListSlider.Name = "actorIDActiveListSlider";
            this.actorIDActiveListSlider.Value = 0;
            // 
            // directorIDActiveAreaSlider
            // 
            resources.ApplyResources(this.directorIDActiveAreaSlider, "directorIDActiveAreaSlider");
            this.directorIDActiveAreaSlider.DrawSlider = true;
            this.directorIDActiveAreaSlider.IndexCharacters = ((System.Collections.Generic.List<char>)(resources.GetObject("directorIDActiveAreaSlider.IndexCharacters")));
            this.directorIDActiveAreaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("directorIDActiveAreaSlider.ItemsInIndices")));
            this.directorIDActiveAreaSlider.MaxItemsPerSliderPixel = 2;
            this.directorIDActiveAreaSlider.Name = "directorIDActiveAreaSlider";
            this.directorIDActiveAreaSlider.RollChangeValue = 1;
            this.directorIDActiveAreaSlider.Value = 50;
            // 
            // actorDDAlphaSlider
            // 
            resources.ApplyResources(this.actorDDAlphaSlider, "actorDDAlphaSlider");
            this.actorDDAlphaSlider.IndexCharacters = ((System.Collections.Generic.List<char>)(resources.GetObject("actorDDAlphaSlider.IndexCharacters")));
            this.actorDDAlphaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("actorDDAlphaSlider.ItemsInIndices")));
            this.actorDDAlphaSlider.Name = "actorDDAlphaSlider";
            this.actorDDAlphaSlider.Value = 50;
            // 
            // actressDDAlphaSlider
            // 
            resources.ApplyResources(this.actressDDAlphaSlider, "actressDDAlphaSlider");
            this.actressDDAlphaSlider.IndexCharacters = ((System.Collections.Generic.List<char>)(resources.GetObject("actressDDAlphaSlider.IndexCharacters")));
            this.actressDDAlphaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("actressDDAlphaSlider.ItemsInIndices")));
            this.actressDDAlphaSlider.Name = "actressDDAlphaSlider";
            this.actressDDAlphaSlider.Value = 50;
            // 
            // directorDDAlphaSlider
            // 
            resources.ApplyResources(this.directorDDAlphaSlider, "directorDDAlphaSlider");
            this.directorDDAlphaSlider.IndexCharacters = ((System.Collections.Generic.List<char>)(resources.GetObject("directorDDAlphaSlider.IndexCharacters")));
            this.directorDDAlphaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("directorDDAlphaSlider.ItemsInIndices")));
            this.directorDDAlphaSlider.Name = "directorDDAlphaSlider";
            this.directorDDAlphaSlider.Value = 50;
            // 
            // actorIDActiveAreaSlider
            // 
            resources.ApplyResources(this.actorIDActiveAreaSlider, "actorIDActiveAreaSlider");
            this.actorIDActiveAreaSlider.DrawSlider = true;
            this.actorIDActiveAreaSlider.IndexCharacters = ((System.Collections.Generic.List<char>)(resources.GetObject("actorIDActiveAreaSlider.IndexCharacters")));
            this.actorIDActiveAreaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("actorIDActiveAreaSlider.ItemsInIndices")));
            this.actorIDActiveAreaSlider.MaxItemsPerSliderPixel = 2;
            this.actorIDActiveAreaSlider.Name = "actorIDActiveAreaSlider";
            this.actorIDActiveAreaSlider.RollChangeValue = 1;
            this.actorIDActiveAreaSlider.Value = 50;
            // 
            // actressIDActiveAreaSlider
            // 
            resources.ApplyResources(this.actressIDActiveAreaSlider, "actressIDActiveAreaSlider");
            this.actressIDActiveAreaSlider.DrawSlider = true;
            this.actressIDActiveAreaSlider.IndexCharacters = ((System.Collections.Generic.List<char>)(resources.GetObject("actressIDActiveAreaSlider.IndexCharacters")));
            this.actressIDActiveAreaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("actressIDActiveAreaSlider.ItemsInIndices")));
            this.actressIDActiveAreaSlider.MaxItemsPerSliderPixel = 2;
            this.actressIDActiveAreaSlider.Name = "actressIDActiveAreaSlider";
            this.actressIDActiveAreaSlider.RollChangeValue = 1;
            this.actressIDActiveAreaSlider.Value = 50;
            // 
            // actorIDListSlider
            // 
            resources.ApplyResources(this.actorIDListSlider, "actorIDListSlider");
            this.actorIDListSlider.BackColor = System.Drawing.Color.Transparent;
            this.actorIDListSlider.IndexNames = null;
            this.actorIDListSlider.List = ((System.Collections.Generic.List<string>)(resources.GetObject("actorIDListSlider.List")));
            this.actorIDListSlider.Name = "actorIDListSlider";
            this.actorIDListSlider.ShowLabel = false;
            this.actorIDListSlider.Value = 0;
            // 
            // actressIDListSlider
            // 
            resources.ApplyResources(this.actressIDListSlider, "actressIDListSlider");
            this.actressIDListSlider.BackColor = System.Drawing.Color.Transparent;
            this.actressIDListSlider.IndexNames = null;
            this.actressIDListSlider.List = ((System.Collections.Generic.List<string>)(resources.GetObject("actressIDListSlider.List")));
            this.actressIDListSlider.Name = "actressIDListSlider";
            this.actressIDListSlider.ShowLabel = false;
            this.actressIDListSlider.Value = 0;
            // 
            // directorIDListSlider
            // 
            resources.ApplyResources(this.directorIDListSlider, "directorIDListSlider");
            this.directorIDListSlider.BackColor = System.Drawing.Color.Transparent;
            this.directorIDListSlider.IndexNames = null;
            this.directorIDListSlider.List = ((System.Collections.Generic.List<string>)(resources.GetObject("directorIDListSlider.List")));
            this.directorIDListSlider.Name = "directorIDListSlider";
            this.directorIDListSlider.ShowLabel = false;
            this.directorIDListSlider.Value = 0;
            // 
            // directorDDActiveAreaSlider
            // 
            this.directorDDActiveAreaSlider.DrawSlider = true;
            this.directorDDActiveAreaSlider.IndexCharacters = ((System.Collections.Generic.List<char>)(resources.GetObject("directorDDActiveAreaSlider.IndexCharacters")));
            this.directorDDActiveAreaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("directorDDActiveAreaSlider.ItemsInIndices")));
            resources.ApplyResources(this.directorDDActiveAreaSlider, "directorDDActiveAreaSlider");
            this.directorDDActiveAreaSlider.MaxItemsPerSliderPixel = 2;
            this.directorDDActiveAreaSlider.Name = "directorDDActiveAreaSlider";
            this.directorDDActiveAreaSlider.RollChangeValue = 1;
            this.directorDDActiveAreaSlider.Value = 0;
            // 
            // actorDDListSlider
            // 
            this.actorDDListSlider.IndexNames = null;
            this.actorDDListSlider.List = ((System.Collections.Generic.List<string>)(resources.GetObject("actorDDListSlider.List")));
            resources.ApplyResources(this.actorDDListSlider, "actorDDListSlider");
            this.actorDDListSlider.Name = "actorDDListSlider";
            this.actorDDListSlider.ShowLabel = false;
            this.actorDDListSlider.Value = 0;
            // 
            // actressDDListSlider
            // 
            this.actressDDListSlider.IndexNames = null;
            this.actressDDListSlider.List = ((System.Collections.Generic.List<string>)(resources.GetObject("actressDDListSlider.List")));
            resources.ApplyResources(this.actressDDListSlider, "actressDDListSlider");
            this.actressDDListSlider.Name = "actressDDListSlider";
            this.actressDDListSlider.ShowLabel = false;
            this.actressDDListSlider.Value = 0;
            // 
            // directorDDListSlider
            // 
            this.directorDDListSlider.IndexNames = null;
            this.directorDDListSlider.List = ((System.Collections.Generic.List<string>)(resources.GetObject("directorDDListSlider.List")));
            resources.ApplyResources(this.directorDDListSlider, "directorDDListSlider");
            this.directorDDListSlider.Name = "directorDDListSlider";
            this.directorDDListSlider.ShowLabel = false;
            this.directorDDListSlider.Value = 0;
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
            // actorDDActiveListSlider
            // 
            this.actorDDActiveListSlider.Data = null;
            this.actorDDActiveListSlider.IndexCharacters = null;
            this.actorDDActiveListSlider.IndexNames = null;
            resources.ApplyResources(this.actorDDActiveListSlider, "actorDDActiveListSlider");
            this.actorDDActiveListSlider.Name = "actorDDActiveListSlider";
            this.actorDDActiveListSlider.Value = 0;
            // 
            // actressDDActiveListSlider
            // 
            this.actressDDActiveListSlider.Data = null;
            this.actressDDActiveListSlider.IndexCharacters = null;
            this.actressDDActiveListSlider.IndexNames = null;
            resources.ApplyResources(this.actressDDActiveListSlider, "actressDDActiveListSlider");
            this.actressDDActiveListSlider.Name = "actressDDActiveListSlider";
            this.actressDDActiveListSlider.Value = 0;
            // 
            // directorDDActiveListSlider
            // 
            this.directorDDActiveListSlider.Data = null;
            this.directorDDActiveListSlider.IndexCharacters = null;
            this.directorDDActiveListSlider.IndexNames = null;
            resources.ApplyResources(this.directorDDActiveListSlider, "directorDDActiveListSlider");
            this.directorDDActiveListSlider.Name = "directorDDActiveListSlider";
            this.directorDDActiveListSlider.Value = 0;
            // 
            // actorIDAlphaSlider
            // 
            this.actorIDAlphaSlider.IndexCharacters = ((System.Collections.Generic.List<char>)(resources.GetObject("actorIDAlphaSlider.IndexCharacters")));
            this.actorIDAlphaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("actorIDAlphaSlider.ItemsInIndices")));
            resources.ApplyResources(this.actorIDAlphaSlider, "actorIDAlphaSlider");
            this.actorIDAlphaSlider.Name = "actorIDAlphaSlider";
            this.actorIDAlphaSlider.Value = 0;
            // 
            // directorIDAlphaSlider
            // 
            this.directorIDAlphaSlider.IndexCharacters = ((System.Collections.Generic.List<char>)(resources.GetObject("directorIDAlphaSlider.IndexCharacters")));
            this.directorIDAlphaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("directorIDAlphaSlider.ItemsInIndices")));
            resources.ApplyResources(this.directorIDAlphaSlider, "directorIDAlphaSlider");
            this.directorIDAlphaSlider.Name = "directorIDAlphaSlider";
            this.directorIDAlphaSlider.Value = 0;
            // 
            // actressIDAlphaSlider
            // 
            this.actressIDAlphaSlider.IndexCharacters = ((System.Collections.Generic.List<char>)(resources.GetObject("actressIDAlphaSlider.IndexCharacters")));
            this.actressIDAlphaSlider.ItemsInIndices = ((System.Collections.Generic.List<uint>)(resources.GetObject("actressIDAlphaSlider.ItemsInIndices")));
            resources.ApplyResources(this.actressIDAlphaSlider, "actressIDAlphaSlider");
            this.actressIDAlphaSlider.Name = "actressIDAlphaSlider";
            this.actressIDAlphaSlider.Value = 0;
            // 
            // FilmFinderGUI
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.actressIDAlphaSlider);
            this.Controls.Add(this.directorIDAlphaSlider);
            this.Controls.Add(this.actorIDAlphaSlider);
            this.Controls.Add(this.directorDDActiveListSlider);
            this.Controls.Add(this.actressDDActiveListSlider);
            this.Controls.Add(this.actorDDActiveListSlider);
            this.Controls.Add(this.directorDDListSlider);
            this.Controls.Add(this.actressDDListSlider);
            this.Controls.Add(this.actorDDListSlider);
            this.Controls.Add(this.actressDDActiveAreaSlider);
            this.Controls.Add(this.directorDDActiveAreaSlider);
            this.Controls.Add(this.actorDDActiveAreaSlider);
            this.Controls.Add(this.actressIDActiveListSlider);
            this.Controls.Add(this.directorIDActiveListSlider);
            this.Controls.Add(this.actorIDActiveListSlider);
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
            this.Controls.Add(this.directorIDActiveAreaSlider);
            this.Controls.Add(this.actressIDActiveAreaSlider);
            this.Controls.Add(this.actorIDActiveAreaSlider);
            this.Controls.Add(this.nextSliderButton);
            this.Controls.Add(this.directorDDAlphaSlider);
            this.Controls.Add(this.actressDDAlphaSlider);
            this.Controls.Add(this.actorDDAlphaSlider);
            this.Controls.Add(this.ratingRangeLabel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ratingRangeSlider);
            this.Controls.Add(this.yearRangeLabel);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.yearRangeSlider);
            this.Controls.Add(this.runningTimeRangeLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.runningTimeRangeSlider);
            this.Controls.Add(this.newDataButton);
            this.Controls.Add(this.certifactionsPanel);
            this.Controls.Add(this.actressIDListSlider);
            this.Controls.Add(this.directorIDListSlider);
            this.Controls.Add(this.actorIDListSlider);
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
		private System.Windows.Forms.Button newDataButton;
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
		private CustomSlider.DDAlphaslider actorDDAlphaSlider;
        private CustomSlider.DDAlphaslider actressDDAlphaSlider;
        private CustomSlider.DDAlphaslider directorDDAlphaSlider;
		private System.Windows.Forms.Button nextSliderButton;
		private CustomSlider.IDActiveAreaSlider actorIDActiveAreaSlider;
        private CustomSlider.IDActiveAreaSlider actressIDActiveAreaSlider;
        private CustomSlider.IDActiveAreaSlider directorIDActiveAreaSlider;
		private System.Windows.Forms.Label currentActressLabel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label6;
		private CustomSlider.IDListSlider actorIDListSlider;
        private CustomSlider.IDListSlider actressIDListSlider;
        private CustomSlider.IDListSlider directorIDListSlider;
		private System.Windows.Forms.Button previousSliderButton;
		private System.Windows.Forms.Button startExperimentButton;
		private System.Windows.Forms.Label pleaseFindLabel;
		private System.Windows.Forms.Label searchLabel;
		private System.Windows.Forms.Button startSearchButton;
		private System.Windows.Forms.Button confirmSearchButton;
		private System.Windows.Forms.Label searchConfirmLabel;
		private System.Windows.Forms.Label label9;
		private CustomSlider.IDActiveListSlider actorIDActiveListSlider;
        private CustomSlider.IDActiveListSlider directorIDActiveListSlider;
        private CustomSlider.IDActiveListSlider actressIDActiveListSlider;
        private CustomSlider.DDActiveAreaSlider actorDDActiveAreaSlider;
        private CustomSlider.DDActiveAreaSlider directorDDActiveAreaSlider;
        private CustomSlider.DDActiveAreaSlider actressDDActiveAreaSlider;
        private CustomSlider.DDListSlider actorDDListSlider;
        private CustomSlider.DDListSlider actressDDListSlider;
        private CustomSlider.DDListSlider directorDDListSlider;
        private CustomSlider.DDActiveListSlider actorDDActiveListSlider;
        private CustomSlider.DDActiveListSlider actressDDActiveListSlider;
        private CustomSlider.DDActiveListSlider directorDDActiveListSlider;
        private CustomSlider.IDAlphaslider actorIDAlphaSlider;
        private CustomSlider.IDAlphaslider directorIDAlphaSlider;
        private CustomSlider.IDAlphaslider actressIDAlphaSlider;


	}
}

