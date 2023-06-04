using AutoMapper;
using MovieProject.Data.Entities;
using MovieProject.Models;
using MovieProject.Services.ApiClient.ViewModels.ImportViewModel;
using MovieProject.ViewModels;

namespace MovieProject.MappingConfiguration
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieViewModel>()
                .ForMember(dest => dest.DirectorFullName, opt => opt.MapFrom(src => src.Director.FirstName + " " + src.Director.LastName))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => string.Join(", ", src.MoviesGenres.Select(mg => mg.Genre.Name))))
                .ReverseMap();

            CreateMap<Actor, ActorViewModel>().ReverseMap();

            CreateMap<MovieActor, MovieActorViewModel>();

            CreateMap<Director, DirectorViewModel>().ReverseMap();

            CreateMap<Genre, GenreViewModel>().ReverseMap();

            CreateMap<MovieGenre, MovieGenreViewModel>().ReverseMap();

            CreateMap<Movie, Movie>();


            CreateMap<MovieImportDto, Movie>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.MovieId))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => DateTime.Parse(src.Released).Year))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => String.Join(", ", src.Genres)));

            CreateMap<GenreImportDto, Genre>()
                .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.GenreId));


            CreateMap<MovieStaffImportDto, Actor>()
                .ForMember(dest => dest.ActorId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FullName.Split()[0]))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FullName.Split()[1]));

            CreateMap<MovieStaffImportDto, Director>()
                .ForMember(dest => dest.DirectorId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FullName.Split()[0]))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FullName.Split()[1]));
        }
    }
}
