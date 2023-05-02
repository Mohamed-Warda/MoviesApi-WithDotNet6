using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _genresService;

        public GenresController( IGenresService genresService)
        {
            _genresService = genresService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _genresService.GetAll();
            return Ok(genres);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]GenreDto dto)
        {

            var genre = new Genre { Name = dto.Name };
            await _genresService.Add(genre);
           
            return Ok(genre);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(byte id, [FromBody] Genre dto)
        {

            var genre = await _genresService.GetById(id);
            if (genre == null)
                return NotFound($"No Genre Was Found With ID {id}");



           
            genre.Name = dto.Name;
           
            _genresService.Update(genre);
            return Ok(genre);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(byte id )
        {

            var genre = await _genresService.GetById(id);
            if (genre == null)
                return NotFound($"No Genre Was Found With ID {id}");

            _genresService.Delete(genre);
            return Ok(genre);
        }



    }
}
