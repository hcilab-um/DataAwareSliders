/******************************
 * Author: Paymahn Moghadasian
 * 
 * Class: Movie.cs
 * 
 * Purpose: Create a movie object. This will be useful later for creating the gui. Movies have: title, year produced, rating, running time, actor(s) and director(s)
 * 
 *			This is lacking security. If we pass null objects or negative values the class will try to use them. Needs more security.
 * ******************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections; //For List<string>
using System.Windows.Forms;
using System.Diagnostics;

namespace FilmFinder
{
	public class OldMovie
	{
		private string title, director, certification;
		private int year, runningTime;
		private double rating;
		private List<string> actorList, genreList;

		public OldMovie()
		{
			title = director = null; // Dummy value
            certification = "No Certification";  //Not all movies have certifications
			year = runningTime = -1; //Dummy Value
			rating = -1.0;
			actorList = new List<string>();
			genreList = new List<string>();
		}


		public double Rating
		{
			get { return rating; }
			set
			{
				if (rating == -1) //make sure the rating hasn't already been set
					rating = ((int)((value)*100))/100.0;
				else
					Debug.WriteLine("Cannot change rating of a movie. Exiting program");
			}
		}

		public int Year
		{
			get { return year; }
			set
			{
				if (year == -1) //make sure the year hasn't already been set
					year = value;
				else
					Debug.WriteLine("Cannot change year of a movie. Exiting program");
			}
		}

		public List<string> GenreList
		{
			get { return new List<string>(genreList); }
		}

		public List<string> ActorList
		{
			get { return new List<string>(actorList); }
		}

		public bool addGenre(string type)
		{
			genreList.Add(type);
			return true;
		}

		public string Title
		{
            get { return title; }
			set
			{
				if (title == null) //Make sure the title hasn't already been set
					title = value;
				else
					Debug.WriteLine("Cannot change the title of a movie. Exiting program");
			}
		}

		public string Director
		{
            get { return director; }
			set
			{
				if (director == null) //make sure director hasn't already been set
					director = value;
				else
					Debug.WriteLine("Cannot change director of a movie. Exiting program");
			}
		}

		public int RunningTime
		{
            get { return runningTime; }
			set
			{
				if (runningTime == -1) //make sure the runningTime hasn't already been set
					runningTime = value;
				else
					Debug.WriteLine("Cannot change the running time of a movie. Exiting program");
			}
		}

		public void addActor(string name)
		{
			//if (actorList.Count < 5) //we can only have up to 5 actors to a movie. Check that this is the case
				actorList.Add(name);
			//else
			//	Debug.WriteLine("Cannot have more than 5 actors to a movie. Exiting program");
		}

		public string Certification
		{
            get { return certification; }
			set
			{
                if (String.Compare(certification, "No Certification") == 0)
					certification = value;
				else
					Debug.WriteLine("Cannot change the certification of a movie. Exiting program");
			}
		}

		public override string ToString()
		{
			string output = String.Format("{0,-50}|{1,-50}|{2,-50}|{3,-50}|{4,-50}|{5,-50}|{6,-50}", title, year, rating, actorList[0], director, genreList
				[0], runningTime);

			return output;
		}

		public OldMovie deepCopy()
		{
			OldMovie temp = new OldMovie();

			temp.Title = title;
			temp.Director = director;
			temp.Rating = rating;
			temp.RunningTime = runningTime;
			temp.Year = year;
			temp.Certification = certification;

			foreach (string str in genreList)
				temp.addGenre(str);

			foreach (string str in actorList)
				temp.addActor(str);

			return temp;
		}

		public void moveGenreToEnd(int indexOfGenre)
		{
			string genreToMove = genreList[indexOfGenre];
			genreList.Remove(genreToMove);
			genreList.Add(genreToMove);
		}

		public void moveGenreToEnd(string genreName)
		{
			moveGenreToEnd(genreList.IndexOf(genreName));
		}

		public void moveGenreToBeginning(int indexOfGenre)
		{
			string genreToMove = genreList[indexOfGenre];
			genreList.Remove(genreToMove);
			genreList.Insert(0,genreToMove);
		}

		public void moveGenreToBeginning(string genreName)
		{
			moveGenreToBeginning(genreList.IndexOf(genreName));
		}


	}
}
