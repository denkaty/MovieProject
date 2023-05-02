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
            CreateMap<Movie, MovieViewModel>()
                .ForMember(dest => dest.DirectorFullName, opt => opt.MapFrom(src => src.Director.FirstName + " " + src.Director.LastName))
                .ReverseMap();

            CreateMap<Actor, ActorViewModel>().ReverseMap();

            CreateMap<MovieActor, MovieActorViewModel>();

            CreateMap<Director, DirectorViewModel>().ReverseMap();

            CreateMap<User, UserViewModel>().ReverseMap();

            CreateMap<Genre, GenreViewModel>().ReverseMap();

            CreateMap<MovieGenre, MovieGenreViewModel>().ReverseMap();
        }
    }
}
