using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FilmFinder
{
	public class Movie
	{
		[BsonId]
		private ObjectId _id;
		private string title;
		private double rating;
		private string genre;
		private string director;
		private string certificate;
		private List<string> actors;
		private List<string> actresses;
		private List<string> actorCharacter;
		private List<string> actressCharacter;
		private int year;
		private int runningTime;
		private const string dummyValue = "empty";

		public Movie()
		{
			rating = -1.0;
			title = dummyValue;
			year = -1;
			runningTime = -1;
			genre = dummyValue;
			director = dummyValue;
			certificate = dummyValue;
			actors = new List<string>();
			actresses = new List<string>();
			actorCharacter = new List<string>();
			actressCharacter = new List<string>();
		}

		public double Rating
		{
			get { return rating; }
			set { rating = value; }
		}

		public string Title
		{
			get { return title; }
			set { title = value; }
		}

		public int Year
		{
			get { return year; }
			set { year = value; }
		}

		public int RunningTime
		{
			get { return runningTime; }
			set { runningTime = value; }
		}

		public string Genre
		{
			get { return genre; }
			set { genre = value; }
		}

		public string Director
		{
			get { return director; }
			set { director = value; }
		}

		public string Certificate
		{
			get { return certificate; }
			set { certificate = value; }
		}

		public void addActor(string name)
		{
			actors.Add(name);
		}
		public void addActorCharacter(string name)
		{
			actorCharacter.Add(name);
		}

		public void addActress(string name)
		{
			actresses.Add(name);
		}

		public void addActressCharacter(string name)
		{
			actressCharacter.Add(name);
		}

		public List<string> ActorList
		{
			get { return actors; }
			set 
			{ 
				actors = value;
				actors.Sort();
			}
		}

		public List<string> ActorCharacterList
		{
			get { return actorCharacter; }
			set { actorCharacter = value; }
		}

		public List<string> ActressList
		{
			get { return actresses; }
			set 
			{ 
				actresses = value;
				actresses.Sort();
			}
		}

		public List<string> ActressCharacterList
		{
			get { return actressCharacter; }
			set { actressCharacter = value; }
		}

		public ObjectId Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public bool isComplete()
		{
			return rating != -1 && !title.Equals(dummyValue) && year != -1 && runningTime != -1 && !genre.Equals(dummyValue) && !director.Equals(dummyValue) && !certificate.Equals(dummyValue) && actors.Count > 0 && actresses.Count > 0;
		}

		public Movie deepCopy()
		{
			Movie temp = new Movie();

			temp.Title = title;
			temp.Director = director;
			temp.Rating = rating;
			temp.RunningTime = runningTime;
			temp.Year = year;
			temp.certificate = certificate;
			temp.genre = genre;

			foreach (string str in ActorList)
				temp.addActor(str);

			foreach (string str in ActorCharacterList)
				temp.addActorCharacter(str);

			foreach (string str in ActressCharacterList)
				temp.addActressCharacter(str);

			foreach (string str in ActressList)
				temp.addActress(str);

			return temp;
		}

		//public void moveGenreToEnd(int indexOfGenre)
		//{
		//    string genreToMove = genreList[indexOfGenre];
		//    genreList.Remove(genreToMove);
		//    genreList.Add(genreToMove);
		//}

		//public void moveGenreToEnd(string genreName)
		//{
		//    moveGenreToEnd(genreList.IndexOf(genreName));
		//}

		//public void moveGenreToBeginning(int indexOfGenre)
		//{
		//    string genreToMove = genreList[indexOfGenre];
		//    genreList.Remove(genreToMove);
		//    genreList.Insert(0, genreToMove);
		//}

		//public void moveGenreToBeginning(string genreName)
		//{
		//    moveGenreToBeginning(genreList.IndexOf(genreName));
		//}
	}
}
