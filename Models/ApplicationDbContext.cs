using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Models
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
    }
}
