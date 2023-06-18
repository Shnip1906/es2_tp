using Backend.Models;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers.api;

public class skill : ControllerBase
{
    private readonly MyDbContext _context;

    public skill()
    {
        _context = new MyDbContext();
    }

    [HttpGet]
    public async Task<IActionResult> ListarSkills()
    {
        var myDbContext = _context.Skills;
        var data = await myDbContext.OrderBy(m => m.NomeSkills).ToListAsync();
        return Ok(data);
    }
    
    [HttpPost]
    public IActionResult RegistarSkillProfissional([FromBody] SkillsProfModel model)
    {
        if (model == null)
        {
            return BadRequest("Invalid model");
        }
        
        var skillM = new Skillprof()
        {
            Nhoras = model.Nhoras,
            IdSkills = model.IdSkills,
            IdPerfil = model.IdPerfil
        };
    
        _context.Skillprofs.Add(skillM);
        _context.SaveChanges();
        
        return Ok();
    }
    
    [HttpDelete]
    public IActionResult EliminarSkillAssoc(Guid id)
    {
        var db = new MyDbContext();
        var result = new Skillprof() { IdSkillsprof = id };
        db.Skillprofs.Attach(result);
        db.Skillprofs.Remove(result);
        db.SaveChanges();

        return Ok();
    }

}