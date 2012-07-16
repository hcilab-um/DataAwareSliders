using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml;
using System.Windows.Forms;
using MongoDB.Driver;

namespace FilmFinder
{
    public class MovieHandler
    {
        private MovieList activeMovies, inactiveMovies;
        private List<string> disabledGenres, disabledCertifications, uniqueActors, uniqueDirectors, uniqueActresses;
        int runningTimeMin, runningTimeMax, yearMin, yearMax;
        double ratingMin, ratingMax;

		Random random = new Random();

        string actorFilter, directorFilter, actressFilter;

        public MovieHandler()
        {
			

			Stopwatch temp = new Stopwatch();
			temp.Start();

            activeMovies = new MovieList();
            inactiveMovies = new MovieList();
            disabledGenres = new List<string>();
            uniqueActors = new List<string>();
			uniqueActresses = new List<string>();
            uniqueDirectors = new List<string>();
            disabledCertifications = new List<string>();

            actorFilter = actressFilter = directorFilter = "All";

			System.Diagnostics.Process.Start(@"C:\Users\paymahn\Downloads\mongodb-win32-x86_64-2.0.5\bin\mongod.exe");
            
			//readXML("movies.xml");
			getMoviesFromDB();


            getUniqueActors();

			getUniqueActresses();

            getUniqueDirectors();

        }

		//public void readXML(String fileName)
		//{
		//    XmlTextReader reader = new XmlTextReader(fileName);
		//    Random rand = new Random();
		//    while (reader.Read())
		//    {
		//        if (reader.NodeType == XmlNodeType.Element && String.Compare(reader.Name, "movie", true) == 0)
		//            createMovie(reader, rand);

		//    }
		//}

		public void getMoviesFromDB()
		{
			MongoServer server = MongoServer.Create();
			MongoDatabase db = server.GetDatabase("MovieDatabase");
			MongoCollection<Movie> collection = db.GetCollection<Movie>("movies");
			//Random rand = new Random();
			activeMovies.Clear();
			inactiveMovies.Clear();

			foreach (Movie m in collection.FindAll())
			{
				if (random.NextDouble() < 0.05)
				{
					activeMovies.Add(m);
				}
			}
		}

        public MovieList ActiveMovies
        {
            get { return activeMovies.deepCopy(); } //defensive accessor. Returns a new list of new movies.
        }

        public List<string> UniqueActors
        {
            get { return new List<string>(uniqueActors); }
        }

        public List<string> UniqueDirectors
        {
            get { return new List<string>(uniqueDirectors); }
        }

		public List<string> UniqueActresses
		{
			get { return new List<string>(uniqueActresses); }
		}

        /// <summary>
        /// This method creates a movie. It takes in an XmlTextReader and loops until it hits an </movie> tag or until the file ends
        /// </summary>
        /// <param name="reader">This is an XmlTextReader used to parse an Xml file containing a bunch of data about movies. We expect that the file has already been opened
        ///							prior to calling this method</param>
		//private void createMovie(XmlTextReader reader, Random rand)
		//{
		//    OldMovie movie = new OldMovie();
		//    string elementName;
		//    string text;
		//    bool stillProcessing = true;
		//    //Random rand = new Random();

		//    while (reader.Read() && stillProcessing)
		//    {
		//        switch (reader.NodeType)
		//        {
		//            case XmlNodeType.Element: //We've found information about the movie and need to act accordingly

		//                elementName = reader.Name;//Find out what information we're filling out about the movie (title? year? director? etc...)
		//                reader.Read(); //move the reader to the value of the information (if we're filling out year, get ready to read in "1984")

		//                if (reader.NodeType == XmlNodeType.Text) //Make sure the element we just read is followed by text
		//                {
		//                    text = reader.Value;

		//                    //This could be better. We know that all of these setting functions return a boolean, we could check to make sure we're doing valid updates to the movie info
		//                    switch (elementName)
		//                    {
		//                        case "title":
		//                            movie.Title = text;
		//                            break;
		//                        case "director":
		//                            movie.Director = text;
		//                            break;
		//                        case "year":
		//                            movie.Year = int.Parse(text);
		//                            break;
		//                        case "length":
		//                            movie.RunningTime = int.Parse(text.Split()[0]);
		//                            break;
		//                        case "genre":
		//                            movie.addGenre(text);
		//                            break;
		//                        case "actor":
		//                            movie.addActor(text);
		//                            break;
		//                        case "rating":
		//                            movie.Rating = rand.NextDouble() * 10;
		//                            break;
		//                        case "certification":
		//                            movie.Certification = text;
		//                            break;
		//                    }//inner switch
		//                }//if statement
		//                break; //XMLNodeType.Element

		//            case XmlNodeType.EndElement:
		//                if (String.Compare(reader.Name, "movie", true) == 0) //We've found the end of information for this movie and thus are finsihed processing
		//                {
		//                    activeMovies.Add(movie);
		//                    stillProcessing = false;
		//                }
		//                break;
		//        }//Done outer switch statement. The one that switches on "reader.NodeType"
		//    }//end while
		//}//end createMovie method

        /// <summary>
        /// Linear search through a list for a target
        /// </summary>
        /// <param name="list">The list that is being searched through</param>
        /// <param name="target">The item we are searching for</param>
        /// <returns>a boolean value is returned. If we find the target true is returned, otherwise false is returned</returns>
        public bool search(List<String> list, string target)
        {
            bool found = false;

            for (int i = 0; i < list.Count && !found; i++)
            {
                if (target.Equals((string)list[i]))
                    found = true;
            }

            return found;
        }

		public int indexOfInsertion(List<string> list, string toInsert)
		{
			int result = 0;
			bool completed = false;

			if (list.Count > 0)
			{
				for (int i = 0; i < list.Count && !completed; i++)
				{
					if (toInsert.CompareTo(list[i]) >= 0)
					{
						result = i;
						completed = true;
					}
				}
			}

			return result;
		}

		/// <summary>
		/// I think this is a meaningless method I wrote when I was messing around  with WinForms and wanted to populate a listbox with
		/// movie information
		/// </summary>
		/// <param name="listBox">The list box that I wanted to populate</param>
		public void fillListBox(ListBox listBox)
        {
            for (int i = 0; i < activeMovies.Count; i++)
            {
                listBox.Items.Add(activeMovies[i].ToString() + "\n");
            }
        }

        /// <summary>
        /// This method goes through each movie in the list and extracts the ratings.
        /// </summary>
        /// <returns>Returns an integer array which is representative of the ratings of the movies</returns>
        public List<double> getRatings()
        {
			List<double> result = new List<double>();

            for (int i = 0; i < activeMovies.Count; i++)
            {
                result.Add(activeMovies[i].Rating);
            }

			for (int i = 0; i < inactiveMovies.Count; i++)
			{
				result.Add(inactiveMovies[i].Rating);
			}

            return result;
        }

        /// <summary>
        /// Exact same as getRatings() but with production years
        /// </summary>
        /// <returns></returns>
        public List<int> getProductionYears()
        {
			List<int> result = new List<int>();

            for (int i = 0; i < activeMovies.Count; i++)
            {
                result.Add(activeMovies[i].Year);
            }

			for (int i = 0; i < inactiveMovies.Count; i++)
			{
				result.Add(inactiveMovies[i].Year);
			}

            return result;
        }

        /// <summary>
        /// This method loops through each movie and each genre associated with each movie to compile a complete list
        /// of genres found in this movie database
        /// </summary>
        /// <returns></returns>
        public List<string> getUniqueGenres()
        {
            List<string> uniqueGenres = new List<string>();
			int resultOfSearch;

			foreach (Movie m in activeMovies)
			{
				resultOfSearch = uniqueGenres.BinarySearch(m.Genre);

				if (resultOfSearch < 0)
					uniqueGenres.Insert(~resultOfSearch, m.Genre);
			}

			foreach (Movie m in inactiveMovies)
			{
				resultOfSearch = uniqueGenres.BinarySearch(m.Genre);
				if (resultOfSearch < 0)
					uniqueGenres.Insert(~resultOfSearch, m.Genre);
			}

            return uniqueGenres;
        }

        public List<string> getUniqueCertifications()
        {
            List<string> uniqueCertifications = new List<string>();
			int resultOfSearch;

            foreach (Movie movie in activeMovies)
            {
				resultOfSearch = uniqueCertifications.BinarySearch(movie.Certificate);
				if (resultOfSearch < 0)
					uniqueCertifications.Insert(~resultOfSearch, movie.Certificate);
            }

			foreach (Movie movie in inactiveMovies)
			{
				resultOfSearch = uniqueCertifications.BinarySearch(movie.Certificate);
				if (resultOfSearch < 0)
					uniqueCertifications.Insert(~resultOfSearch, movie.Certificate);
			}

            return uniqueCertifications;
        }

        public List<int> getRunningTimes()
        {
            List<int> result = new List<int>();

            foreach (Movie m in activeMovies)
                result.Add(m.RunningTime);

			foreach (Movie m in inactiveMovies)
				result.Add(m.RunningTime);

            return result;
        }

        private void getUniqueDirectors()
        {
			int resultOfSearch;

			uniqueDirectors.Clear();

            foreach (Movie movie in activeMovies)
            {
				resultOfSearch = uniqueDirectors.BinarySearch(movie.Director);
				if (resultOfSearch < 0)
					uniqueDirectors.Insert(~resultOfSearch, movie.Director);
            }

			foreach (Movie movie in inactiveMovies)
			{
				resultOfSearch = uniqueDirectors.BinarySearch(movie.Director);
				if (resultOfSearch < 0)
					uniqueDirectors.Insert(~resultOfSearch, movie.Director);
			}

			uniqueDirectors.Insert(0, "All");
        }

        private void getUniqueActors()
        {
            List<string> actorsForCurrentMovie = new List<string>();
			uniqueActors.Clear();
			int resultOfSearch;
            foreach (Movie movie in activeMovies)
            {
                actorsForCurrentMovie = movie.ActorList;
                foreach (string str in actorsForCurrentMovie)
                {
					resultOfSearch = uniqueActors.BinarySearch(str);
					if (resultOfSearch < 0)
						uniqueActors.Insert(~resultOfSearch, str);
                }
            }

			foreach (Movie movie in inactiveMovies)
			{
				actorsForCurrentMovie = movie.ActorList;
				foreach (string str in actorsForCurrentMovie)
				{
					resultOfSearch = uniqueActors.BinarySearch(str);
					if (resultOfSearch < 0)
						uniqueActors.Insert(~resultOfSearch, str);
				}
			}

			uniqueActors.Insert(0, "All");
        }

		private void getUniqueActresses()
		{
			List<string> actressesForCurrentMovie = new List<string>();

			uniqueActresses.Clear();
			int resultOfSearch;
			foreach (Movie movie in activeMovies)
			{
				actressesForCurrentMovie = movie.ActressList;
				foreach (string str in actressesForCurrentMovie)
				{
					resultOfSearch = uniqueActresses.BinarySearch(str);
					if (resultOfSearch < 0)
						uniqueActresses.Insert(~resultOfSearch, str);
				}
			}

			foreach (Movie movie in inactiveMovies)
			{
				actressesForCurrentMovie = movie.ActressList;
				foreach (string str in actressesForCurrentMovie)
				{
					resultOfSearch = uniqueActresses.BinarySearch(str);
					if (resultOfSearch < 0)
						uniqueActresses.Insert(~resultOfSearch, str);
				}
			}

			uniqueActresses.Insert(0, "All");
		}


        public void disableGenre(string genreName)
        {
            if (!search(disabledGenres, genreName))
            {
                disabledGenres.Add(genreName);
                filterMovies();
            }
        }

        public void disableAllGenres(List<string> genres)
        {
            foreach (string str in genres)
				if (!search(disabledGenres, str))
                    disabledGenres.Add(str);

            removeAllFromActiveSet();
        }


        public void enableGenre(string genreName)
        {
            if (disabledGenres.Remove(genreName))
            {
				//List<string> currentGenreSet;
				//Movie movie;

				//for (int j = inactiveMovies.Count - 1; j >= 0; j--)
				//{
				//    movie = inactiveMovies[j];
				//    currentGenreSet = movie.GenreList;

				//    if (search(currentGenreSet, genreName))
				//    {
				//        movie.moveGenreToBeginning(genreName);
				//        //toBeMoved = movie;
				//        //inactiveMovies.RemoveAt(j);
				//        //activeMovies.Add(toBeMoved);
				//        //enabledMovies.Add(toBeMoved);
				//    }
				//}//end for

                filterMovies();
            }
        }

        public void enableAllGenres()
        {
            disabledGenres.Clear();

            filterMovies();
        }

        public void disableCertification(string certificationName)
        {
            if (!search(disabledCertifications, certificationName))
            {
                disabledCertifications.Add(certificationName);
                filterMovies();
            }
        }

		public void disableAllCertifications(List<string> certifications)
		{
			foreach (string str in certifications)
				if (!search(disabledCertifications, str))
				disableCertification(str);

			removeAllFromActiveSet();
		}

        public void enableCertification(string certificationName)
        {
            if (disabledCertifications.Remove(certificationName))
            {
                filterMovies();
            }
        }

		public void enableAllCertifications()
		{
			disabledCertifications.Clear();

			filterMovies();
		}

        private void removeAllFromActiveSet()
        {
            foreach (Movie movie in activeMovies)
            {
                inactiveMovies.Add(movie);
            }

            activeMovies.Clear();
        }

        private void addAllToActiveSet()
        {
            foreach (Movie movie in inactiveMovies)
            {
                activeMovies.Add(movie);
            }

            inactiveMovies.Clear();
        }

        public string updateActorFilter(int index)
        {
			//if (index == 0)
			//{
			//    actorFilter = "All";
			//}
			//else
			//{
			actorFilter = uniqueActors[index /*- 1*/];
			//}

            filterMovies();

            return actorFilter;
        }

		public string updateActressFilter(int index)
		{
			//if (index == 0)
			//{
			//    actressFilter = "All";
			//}
			//else
			//{
				actressFilter = uniqueActresses[index/* - 1*/];
			//}

			filterMovies();

			return actressFilter;
		}

        public string updateDirectorFilter(int index)
        {
			//if (index == 0)
			//{
			//    directorFilter = "All";
			//}
			//else
			//{
			    directorFilter = uniqueDirectors[index /* - 1*/];
			//}

            filterMovies();

            return directorFilter;
        }

        public void updateRunningTimeRange(int min, int max)
        {
            runningTimeMin = min;
            runningTimeMax = max;

            filterMovies();
        }

        public void updateYearRange(int min, int max)
        {
            yearMin = min;
            yearMax = max;

            filterMovies();
        }

        public void updateRatingRange(int min, int max)
        {
            ratingMin = min / 10.0;
            ratingMax = max / 10.0;

            filterMovies();
        }

        private void filterMovies()
        {
            addAllToActiveSet();

            Movie temp, current;
            for (int i = activeMovies.Count - 1; i >= 0; i--)
            {
                current = activeMovies[i];
                if (testRatingRange(current) || testYearRange(current) ||  testRunningTime(current) || testActor(current) || testDirector(current) || testCertification(current) || testGenre(current) || testActress(current))
                {
                    temp = current;
                    activeMovies.RemoveAt(i);
                    inactiveMovies.Add(temp);
                }
            }


        }



		#region Filter Tests. All of the return true if the movie being tested needs to be removed from the active set

		private bool testGenre(Movie movie)
        {
			//bool result = false;

			//List<string> currentGenreSet;
			//int numberOfMatchingGenres;

			//currentGenreSet = movie.GenreList;
			//numberOfMatchingGenres = 0;

			////loop through each disabled genre
			//for (int i = 0; i < disabledGenres.Count && numberOfMatchingGenres != currentGenreSet.Count; i++)
			//{
			//    //search for that genre in the set of genres for the current movie
			//    if (search(currentGenreSet, disabledGenres[i]))
			//    {
			//        numberOfMatchingGenres++;
			//        movie.moveGenreToEnd(disabledGenres[i]);
			//    }
			//}

			//if (numberOfMatchingGenres == currentGenreSet.Count)
			//{
			//    result = true;
			//}

			bool result;

			if (movie.Genre != null && search(disabledGenres, movie.Genre))
			{
				result = true;
			}
			else
				result = false;

			return result;
        }

        private bool testActor(Movie movie)
        {
            List<string> currentActorSet = movie.ActorList;
            bool result;

            if (String.Compare(actorFilter, "all", true) != 0)
            {
                currentActorSet = movie.ActorList;
                result = !search(currentActorSet, actorFilter);
            }
            else
            {
                result = false;
            }

            return result;
        }

		private bool testActress(Movie movie)
		{
			List<string> currentActressSet = movie.ActressList;
			bool result;

			if (String.Compare(actressFilter, "all", true) != 0)
			{
				currentActressSet = movie.ActressList;
				result = !search(currentActressSet, actressFilter);
			}
			else
			{
				result = false;
			}

			return result;
		}

        private bool testDirector(Movie movie)
        {
            bool result;

            if (String.Compare(directorFilter, "all", true) != 0)
                result = !(String.Compare(directorFilter, movie.Director) == 0);
            else
                result = false;

            return result;
        }

        private bool testCertification(Movie movie)
        {
            bool result;

            if (movie.Certificate != null && search(disabledCertifications, movie.Certificate))
            {
                result = true;
            }
            else
                result = false;

            return result;
        }

        private bool testRunningTime(Movie movie)
        {
            return !(movie.RunningTime >= runningTimeMin && movie.RunningTime <= runningTimeMax);
        }

        private bool testYearRange(Movie movie)
        {
            return !(movie.Year >= yearMin && movie.Year <= yearMax);
        }

        private bool testRatingRange(Movie movie)
        {
            return !(movie.Rating >= ratingMin && movie.Rating <= ratingMax);
        }

		#endregion



		public Movie findMovie(string genre, int year, double rating)
        {
            Movie result = null;


            foreach (Movie movie in activeMovies)
            {
                if (genre.Equals(movie.Genre) && year.Equals(movie.Year) && (rating.Equals(movie.Rating) || rating.Equals(movie.Rating - 0.01)))
                    result = movie.deepCopy();
            }


            //if (result == null)
            //{
            //    Debug.WriteLine("searching inactive");
            //    foreach (Movie movie in inactiveMovies)
            //    {
            //        if (/*genre.Equals(movie.GenreList[0]) &&*/ year.Equals(movie.Year) && rating.Equals(movie.Rating))
            //            result = movie.deepCopy();
            //    }
            //}


            //if (result == null)
            //{
            //    Debug.WriteLine("seraching active for just year");
            //    foreach (Movie movie in activeMovies)
            //        if (year.Equals(movie.Year))
            //            Debug.WriteLine(movie);
            //    Debug.WriteLine("searching active for just rating");
            //    foreach (Movie movie in activeMovies)
            //        if (rating.Equals(movie.Rating))
            //            Debug.WriteLine(movie);
            //}

            if (result == null)
            {
                Debug.WriteLine("houston, we have a problem");
                Environment.Exit(1);
            }

            return result;
        }
    }
}
