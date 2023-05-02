using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Services;
using MoviesAPI.Models;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IGenresService _genresService;
        private readonly IMoviesService _moviesService;
        private readonly IMapper _mapper;

        private readonly List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;
        public MoviesController(IMoviesService moviesService, IMapper mapper, IGenresService genresService)
        {

            _moviesService = moviesService;
            _mapper = mapper;
            _genresService = genresService;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movie = await _moviesService.GetAll();

            var newData  =_mapper.Map<IEnumerable<MovieDetailsDto>>(movie);
            return Ok(newData);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _moviesService.GetById(id);
            if (movie==null)
                return NotFound();

            var newData = _mapper.Map<MovieDetailsDto>(movie);
            return Ok(newData);
           
        }


          [HttpGet("Genre/{id}")]
        public async Task<IActionResult> GetByGenreID(byte id)
        {
            var movie =  await _moviesService.GetAll(id);
            if (movie==null)
                return NotFound();


            var newData = _mapper.Map<IEnumerable<MovieDetailsDto>>(movie);
            return Ok(newData);
           
        }



        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {

            var movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound($"No Movie Was Found With ID {id}");

           _moviesService.Delete(movie);
            return Ok(movie);
        }






       

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] MovieDto dto)
        {
            if (dto.Poster == null)
                return BadRequest("Poster is required!");

            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
            {
                return BadRequest("Only .png and .jpg images are allowed!");

            }

            if (dto.Poster.Length > _maxAllowedPosterSize)
            {
                return BadRequest("Max allowed size for poster is 1MB!");

            }



            var isValidGenre =await  _genresService.IsvalidGenre(dto.GenreId);



            if (!isValidGenre)
            {
                return BadRequest("Invalid genere ID!");

            }

            using var dataStream = new MemoryStream();

            await dto.Poster.CopyToAsync(dataStream);

            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = dataStream.ToArray();

            _moviesService.Add(movie);

            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id ,[FromForm] MovieDto dto)
        {
            var movie =  await _moviesService.GetById(id);
            if (movie == null)
                return NotFound($"No Movie Was Found With ID {id}");



            //check the genre id
            var isValidGenre = await _genresService.IsvalidGenre(dto.GenreId);



            if (!isValidGenre)
            {
                return BadRequest("Invalid Genre ID!");

            }





            if (dto.Poster != null)
            {
                //check extention
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .JPG and .PNG are Allowed");

                //check the size of the photo
                if (_maxAllowedPosterSize < dto.Poster.Length)
                    return BadRequest("Size Must Not Exceed 1 MB");


                //change photo from FORM type to array of BYTES
                using var datastream = new MemoryStream();

                await dto.Poster.CopyToAsync(datastream);
                movie.Poster = datastream.ToArray();
            }
          

            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.Rate = dto.Rate;
            movie.Storeline = dto.Storeline;
         
            movie.GenreId = dto.GenreId;



            _moviesService.Update(movie);

            return Ok(movie);
         
        }
    }
}
