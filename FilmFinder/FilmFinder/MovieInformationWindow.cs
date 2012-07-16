using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FilmFinder
{
    public partial class MovieInformationWindow : Form
    {
        public MovieInformationWindow(Movie movie)
        {
            InitializeComponent();

            this.Text = movie.Title;

            directorNameLabel.Text = movie.Director;
            certificationValueLabel.Text = movie.Certificate;
            ratingValueLabel.Text = movie.Rating.ToString();
            movieNameLabel.Text = movie.Title;
            yearValueLabel.Text = movie.Year.ToString();
            runningTimeValueLabel.Text = movie.RunningTime.ToString() + " minutes";
			genreValueLabe.Text = movie.Genre;

            foreach (string str in movie.ActorList)
                actorListBox.Items.Add(str);

            foreach (string str in movie.ActressList)
                actressListBox.Items.Add(str);

            this.Visible = true;
        }

    }
}
