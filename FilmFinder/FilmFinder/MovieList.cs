/******************************
 * Author: Paymahn Moghadasian
 * 
 * Class: MovieList.cs
 * 
 * Purpose: Subclass of List<Movie>. Created for the sole purpose of needing a way to deep copy the list of movies
 * 
 * ******************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilmFinder
{
	public class MovieList : List<Movie>
	{
		/// <summary>
		/// This method deep copies the list. Creates a new list with new moviesS
		/// </summary>
		/// <returns></returns>
		public MovieList deepCopy()
		{
			MovieList newMovieList = new MovieList();

			foreach (Movie m in this)
				newMovieList.Add(m.deepCopy());

			return newMovieList;
		}
	}
}
