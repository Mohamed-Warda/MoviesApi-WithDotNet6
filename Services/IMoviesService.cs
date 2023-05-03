using API.Helpers;
using MoviesAPI.Helpers;

namespace MoviesApi.Services
{
    public interface IMoviesService
    {
        Task<PagedList<Movie>> GetAll(PaginationParams paginationParams);
        Task<Movie> GetById(int id);
        Task<Movie> Add(Movie movie);
        Movie Update(Movie movie);
        Movie Delete(Movie movie);
    }
}