using BusinessLogic.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers.api;

public class admin : ControllerBase
{
    //  [HttpGet] //
        private readonly MyDbContext _context;

        public admin()
        {
            _context = new MyDbContext();
        }


        [HttpGet]
        public async Task<IActionResult> AreaProfissional()
        {
            var myDbContext = _context.Areaprofissionals;
            var data = await myDbContext.OrderBy(m => m.NomeAreaPrfossional).ToListAsync();
            return Ok(data);
        }
        
        [HttpGet("edit/{id}")]
        public IActionResult Edit([FromRoute] Guid? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Invalid ID"); // Return a 400 Bad Request response if the ID is not provided
            }

            var result = _context.Areaprofissionals.FirstOrDefault(b => b.IdAreaProfissional == id);
            if (result == null)
            {
                return NotFound("Entity not found"); // Return a 404 Not Found response if the entity is not found
            }

            var model = new
            {
                Id = result.IdAreaProfissional,
                Nome = result.NomeAreaPrfossional
            };

            return Ok(model); // Return the entity in the response body
        }

}