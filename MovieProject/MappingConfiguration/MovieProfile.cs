using AutoMapper;
using MovieProject.Data.Entities;
using MovieProject.Models;
using MovieProject.ViewModels;

namespace MovieProject.MappingConfiguration
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieViewModel>().ReverseMap();

            CreateMap<Actor, ActorViewModel>().ReverseMap();

            CreateMap<MovieActor, MovieActorViewModel>().ReverseMap();

            CreateMap<Director, DirectorViewModel>().ReverseMap();

            CreateMap<User, UserViewModel>().ReverseMap();

            CreateMap<Writer, WriterViewModel>().ReverseMap();

            CreateMap<MovieWriter, MovieWriterViewModel>().ReverseMap();
        }
    }
}
