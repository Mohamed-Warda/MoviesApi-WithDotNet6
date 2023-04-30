using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviessController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly List<string> _allowExtenstions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;
        public MoviessController( ApplicationDbContext context)
        {
            _context = context;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movie = await _context.Movies.Include(m=> m.Genre).OrderByDescending(m=>m.Rate).ToListAsync();
            return Ok(movie);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id==id);
            if (movie==null)
                return NotFound();

            return Ok(movie);
        }


          [HttpGet("Genre/{id}")]
        public async Task<IActionResult> GetByGenreID(int id)
        {
            var movie =  _context.Movies.Include(m => m.Genre).Where(m => m.GenreId==id);
            if (movie==null)
                return NotFound();

            return Ok(movie);
        }




        [HttpPost]
        public async Task<IActionResult> Create([FromForm] MovieDto dto)
        {

            //check extention
            if (!_allowExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .JPG and .PNG are Allowed");

            //check the size of the photo
            if(_maxAllowedPosterSize <dto.Poster.Length)
                return BadRequest("Size Must Not Exceed 1 MB");

            //check the genre id
            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);
            if(!isValidGenre)
                return BadRequest("Invalid Genre ID!");



            //change photo from FORM type to array of BYTES
            using var datastream = new MemoryStream();  

            await dto.Poster.CopyToAsync(datastream);

            var movie = new Movie {
            Title= dto.Title,
            Year= dto.Year,
            Rate= dto.Rate, 
            Storeline= dto.Storeline,   
            Poster =datastream.ToArray(),
            GenreId=dto.GenreId,    
            
            };

            await _context.Movies.AddAsync(movie);
            _context.SaveChanges();  

            return Ok(movie);
         
        }
    }
}
