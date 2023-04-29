﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly  ApplicationDbContext _context;

        public GenresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _context.Genres.OrderBy(g=>g.Name).ToListAsync();
            return Ok(genres);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]GenreDto dto)
        {

            var genre = new Genre { Name = dto.Name };
            await _context.AddAsync(genre);
            _context.SaveChanges();
            return Ok(genre);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GenreDto dto)
        {

            var genre =await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
            if (genre == null)
                return NotFound($"No Genre Was Found With ID {id}");

            genre.Name = dto.Name;
            _context.SaveChanges(); 
            return Ok(genre);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id )
        {

            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
            if (genre == null)
                return NotFound($"No Genre Was Found With ID {id}");

            _context.Genres.Remove(genre);
            _context.SaveChanges();
            return Ok(genre);
        }

    }
}
