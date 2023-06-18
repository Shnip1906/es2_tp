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
        
        var db6 = new MyDbContext();
        var props = db6.Skills.OrderBy(m => m.NomeSkills).ToList();
    
        List<SkillsModel> skFinal = new List<SkillsModel>();
    
        var myDbContext = _context.Areaprofissionals;
        var data = await myDbContext.Where(m => props.Select(p => p.IdAreaProfissional).Contains(m.IdAreaProfissional)).OrderBy(m => m.IdAreaProfissional).ToListAsync();
    
        foreach (var prop in props)
        {
            foreach (var sk in data)
            {
                if (sk.IdAreaProfissional == prop.IdAreaProfissional)
                {
                    SkillsModel sk1 = new SkillsModel()
                    {
                        IdSkills = prop.IdSkills,
                        NomeSkills = prop.NomeSkills,
                        IdAreaProfissional = prop.IdAreaProfissional,
                        NomeAreaProfissional = sk.NomeAreaPrfossional
                    };
       
                    skFinal.Add(sk1);   
                }
            }
        }
        
        return Ok(skFinal);
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
    
    [HttpGet]
    public async Task<IActionResult> VerDadosEditSkill([FromRoute] Guid id)
    {
        if (id.Equals(null))
        {
            return BadRequest("Invalid ID"); 
        }

        var result = _context.Skills.FirstOrDefault(b => b.IdSkills == id);
        if (result == null)
        {
            return NotFound("Entity not found"); // Return a 404 Not Found response if the entity is not found
        }

        SkillsModel model = new SkillsModel()
        {
            IdSkills = result.IdSkills,
            NomeSkills = result.NomeSkills,
            IdAreaProfissional = result.IdAreaProfissional
        };

        return Ok(model);
    }
    
    [HttpPost]
    public IActionResult EditarSkill([FromBody] SkillsModel skillModel)
    {
        if (skillModel == null)
        {
            return BadRequest("Invalid model");
        }

        var db = new MyDbContext();
        var result = db.Skills.SingleOrDefault(b => b.IdSkills == skillModel.IdSkills);

        if (result == null)
        {
            return NotFound("Entity not found"); // Return a 404 Not Found response if the entity is not found
        }

        result.NomeSkills = skillModel.NomeSkills;

        db.SaveChanges();

        return Ok(); 
    }
    
    [HttpPost]
    public IActionResult RegistarSkill([FromBody] SkillsModel model)
    {
        if (model == null)
        {
            return BadRequest("Invalid model");
        }
        
        var skill = new Skill()
        {
            NomeSkills = model.NomeSkills,
            IdAreaProfissional = model.IdAreaProfissional
        };
    
        _context.Skills.Add(skill);
        _context.SaveChanges();
        
        return Ok();
    }
    
    [HttpDelete]
    public IActionResult EliminarSkill(Guid id)
    {
        var db = new MyDbContext();
        var result = new Skill() { IdSkills = id };
        db.Skills.Attach(result);
        db.Skills.Remove(result);
        db.SaveChanges();

        return Ok();
    }

}