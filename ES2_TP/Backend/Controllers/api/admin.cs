using Backend.Models;
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
        
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] Guid id)
        {
            if (id.Equals(null))
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
        
        [HttpPost]
        public IActionResult Edits([FromBody] AreaProfissionalModel areaProfissionalModel)
        {
            if (areaProfissionalModel == null)
            {
                return BadRequest("Invalid model"); // Return a 400 Bad Request response if the model is null
            }

            var db = new MyDbContext();
            var result = db.Areaprofissionals.SingleOrDefault(b => b.IdAreaProfissional == areaProfissionalModel.IdAreaProfissional);

            if (result == null)
            {
                return NotFound("Entity not found"); // Return a 404 Not Found response if the entity is not found
            }

            result.NomeAreaPrfossional = areaProfissionalModel.NomeAreaPrfossional;
            db.SaveChanges();

            return Ok(); // Redirect to the "AreaProfissional" action
        }

}