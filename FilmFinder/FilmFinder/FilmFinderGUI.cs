using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using System.Diagnostics;
using CustomSlider;
using System.IO;
using System.Text.RegularExpressions;
using FilmFinder.Properties;

namespace FilmFinder
{
	public partial class FilmFinderGUI : Form
	{
		private static Random randomGenerator = new Random();
		private const int NUMBER_OF_SLIDERS = 4;

		private MovieHandler movieHandler;
		private List<string> genreList;
        private List<CheckBox> genreCheckBoxList, certificationCheckBoxList;
		private int currSlider = -1;

		private enum SearchCategory { Actor, Actress, Director };
		private SearchCategory currentSearchCategory = 0;
		private string searchTarget;
		private Stopwatch stopwatch;
		private int userId = 3;
		private StreamWriter file;
		private int[] arrayTechnique1;
		private int[] arrayDataSize1;
		private int[] arrayTargetDensity1;
		private int[] arrayTask1;
		private int maxIndex = 0;
		private int currIndex = 0;
		private bool experimentStarted = false;

		public FilmFinderGUI(MovieHandler app)
		{
			InitializeComponent();

            genreCheckBoxList = new List<CheckBox>();
            certificationCheckBoxList = new List<CheckBox>();
			this.movieHandler = app;

			genreList = movieHandler.getUniqueGenres();
			createAllSeries();
			createGenreCheckBoxes();
            createCertificationCheckBoxes();
			drawChartForFirstTime();
			
			initializeActorSlider();
            initializeDirectorSlider();
			initializeActressSlider();

			initializeRunningTimeRangeSlider();
			initializeYearRangeSlider();
			initializeRatingRangeSlider();

			pleaseFindLabel.Hide();
			searchLabel.Hide();
			startSearchButton.Hide();
			confirmSearchButton.Hide();
			searchConfirmLabel.Hide();
			stopwatch = new Stopwatch();

			initializeFileWriting();
			setTrial();
		}

		private void initializeFileWriting()
		{
			userId = Settings.Default.ParticipantNumber;
			string fileName = "Subject " + userId + ".txt";
			file = new StreamWriter(fileName, true);
			file.Close();

			try
			{
				// Create an instance of StreamReader to read from a file.
				// The using statement also closes the StreamReader.
				using (StreamReader sr = new StreamReader(fileName))
				{
					String line;
					// Read and display lines from the file until the end of
					// the file is reached.
					while ((line = sr.ReadLine()) != null)
					{
						currIndex = int.Parse(Regex.Split(line, ",")[0]);
					}

					//Subject has already performed searches. Want to start after the last logged one
					if (currIndex > 0)
						currIndex++;
				}
			}
			catch (Exception e)
			{
				// Let the user know what went wrong.
				Console.WriteLine("The file could not be read:");
				Console.WriteLine(e.Message);
			}

			file = new StreamWriter(fileName, true);
			file.AutoFlush = true;
		}
		
		private void FilmFinderGUI_Load(object sender, System.EventArgs e)
		{
			nextSliderButton.PerformClick();
		}

		/// <summary>
		/// This method gets the active set of movies from the variable "app" and then adds each movie
		/// to it's respective series based on the first listed genre of the movie
		/// </summary>
		private void drawActiveSet()
		{
			MovieList activeMovies = movieHandler.ActiveMovies;

			//clear the series
			foreach (Series series in chart1.Series)
				series.Points.Clear();

			//Dummy point. The reason this is needed is that if there are no active movies based on whatever is being filtered
			//then none of the series will have any points. When this happens the graph disappears. By adding a transparent
			//dummy point I alleviate this problem.
			chart1.Series[0].Points.AddXY(chart1.ChartAreas[0].AxisX.Minimum, chart1.ChartAreas[0].AxisY.Minimum);
			chart1.Series[0].Points[0].Color = Color.Transparent;

			//redraw the points. 
			foreach (Movie m in activeMovies)
				chart1.Series[m.Genre].Points.AddXY(m.Year, m.Rating);

			//update the label indicating how many movies are currently visible
			label2.Text = "" + activeMovies.Count;

			
		}

		#region Initialization of controls




		/// <summary>
		/// This method creates a series for each unique genre. It also assigns a color to each series with an algorithm that I whipped up.
		/// The algorithm isn't particularly special and isn't by any means tested. Basically, I assign color to the series with a red, green and blue value.
		/// 
		/// "% 256" ensures my values stay valid
		/// 101, 17 and 53 are all prime numbers. I have found prime numbers to be useful for things like this since they don't share any factors.
		/// I multiply by "i" to help cycle through values (plus 1 is just an offset to help avoid repeat values). Through some quick testing I found that without this multiplication I get much more similar colors.
		/// </summary>
		private void createAllSeries()
		{
			int red, blue, green;

			red = blue = green = 7;

			chart1.Series.Clear();
			for (int i = 0; i < genreList.Count; i++ )
			{
				chart1.Series.Add(genreList[i]);
				chart1.Series[i].MarkerStyle = MarkerStyle.Circle;
				chart1.Series[i].ChartType = SeriesChartType.Point;
				chart1.Series[i].Color = Color.FromArgb(red, green, blue);
				chart1.Series[i].MarkerSize = 8;
				chart1.Series[i].IsVisibleInLegend = false;
				chart1.Series[i].ToolTip = "#VALX, #VALY";
				chart1.Series[i].Name = genreList[i];
				red = (red + 101*(i+1)) % 256;
				green = (green + 17*(i+1)) % 256;
				blue = (blue + 53*(i+1)) % 256;
			}

			//need to create a dummy series
		}

		/// <summary>
		/// This method, as the name implies, initializes the slider responsible for filtering actors
		/// </summary>
		private void initializeActorSlider()
		{
			List<string> uniqueActors = movieHandler.UniqueActors;
			List<string> firstCharacters = new List<string>();
			List<uint> buckets = new List<uint>();
			char lastFirstLetter = '\0';
			int lastIndex = 0;

			buckets.Add(0); //filter equivalent of "All"
			lastFirstLetter = uniqueActors[1][0]; //prime the loop and variables
			firstCharacters.Add(lastFirstLetter + "");
			for (int i = 1; i < uniqueActors.Count; i++)
			{
				if (char.ToUpper(uniqueActors[i][0]) == lastFirstLetter)
				{
					buckets[lastIndex]++;
				}
				else
				{
					lastFirstLetter = char.ToUpper(uniqueActors[i][0]);
					firstCharacters.Add(lastFirstLetter + "");
					buckets.Add(1);

					lastIndex++;
				}
			}

			actorAlphaSlider.Data = uniqueActors;
			actorAlphaSlider.Value = 0;
			actorAlphaSlider.ValueChanged += new EventHandler(actorSlider_ValueChanged);

			actorActiveAreaSlider.ItemsInIndices = buckets;
			actorActiveAreaSlider.IndexNames = firstCharacters;
			actorActiveAreaSlider.Value = 0;
			actorActiveAreaSlider.ValueChanged += new EventHandler(actorSlider_ValueChanged);

			actorMVSv3.List = uniqueActors;
			actorMVSv3.ItemsInIndices = buckets;
			actorMVSv3.IndexNames = firstCharacters;
			actorMVSv3.ShowLabel = false;
			actorMVSv3.TextChanged += new EventHandler(actorMVSv3_TextChanged);
			actorMVSv3.Value = 0;

			actorActiveMultiSlider.Data = uniqueActors;
			actorActiveMultiSlider.ItemsInIndices = buckets;
			actorActiveMultiSlider.IndexNames = firstCharacters;
			actorActiveMultiSlider.QueryChanged += new EventHandler(actorActiveMultiSlider_QueryChanged);
			actorActiveMultiSlider.Value = 0;
		}

		

		/// <summary>
		/// This method, as the name implies, initializes the slider responsible for filtering directors
		/// </summary>
        private void initializeDirectorSlider()
        {
			List<string> uniqueDirectors = movieHandler.UniqueDirectors;
			List<uint> buckets = new List<uint>();
			List<string> firstCharacters = new List<string>();
			char lastFirstLetter = '\0';
			int lastIndex = 0;

			buckets.Add(0); //filter equivalent of "All"
			lastFirstLetter = uniqueDirectors[1][0]; //prime the loop and variables
			firstCharacters.Add(lastFirstLetter + "");
			for (int i = 1; i < uniqueDirectors.Count; i++)
			{
				if (char.ToUpper(uniqueDirectors[i][0]) == lastFirstLetter)
				{
					buckets[lastIndex]++;
				}
				else
				{
					lastFirstLetter = char.ToUpper(uniqueDirectors[i][0]);
					firstCharacters.Add(lastFirstLetter + "");
					buckets.Add(1);

					lastIndex++;
				}
			}

			directorAlphaSlider.Data = uniqueDirectors;
			directorAlphaSlider.Value = 0;
			directorAlphaSlider.ValueChanged += new EventHandler(directorSlider_ValueChanged);

			directorActiveAreaSlider.ItemsInIndices = buckets;
			directorActiveAreaSlider.IndexNames = firstCharacters;
			directorActiveAreaSlider.Value = 0;
			directorActiveAreaSlider.ValueChanged += new EventHandler(directorSlider_ValueChanged);

			directorMVSc3.List = uniqueDirectors;
			directorMVSc3.ItemsInIndices = buckets;
			directorMVSc3.IndexNames = firstCharacters;
			directorMVSc3.Value = 0;
			directorMVSc3.ShowLabel = false;
			directorMVSc3.TextChanged += new EventHandler(directorMVSc3_TextChanged);

			directorActiveMultiSlider.Data = uniqueDirectors;
			directorActiveMultiSlider.ItemsInIndices = buckets;
			directorActiveMultiSlider.IndexNames = firstCharacters;
			directorActiveMultiSlider.QueryChanged += new EventHandler(directorActiveMultiSlider_QueryChanged);
			directorActiveMultiSlider.Value = 0;
        }

		

		private void initializeActressSlider()
		{
			List<string> uniqueActresses = movieHandler.UniqueActresses;
			List<uint> buckets = new List<uint>();
			List<string> firstCharacters = new List<string>();
			char lastFirstLetter = '\0';
			int lastIndex = 0;

			buckets.Add(0); //filter equivalent of "All"
			lastFirstLetter = uniqueActresses[1][0]; //prime the loop and variables
			firstCharacters.Add(lastFirstLetter + "");
			for (int i = 1; i < uniqueActresses.Count; i++)
			{
				if (char.ToUpper(uniqueActresses[i][0]) == lastFirstLetter)
				{
					buckets[lastIndex]++;
				}
				else
				{
					lastFirstLetter = char.ToUpper(uniqueActresses[i][0]);
					firstCharacters.Add(lastFirstLetter + "");
					buckets.Add(1);

					lastIndex++;
				}
			}

			actressAlphaSlider.Data = uniqueActresses;
			actressAlphaSlider.Value = 0;
			actressAlphaSlider.ValueChanged += new EventHandler(actressSlider_ValueChanged);

			actressActiveAreaSlider.ItemsInIndices = buckets;
			actressActiveAreaSlider.IndexNames = firstCharacters;
			actressActiveAreaSlider.Value = 0;
			actressActiveAreaSlider.ValueChanged += new EventHandler(actressSlider_ValueChanged);

			actressMVSv3.List = uniqueActresses;
			actressMVSv3.ItemsInIndices = buckets;
			actressMVSv3.IndexNames = firstCharacters;
			actressMVSv3.Value = 0;
			actressMVSv3.ShowLabel = false;
			actressMVSv3.TextChanged += new EventHandler(actressMVSv3_TextChanged);


			actressActiveMultiSlider.Data = uniqueActresses;
			actressActiveMultiSlider.ItemsInIndices = buckets;
			actressActiveMultiSlider.IndexNames = firstCharacters;
			actressActiveMultiSlider.QueryChanged += new EventHandler(actressActiveMultiSlider_QueryChanged);
			actressActiveMultiSlider.Value = 0;
		}

		

		private void initializeRunningTimeRangeSlider()
		{
			runningTimeRangeSlider.BoundChanged -= runningTimeRangeSlider_BoundChanged;

			List<int> runningTimes = movieHandler.getRunningTimes();
			runningTimes.Sort();
			runningTimeRangeSlider.UpperRange = runningTimes[runningTimes.Count - 1];
			runningTimeRangeSlider.UpperBound = runningTimeRangeSlider.UpperRange;

			runningTimeRangeSlider.BoundChanged += new EventHandler(runningTimeRangeSlider_BoundChanged);
			runningTimeRangeSlider.LowerRange = runningTimes[0]; //the label will be updated now
			runningTimeRangeSlider.LowerBound = runningTimeRangeSlider.LowerRange;
		}

		private void initializeYearRangeSlider()
		{
			yearRangeSlider.BoundChanged -= yearRangeSlider_BoundChanged;

			List<int> years = movieHandler.getProductionYears();
			years.Sort();
			yearRangeSlider.UpperRange = years[years.Count - 1];
			yearRangeSlider.UpperBound = yearRangeSlider.UpperRange;

			yearRangeSlider.BoundChanged += new EventHandler(yearRangeSlider_BoundChanged);
			yearRangeSlider.LowerRange = years[0]; //the label will be updated now
			yearRangeSlider.LowerBound = yearRangeSlider.LowerRange;
		}

		private void initializeRatingRangeSlider()
		{
			ratingRangeSlider.BoundChanged -= ratingRangeSlider_BoundChanged;

			List<double> ratings = movieHandler.getRatings();
			ratings.Sort();
			ratingRangeSlider.UpperRange = (int)ratings[ratings.Count - 1]*10;
			ratingRangeSlider.UpperBound = ratingRangeSlider.UpperRange;

			ratingRangeSlider.BoundChanged += new EventHandler(ratingRangeSlider_BoundChanged);
			ratingRangeSlider.LowerRange = (int)ratings[0]*10; //the label will be updated now
			ratingRangeSlider.LowerBound = ratingRangeSlider.LowerRange;
		}


        /// <summary>
        /// Initialize the chart. This means set up the axes
        /// </summary>
        private void drawChartForFirstTime()
		{
			List<int> years = movieHandler.getProductionYears(); //the production year of each movie
			List<double> ratings = movieHandler.getRatings(); //the rating of each movie

			int minYear, maxYear;
			//double minRating, maxRating;

			years.Sort();
			//ratings.Sort();

			minYear = years[0];
			maxYear = years[years.Count - 1];

			//minRating = ratings[0];
			//maxRating = ratings[ratings.Count - 1];

			
			chart1.ChartAreas[0].AxisY.Interval = 1;
			chart1.ChartAreas[0].AxisY.Minimum = 0;
			chart1.ChartAreas[0].AxisY.Maximum = 10;

			chart1.ChartAreas[0].AxisX.Interval = 20;
			chart1.ChartAreas[0].AxisX.Minimum = minYear - 3;
			chart1.ChartAreas[0].AxisX.Maximum = maxYear + (chart1.ChartAreas[0].AxisX.Interval - maxYear % chart1.ChartAreas[0].AxisX.Interval); //I want the axis to look good so I had to do some math.
												//I want my scale to end as a multiple of my interval. To do that, I need to find out far away my max value is from the next multiple. This is done in the brackets	
            
            drawActiveSet();
		}

        /// <summary>
        /// This method loops through all of the genre names and creates a checkBox for each one
        /// </summary>
		private void createGenreCheckBoxes()
		{
			for (int i = flowLayoutPanel1.Controls.Count - 1; i > 1; i-- )
			{
				flowLayoutPanel1.Controls.RemoveAt(i);
			}
			for (int i = 0; i < genreList.Count; i++)
			{
				addGenreCheckBox(genreList[i]);
			}
		}

        /// <summary>
        /// This method adds a check box to the flowlayoutpanel
        /// </summary>
        /// <param name="name">the name of the checkbox to add</param>
		private void addGenreCheckBox(String name)
		{
			//This code was basically copied from what is automatically generated by windows forms
			//when you create a check box
			CheckBox checkBox = new CheckBox();
			checkBox.AutoSize = true;
			checkBox.Name = name;
			checkBox.TabIndex = 12;
			checkBox.Text = name;
			checkBox.UseVisualStyleBackColor = true;
			checkBox.CheckState = CheckState.Checked;

			checkBox.BackColor = chart1.Series[name].Color;
			checkBox.ForeColor = Color.White;

			flowLayoutPanel1.Controls.Add(checkBox);
			checkBox.Click += new System.EventHandler(genreCheckBoxClick);
            genreCheckBoxList.Add(checkBox);
		}

		/// <summary>
		/// This method is responsible for creating as many checkboxes as there are certifications.
		/// IE: if there are 4 different certifications, 4 checkboxes will be made
		/// </summary>
        private void createCertificationCheckBoxes()
        {
			//get all of the unique certifications
            List<string> certifications = movieHandler.getUniqueCertifications();

			for (int i = certifactionsPanel.Controls.Count - 1; i > 1; i--)
			{
				certifactionsPanel.Controls.RemoveAt(i);
			}
			//create a check box for each unique certification
            foreach (string str in certifications)
            {
                addCertificationCheckBox(str);
            }
        }

		/// <summary>
		/// This method is responsible for the actual creation of the certification checkboxes. The checkbox created
		/// here is added to the flowlayout panel called certificationsPanel
		/// </summary>
		/// <param name="text">The name and text of the checkbox</param>
        private void addCertificationCheckBox(string text)
        {
            CheckBox checkBox = new CheckBox();
            checkBox.AutoSize = true;
            checkBox.Name = text;
            checkBox.Text = text;
            checkBox.CheckState = CheckState.Checked; //start the check box off as checked
			certificationCheckBoxList.Add(checkBox);
            certifactionsPanel.Controls.Add(checkBox);
            checkBox.Click += new System.EventHandler(certificationCheckBoxClick);
        }

		#endregion


		#region Event catchers for all controls

		/// <summary>
		/// This method disables all of the series. Catches the click event for the "Disable All" button.
		/// A list of all of the genres is created and passed to a method in movieHandler which takes care
		/// of disabling a list of genres.
		/// </summary>
		/// <param name="sender">Expect this to be a button</param>
		/// <param name="e">The arguments sent by the control</param>
		private void disableAllGenreButton_Click(object sender, EventArgs e)
		{
			List<string> temp = new List<string>();

			//Loop through each genre checkbox and add it's name to the list of genres to disable
			//(each checkbox is named after a different genre of movie)
            for (int i = 0; i < genreCheckBoxList.Count; i++)
            {
                genreCheckBoxList[i].CheckState = CheckState.Unchecked;
				temp.Add(genreCheckBoxList[i].Text);
            }

			//disable those movies and then draw the active set of movies
			movieHandler.disableAllGenres(temp);
			drawActiveSet();
		}

        /// <summary>
        /// This method enables all of the series. Catches the click event for the "Enable All" button
        /// </summary>
		/// <param name="sender">Expect this to be a button</param>
		/// <param name="e">The arguments sent by the control</param>
		private void enableAllGenreButton_Click(object sender, EventArgs e)
		{
            for (int i = 0; i < genreCheckBoxList.Count; i++)
            {
                genreCheckBoxList[i].CheckState = CheckState.Checked;
            }

			movieHandler.enableAllGenres();
			drawActiveSet();
		}

		/// <summary>
		/// Disables all certifications
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void disableAllCertificationsButton_Click(object sender, EventArgs e)
		{
			List<string> temp = new List<string>();

			//Loop through each genre checkbox and add it's name to the list of genres to disable
			//(each checkbox is named after a different genre of movie)
			for (int i = 0; i < certificationCheckBoxList.Count; i++)
			{
				certificationCheckBoxList[i].CheckState = CheckState.Unchecked;
				temp.Add(certificationCheckBoxList[i].Text);
			}

			//disable those movies and then draw the active set of movies
			movieHandler.disableAllCertifications(temp);
			drawActiveSet();
		}

		/// <summary>
		/// Enables all certifications
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void enableAllCertificationsButton_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < certificationCheckBoxList.Count; i++)
			{
				certificationCheckBoxList[i].CheckState = CheckState.Checked;
			}

			movieHandler.enableAllCertifications();
			drawActiveSet();
		}

        /// <summary>
        /// Handles what happens when a checkbox is clicked. Depending on the state of the check box this method
		/// will either enable or disable a series (genre).
        /// </summary>
        /// <param name="sender">Sender of the event. Expect this to only be one of the genre checkboxes</param>
        /// <param name="e">Parameters sent with the event</param>
		private void genreCheckBoxClick(object sender, EventArgs e)
		{
			CheckBox toHandle = (CheckBox)sender;
            if (toHandle.CheckState == CheckState.Checked)
                movieHandler.enableGenre(toHandle.Name);
            else if (toHandle.CheckState == CheckState.Unchecked)
                movieHandler.disableGenre(toHandle.Name);

            drawActiveSet();
		}

		/// <summary>
		/// Quite literally the same as genreCheckBoxClick but instead of genres this deals with certifications
		/// </summary>
		/// <param name="sender">Sender of the event. Expect this to only be one of the certification checkboxes</param>
		/// <param name="e">Parameters sent with the event</param>
        private void certificationCheckBoxClick(object sender, EventArgs e)
        {
            CheckBox toHandle = (CheckBox)sender;
            if (toHandle.CheckState == CheckState.Checked)
                movieHandler.enableCertification(toHandle.Name);
            else if (toHandle.CheckState == CheckState.Unchecked)
                movieHandler.disableCertification(toHandle.Name);

            drawActiveSet();
        }

		/// <summary>
		/// This method handles the event created when the value of the actor slider changes.
		/// The label showing the current actor gets updated. The movies get filtered and then
		/// the active set is redrawn
		/// </summary>
		/// <param name="sender">Sender of the event. Expect this to only be the actor slider</param>
		/// <param name="e">Parameters sent with the event</param>
		private void actorSlider_ValueChanged(object sender, EventArgs e)
		{
			int value = Int32.MinValue;
			if (sender is DensitySlider)
			{
				DensitySlider slider = (DensitySlider)sender;
				value = slider.Value;
				if (slider is AlphasliderV3)
					value = (slider as AlphasliderV3).Value;
			}

			if (value != Int32.MinValue)
			{
				currentActorLabel.Text = movieHandler.updateActorFilter(value);
				drawActiveSet();
			}
		}

		/// <summary>
		/// The same as actorSlider_valueChanged except this deals with directors and the director slider
		/// </summary>
		/// <param name="sender">Sender of the event. Expect this to only be the director slider</param>
		/// <param name="e">Parameters sent with the event</param>
		private void directorSlider_ValueChanged(object sender, EventArgs e)
		{
			int value = Int32.MinValue;
			if (sender is DensitySlider)
			{
				DensitySlider slider = (DensitySlider)sender;
				value = slider.Value;
				if (slider is AlphasliderV3)
					value = (slider as AlphasliderV3).Value; 
			}

			if (value != Int32.MinValue)
			{
				currentDirectorLabel.Text = movieHandler.updateDirectorFilter(value);
				drawActiveSet();
			}
        }

		private void actressSlider_ValueChanged(object sender, EventArgs e)
		{
			int value = Int32.MinValue;
			if (sender is DensitySlider)
			{
				DensitySlider slider = (DensitySlider)sender;
				value = slider.Value;
				if (slider is AlphasliderV3)
					value = (slider as AlphasliderV3).Value;
			}

			if (value != Int32.MinValue)
			{
				currentActressLabel.Text = movieHandler.updateActressFilter(value);
				drawActiveSet();
			}
		}

		void actorMVSv3_TextChanged(object sender, EventArgs e)
		{
			if (actorMVSv3.SelectedIndex < actorMVSv3.RangeOfValues.Count)
			{
				currentActorLabel.Text = movieHandler.updateActorFilter(actorMVSv3.RangeOfValues[actorMVSv3.SelectedIndex]);
				drawActiveSet();
			}
		}

		void actressMVSv3_TextChanged(object sender, EventArgs e)
		{
			if (actressMVSv3.SelectedIndex < actressMVSv3.RangeOfValues.Count)
			{
				currentActressLabel.Text = movieHandler.updateActressFilter(actressMVSv3.RangeOfValues[actressMVSv3.SelectedIndex]);
				drawActiveSet();
			}
		}

		void directorMVSc3_TextChanged(object sender, EventArgs e)
		{
			if (directorMVSc3.SelectedIndex < directorMVSc3.RangeOfValues.Count)
			{
				currentDirectorLabel.Text = movieHandler.updateDirectorFilter(directorMVSc3.RangeOfValues[directorMVSc3.SelectedIndex]);
				drawActiveSet();
			}
		}

		void actorActiveMultiSlider_QueryChanged(object sender, EventArgs e)
		{
			if (actorActiveMultiSlider.SelectedIndex < actorActiveMultiSlider.RangeOfValues.Count)
			{
				currentActorLabel.Text = movieHandler.updateActorFilter(actorActiveMultiSlider.Value + actorActiveMultiSlider.SelectedIndex);
				drawActiveSet();
			}
		}

		void directorActiveMultiSlider_QueryChanged(object sender, EventArgs e)
		{
			if (directorActiveMultiSlider.SelectedIndex < directorActiveMultiSlider.RangeOfValues.Count)
			{
				currentDirectorLabel.Text = movieHandler.updateDirectorFilter(directorActiveMultiSlider.Value + directorActiveMultiSlider.SelectedIndex);
				drawActiveSet();
			}
		}

		void actressActiveMultiSlider_QueryChanged(object sender, EventArgs e)
		{
			if (actressActiveMultiSlider.SelectedIndex < actressActiveMultiSlider.RangeOfValues.Count)
			{
				currentActressLabel.Text = movieHandler.updateActressFilter(actressActiveMultiSlider.Value + actressActiveMultiSlider.SelectedIndex);
				drawActiveSet();
			}
		}
		

		/// <summary>
		/// This method is responsible for created a new window outlining information about a specific movie.
		/// </summary>
		/// <param name="sender">sender of the information. Expect it to be the chart</param>
		/// <param name="e">Parameters of the vent</param>
        private void chart1_Click(object sender, MouseEventArgs e)
        {
			//Make sure it's a left click
            if (e.Button == MouseButtons.Left)
            {
				//find out what the user clicked on
                HitTestResult result = chart1.HitTest(e.X, e.Y);

				//If it's a datapoint
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
					//get the movie associated with that data point
                    DataPoint point = chart1.Series[result.Series.Name].Points[result.PointIndex];
                    Movie associatedMovie = movieHandler.findMovie(result.Series.Name, (int)point.XValue, point.YValues[0]);

					//and show it. Unlessit's null, in which case there was an error of some sort
                    if (associatedMovie == null)
                        MessageBox.Show("There has been an error. The movie you clicked on could not be found.");
                    else
                        new MovieInformationWindow(associatedMovie);
                }

            }
        }
		
		/// <summary>
		/// This method is responsible for catching the ChangedValue event created by the runningTimeRangeSelector
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void runningTimeRangeSlider_BoundChanged(object sender, EventArgs e)
        {
			movieHandler.updateRunningTimeRange(runningTimeRangeSlider.LowerBound, runningTimeRangeSlider.UpperBound);
			runningTimeRangeLabel.Text = runningTimeRangeSlider.LowerBound + " - " + runningTimeRangeSlider.UpperBound;
			drawActiveSet();
        }

		/// <summary>
		/// This method is responsible for catching the ChangedValue event created by the yearRangeSelector
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void yearRangeSlider_BoundChanged(object sender, EventArgs e)
        {
            movieHandler.updateYearRange(yearRangeSlider.LowerBound, yearRangeSlider.UpperBound);
			yearRangeLabel.Text = yearRangeSlider.LowerBound + " - " + yearRangeSlider.UpperBound;
			drawActiveSet();
        }

		/// <summary>
		/// This method is responsible for catching the ChangedValue event created by the ratingRangeSelector
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void ratingRangeSlider_BoundChanged(object sender, EventArgs e)
        {
			movieHandler.updateRatingRange(ratingRangeSlider.LowerBound, ratingRangeSlider.UpperBound);
			ratingRangeLabel.Text = ratingRangeSlider.LowerBound / 10.0 + " - " + ratingRangeSlider.UpperBound / 10.0;
            drawActiveSet();
		}

		#endregion

		private void button1_Click(object sender, EventArgs e)
		{
			movieHandler = new MovieHandler();

			genreList = movieHandler.getUniqueGenres();
			createAllSeries();

			initializeActorSlider();
			initializeDirectorSlider();
			initializeActressSlider();

			initializeRunningTimeRangeSlider();
			initializeYearRangeSlider();
			initializeRatingRangeSlider();

			createGenreCheckBoxes();
			createCertificationCheckBoxes();

			drawActiveSet();

			Debug.WriteLine(movieHandler.UniqueActors.Count + "\t\t" + movieHandler.UniqueActresses.Count + "\t\t" + movieHandler.UniqueDirectors.Count);
		}

		#region Slider cycling

		/// <summary>
		/// Hide the alphasliders
		/// </summary>
		private void hideAlphaSliders()
		{
			actorAlphaSlider.Hide();
			actressAlphaSlider.Hide();
			directorAlphaSlider.Hide();
		}

		/// <summary>
		/// Show the alpha sliders
		/// </summary>
		private void showAlphaSliders()
		{
			actorAlphaSlider.Show();
			actorAlphaSlider.Value = 0;

			actressAlphaSlider.Show();
			actressAlphaSlider.Value = 0;

			directorAlphaSlider.Show();
			directorAlphaSlider.Value = 0;
		}

		private void hideActiveAreaSliders()
		{
			actorActiveAreaSlider.Hide();
			actressActiveAreaSlider.Hide();
			directorActiveAreaSlider.Hide();
		}

		private void showActiveAreaSliders()
		{
			actorActiveAreaSlider.Show();
			actorActiveAreaSlider.Value = 0;

			actressActiveAreaSlider.Show();
			actressActiveAreaSlider.Value = 0;

			directorActiveAreaSlider.Show();
			directorActiveAreaSlider.Value = 0;
		}

		private void hideMSVv3s()
		{
			actorMVSv3.Hide();
			actressMVSv3.Hide();
			directorMVSc3.Hide();
		}

		private void showMVSv3s()
		{
			actorMVSv3.Show();
			actorMVSv3.Value = 0;

			actressMVSv3.Show();
			actressMVSv3.Value = 0;

			directorMVSc3.Show();
			directorMVSc3.Value = 0;
		}

		private void hideActiveMultiSliders()
		{
			actorActiveMultiSlider.Hide();
			actressActiveMultiSlider.Hide();
			directorActiveMultiSlider.Hide();
		}

		private void showActiveMultiSliders()
		{
			actorActiveMultiSlider.Show();
			actorActiveMultiSlider.Value = 0;

			actressActiveMultiSlider.Show();
			actressActiveMultiSlider.Value = 0;

			directorActiveMultiSlider.Show();
			directorActiveMultiSlider.Value = 0;
		}

		/// <summary>
		/// hides all sliders
		/// </summary>
		private void hideAllSliders()
		{
			hideAlphaSliders();
			hideActiveAreaSliders();
			hideMSVv3s();
			hideActiveMultiSliders();
		}

		private void nextSliderButton_Click(object sender, EventArgs e)
		{
			currSlider = (currSlider + 1) % NUMBER_OF_SLIDERS;
			changeVisibleSlider();
		}

		private void previousSliderButton_Click(object sender, EventArgs e)
		{
			//currSlider = (currSlider - 1) % 7; //since mod doesn't work properly in c# for negative numbers

			currSlider = currSlider - 1;
			if (currSlider < 0)
				currSlider = NUMBER_OF_SLIDERS - 1;

			Debug.WriteLine(currSlider.ToString());
			changeVisibleSlider();
		}

		private void changeVisibleSlider()
		{
			hideAllSliders();
			switch (currSlider)
			{
				case 0:
					showAlphaSliders();
					break;
				case 1:
					showActiveAreaSliders();
					break;
				case 2:
					showMVSv3s();
					break;
				case 3:
					showActiveMultiSliders();
					break;

			}
		}


		#endregion

		#region Experiment

		private void startExperimentButton_Click(object sender, EventArgs e)
		{
			experimentStarted = true;
			//initializeFileWriting();

			nextSliderButton.Hide();
			previousSliderButton.Hide();
			startExperimentButton.Hide();

			pleaseFindLabel.Show();
			searchLabel.Show();
			//startSearchButton.Show();
			//confirmSearchButton.Show();
			searchConfirmLabel.Show();
			confirmSearchButton.Enabled = false;
			generateNewSearch();
			disableAllSliders();
			//Debug.WriteLine(currentSearchCategory.ToString());

		}

		private void generateNewSearch()
		{
			if (currIndex > maxIndex)
			{
				MessageBox.Show("Horray you're done!");
				file.Flush();
				Settings.Default.ParticipantNumber = Settings.Default.ParticipantNumber + 1;
				Settings.Default.Save();
				file.Close();
				Environment.Exit(1);
			}

			int randomQuery = 0;
			List<string> currentCategory = new List<string>();
			DensitySlider sliderToAccess; //will need to access some density information so I'll access a slider to do that. Since all sliders have the same distribution it doesn't matter which one I access
			List<uint> itemsPerIndex;
			int largestIndex;
			bool foundSatisfyingRandomNumber = false;
			double targetLowerBound = 0.0;
			double targetUpperBound = 1.0;
			int indexOfRandomNumber;

			currentSearchCategory = (SearchCategory)arrayDataSize1[currIndex];

			//decide if the query will be of actors, directors or actresses
			if (currentSearchCategory == SearchCategory.Actor)
			{
				currentCategory = movieHandler.UniqueActors;
				sliderToAccess = actorActiveAreaSlider;
			}
			else if (currentSearchCategory == SearchCategory.Actress)
			{
				currentCategory = movieHandler.UniqueActresses;
				sliderToAccess = actressActiveAreaSlider;
			}
			else
			{
				currentCategory = movieHandler.UniqueDirectors;
				sliderToAccess = directorActiveAreaSlider;
			}

			itemsPerIndex = sliderToAccess.ItemsInIndices;
			largestIndex = sliderToAccess.findLargestIndex();

			//find a satisfying person based on whether we want the target to be in a dense area, sparse area or somewhere in between
			while (!foundSatisfyingRandomNumber)
			{
				randomQuery = randomGenerator.Next(1, currentCategory.Count);

				if (arrayTargetDensity1[currIndex] == 0)
				{
					targetLowerBound = 0.0;
					targetUpperBound = 1.0 / 3.0;
				}
				else if (arrayTargetDensity1[currIndex] == 1)
				{
					targetLowerBound = 1.0 / 3.0;
					targetUpperBound = 2.0 / 3.0;
				}
				else if (arrayTargetDensity1[currIndex] == 2)
				{
					targetLowerBound = 2.0 / 3.0;
					targetUpperBound = 1.0;
				}

				indexOfRandomNumber = findIndexOfNumber(randomQuery, itemsPerIndex);

				if (itemsPerIndex[indexOfRandomNumber] * 1.0 / itemsPerIndex[largestIndex] <= targetUpperBound && itemsPerIndex[indexOfRandomNumber] * 1.0 / itemsPerIndex[largestIndex] >= targetLowerBound)
					foundSatisfyingRandomNumber = true;
			}

			//update text telling what to search for
			searchTarget = currentCategory[randomQuery];
			searchLabel.Text = currentSearchCategory.ToString() + " " + searchTarget;

			//change which slider is visible
			currSlider = arrayTechnique1[currIndex];
			changeVisibleSlider();
		}

		private int findIndexOfNumber(int number, List<uint> itemsPerIndex)
		{
			int sumSoFar = 0;
			int result = -1;

			for (int i = 0; i < itemsPerIndex.Count && result == -1; i++)
			{
				sumSoFar += (int)itemsPerIndex[i];
				if (number <= sumSoFar)
					result = i;
			}

			return result;
		}

		private void startSearchButton_Click(object sender, EventArgs e)
		{
			confirmSearchButton.Enabled = true;
			startSearchButton.Enabled = false;
			enableAllSlider();
			stopwatch.Start();
		}

		private void confirmSearchButton_Click(object sender, EventArgs e)
		{
			stopwatch.Stop();

			disableAllSliders();

			confirmSearchButton.Enabled = false;
			startSearchButton.Enabled = true;
			bool correctSearch = checkForCorrectSearch();

			string statistics = currIndex + "," + currSlider + "," + currentSearchCategory.ToString() + "," + +arrayTargetDensity1[currIndex] + "," + stopwatch.ElapsedMilliseconds
				+ ","+ correctSearch;
			file.WriteLine(statistics);

			currIndex++;
			generateNewSearch();

			stopwatch.Reset();
		}

		private bool checkForCorrectSearch()
		{
			bool result = false;

			Label labelToCheck;

			if (currentSearchCategory == SearchCategory.Actor)
				labelToCheck = currentActorLabel;
			else if (currentSearchCategory == SearchCategory.Actress)
				labelToCheck = currentActressLabel;
			else
				labelToCheck = currentDirectorLabel;

			if(string.Equals(labelToCheck.Text, searchTarget))
				result = true;

			return result;
		}

		private void disableAllSliders()
		{
			actorActiveAreaSlider.Enabled = false;
			actorAlphaSlider.Enabled = false;
			actorMVSv3.Enabled = false;
			actorActiveMultiSlider.Enabled = false;

			directorActiveAreaSlider.Enabled = false;
			directorAlphaSlider.Enabled = false;
			directorMVSc3.Enabled = false;
			directorActiveMultiSlider.Enabled = false;

			actressActiveAreaSlider.Enabled = false;
			actressAlphaSlider.Enabled = false;
			actressMVSv3.Enabled = false;
			actressActiveMultiSlider.Enabled = false;
		}

		private void enableAllSlider()
		{
			actorActiveAreaSlider.Enabled = true;
			actorAlphaSlider.Enabled = true;
			actorMVSv3.Enabled = true;
			actorActiveMultiSlider.Enabled = true;

			directorActiveAreaSlider.Enabled = true;
			directorAlphaSlider.Enabled = true;
			directorMVSc3.Enabled = true;
			directorActiveMultiSlider.Enabled = true;

			actressActiveAreaSlider.Enabled = true;
			actressAlphaSlider.Enabled = true;
			actressMVSv3.Enabled = true;
			actressActiveMultiSlider.Enabled = true;
		}

		public void setTrial()
		{
			int[,] latinSquare;
			int index;
			int numOfBlocks = 1;
			int numOfTechnique = NUMBER_OF_SLIDERS;
			int numOfTask = 1;
			int trialPerCondition = 1;
			int numDifferentDataSize = 3;
			int numOftargetDensity = 3;

			latinSquare = generateLatinSquare();


			arrayTechnique1 = new int[1000];
			arrayDataSize1 = new int[1000];
			arrayTargetDensity1 = new int[1000];
			arrayTask1 = new int[1000];

			for (int q = 0; q < numOfBlocks; q++)
				for (int i = 0; i < numOfTechnique; i++)
					for (int k = 0; k < numDifferentDataSize; k++)
						for (int s = 0; s < numOftargetDensity; s++)
							for (int t = 0; t < numOfTask; t++)
								for (int j = 0; j < trialPerCondition; j++)
								{

									index = q * numOfTechnique * numDifferentDataSize * numOftargetDensity * numOfTask * trialPerCondition +
															 i * numDifferentDataSize * numOftargetDensity * numOfTask * trialPerCondition +
																				   k * numOftargetDensity * numOfTask * trialPerCondition +
																										s * numOfTask * trialPerCondition +
																													t * trialPerCondition +
																																		j;
									if (index > maxIndex)
										maxIndex = index;

									int offsetTech = (userId - 1) % latinSquare.GetLength(0);
									int new_i = latinSquare[offsetTech, i] - 1;
									arrayTechnique1[index] = new_i;

									arrayDataSize1[index] = k;
									arrayTargetDensity1[index] = s;
									arrayTask1[index] = t;

								}

		}

		private int[,] generateLatinSquare()
		{
			int[,] latinSquare;
			List<int> topRow = new List<int>();

			//generate the top row
			topRow.Add(0);
			topRow.Add(1);
			topRow.Add(3);
			topRow.Add(2);

			if (topRow.Count % 2 == 1)
				latinSquare = new int[2 * topRow.Count, topRow.Count];
			else
				latinSquare = new int[topRow.Count, topRow.Count];

			//put top row into multidimen array
			for (int i = 0; i < topRow.Count; i++)
			{
				latinSquare[0, i] = topRow[i];
			}

			//fill out rest of array
			for (int i = 1; i < topRow.Count; i++)
			{
				for (int j = 0; j < topRow.Count; j++)
					latinSquare[i, j] = (latinSquare[i - 1, j] + 1) % topRow.Count;
			}

			//increment all values by one
			for (int i = 0; i < topRow.Count; i++)
			{
				for (int j = 0; j < topRow.Count; j++)
					latinSquare[i, j]++;
			}

			//take left-right mirror of top half and put in bottom half
			if (topRow.Count % 2 == 1)
			{
				for (int i = topRow.Count; i < 2 * topRow.Count; i++)
				{
					for (int j = 0; j < topRow.Count; j++)
					{
						latinSquare[i, j] = latinSquare[i - topRow.Count, topRow.Count - 1 - j];
					}
				}
			}

			return latinSquare;
		}

		#endregion


		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (experimentStarted)
			{
				if (keyData == Keys.Space)
				{
					if (searchConfirmLabel.BackColor == Color.Red)
					{
						startSearchButton.Show();
						startSearchButton.PerformClick();
						startSearchButton.Hide();
						searchConfirmLabel.BackColor = Color.Green;
					}
					else if (searchConfirmLabel.BackColor == Color.Green)
					{
						confirmSearchButton.Show();
						confirmSearchButton.PerformClick();
						confirmSearchButton.Hide();
						searchConfirmLabel.BackColor = Color.Red;
					}

					return true;
				}
			}
			
			return base.ProcessDialogKey(keyData);
		}






	}//end class
}//end namespace
		