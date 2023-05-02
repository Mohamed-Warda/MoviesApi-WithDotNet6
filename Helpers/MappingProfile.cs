using AutoMapper;

namespace MoviesAPI.Helpers
{
    public class MappingProfileL:Profile
    {

        public MappingProfileL()
        {
                CreateMap<Movie, MovieDetailsDto>();
            CreateMap<MovieDto, Movie>().ForMember(src => src.Poster, options => options.Ignore());
        }
    }
}
