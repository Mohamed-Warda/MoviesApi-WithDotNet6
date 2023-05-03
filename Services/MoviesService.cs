using API.Helpers;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MoviesApi.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly ApplicationDbContext _context;

        public MoviesService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Movie> Add(Movie movie)
        {
            await _context.AddAsync(movie);
            _context.SaveChanges();

            return movie;
        }

        public Movie Delete(Movie movie)
        {
            _context.Remove(movie);
            _context.SaveChanges();

            return movie;
        }

        public async Task<PagedList<Movie>> GetAll(PaginationParams paginationParams)

        {
         
               var  quary = _context.Movies
              .Where(x => x.Genre.Name.ToLower() == paginationParams.genreName || paginationParams.genreName==null)
                .Include(m => m.Genre)
             .AsNoTracking();
            



            if (paginationParams.rate == "descending")
            {
             var   newQuary = quary.OrderByDescending(m => m.Rate);
                return await PagedList<Movie>.CreateAsync(newQuary, paginationParams.PageNumber, paginationParams.PageSize);


            }



            return await PagedList<Movie>.CreateAsync(quary, paginationParams.PageNumber, paginationParams.PageSize);


        }

        public Task<Movie> GetById(int id)
        {
            throw new NotImplementedException();
        }


        /*   public async Task<Movie> GetById(int id)
           {
               return await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);
           }*/

        public Movie Update(Movie movie)
        {
            _context.Update(movie);
            _context.SaveChanges();

            return movie;
        }

        
    }
}