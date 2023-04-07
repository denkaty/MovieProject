﻿using MovieProject.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieProject.Models
{
    public class MovieVM
    {
        public string MovieId { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Released { get; set; }
        public int Runtime { get; set; }
        public string Genre { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Poster { get; set; }
        public decimal BoxOffice { get; set; }
        public string DirectorId { get; set; }
        public Director Director { get; set; }
        public ICollection<MovieActor> MoviesActors { get; set; }
        public ICollection<MovieWriter> MoviesWriters { get; set; }
    }
}