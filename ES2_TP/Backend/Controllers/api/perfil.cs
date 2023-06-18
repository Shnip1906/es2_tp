using Backend.Models;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers.api;

public class perfil: ControllerBase
{
    //  [HttpGet] //
    private readonly MyDbContext _context;

    public perfil()
    {
        _context = new MyDbContext();
    }


    [HttpGet]
    public async Task<IActionResult> ListarPerfis()
    {
        var myDbContext = _context.Perfils;
        var data = await myDbContext.OrderBy(m => m.NomePerfil).ToListAsync();
        return Ok(data);
    }
    
    [HttpDelete]
    public IActionResult EliminarPerfil(Guid id)
    {
        var db = new MyDbContext();
        var db2 = new MyDbContext();
        var db3 = new MyDbContext();
        var db4 = new MyDbContext();
        db2.Skillprofs.RemoveRange(db2.Skillprofs.Where(u => u.IdPerfil == id));
        db2.SaveChanges();

        var result = new Perfil { IdPerfil = id };
        db.Perfils.Attach(result);
        db.Perfils.Remove(result);
        db.SaveChanges();
        
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> VerDadosEditPerfil([FromRoute] Guid id)
    {
        if (id.Equals(null))
        {
            return BadRequest("Invalid ID"); 
        }

        var result = _context.Perfils.FirstOrDefault(b => b.IdPerfil == id);
        if (result == null)
        {
            return NotFound("Entity not found"); // Return a 404 Not Found response if the entity is not found
        }

        PerfisModel model = new PerfisModel()
        {
            IdPerfil = result.IdPerfil,
            NomePerfil = result.NomePerfil,
            Pais = result.Pais,
            Email = result.Email,
            Precohora = result.Precohora,
            Publico = result.Publico
        };

        return Ok(model);
    }
    
    [HttpPost]
    public IActionResult EditarPerfil([FromBody] PerfisModel perfilModel)
    {
        if (perfilModel == null)
        {
            return BadRequest("Invalid model");
        }

        var db = new MyDbContext();
        var result = db.Perfils.SingleOrDefault(b => b.IdPerfil == perfilModel.IdPerfil);

        if (result == null)
        {
            return NotFound("Entity not found"); // Return a 404 Not Found response if the entity is not found
        }

        result.NomePerfil = perfilModel.NomePerfil;
        result.Precohora = perfilModel.Precohora;
        result.Email = perfilModel.Email;
        result.Pais = perfilModel.Pais;
        result.Publico = perfilModel.Publico;
        
        db.SaveChanges();

        return Ok(); 
    }
    
    [HttpPost]
    public IActionResult RegistarPerfil([FromBody] PerfisModel model)
    {
        if (model == null)
        {
            return BadRequest("Invalid model");
        }
        
        var perfil = new Perfil()
        {
            NomePerfil = model.NomePerfil,
            Email = model.Email,
            Pais = model.Pais,
            Precohora = model.Precohora,
            Publico = true
        };
    
        _context.Perfils.Add(perfil);
        _context.SaveChanges();
        
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> ListarExpPerfis([FromRoute] Guid? id)
    {
        if (id.Equals(null))
        {
            return BadRequest("Invalid ID"); 
        }
        
        var myDbContext = _context.Experiencia;
        var data = await myDbContext.Where(u => u.IdPerfil == id).ToListAsync();
        return Ok(data);
    }
    
    [HttpPost]
    public IActionResult RegistarExperiencia([FromBody] ExperienciaModel model)
    {
        if (model == null)
        {
            return BadRequest("Invalid model");
        }
        
        var expModel = new Experiencium()
        {
            NomeExperiencia = model.NomeExperiencia,
            NomeEmpresa = model.NomeEmpresa,
            Anoinicio = model.Anoinicio,
            Anofim = model.Anofim,
            IdPerfil = model.IdPerfil,
            Continuo = model.Anofim == 2023
        };
    
        _context.Experiencia.Add(expModel);
        _context.SaveChanges();
        
        return Ok();
    }
    
    [HttpDelete]
    public IActionResult EliminarExperiencia(Guid id)
    {
        var db = new MyDbContext();
        var result = new Experiencium() { IdExperiencia = id };
        db.Experiencia.Attach(result);
        db.Experiencia.Remove(result);
        db.SaveChanges();

        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> ListarSkillPerfil([FromRoute] Guid? id)
    {
        if (id.Equals(null))
        {
            return BadRequest("Invalid ID"); 
        }
        
        var db6 = new MyDbContext();
        var db5 = new MyDbContext();
    
        var props = db6.Skillprofs.Where(u => u.IdPerfil == id).ToList();
    
        List<SkillsProfModel> skFinal = new List<SkillsProfModel>();
    
        var myDbContext = _context.Skills;
        var data = await myDbContext.Where(m => props.Select(p => p.IdSkills).Contains(m.IdSkills)).OrderBy(m => m.IdSkills).ToListAsync();
    
        foreach (var prop in props)
        {
            foreach (var sk in data)
            {
                if (sk.IdSkills == prop.IdSkills)
                {
                    SkillsProfModel sk1 = new SkillsProfModel()
                    {
                        IdSkillsprof = prop.IdSkillsprof,
                        Nhoras = prop.Nhoras,
                        IdPerfil = prop.IdPerfil,
                        IdSkills = prop.IdSkills,
                        NomeSkills = sk.NomeSkills
                    };
       
                    skFinal.Add(sk1);   
                }
            }
        }
    
        return Ok(skFinal);
    }
    
    [HttpGet]
    public async Task<IActionResult> ListarPerfilSkill([FromRoute] Guid? id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid ID");
        }

        var props = await _context.Skillprofs
            .Where(u => u.IdSkills == id)
            .ToListAsync();

        var perfilIds = props.Select(p => p.IdPerfil).ToList();

        var perfis = await _context.Perfils
            .Where(m => perfilIds.Contains(m.IdPerfil))
            .OrderBy(m => m.IdPerfil)
            .ToListAsync();

        var perfilFinal = perfis.Select(sk => new PerfisModel
        {
            IdPerfil = sk.IdPerfil,
            NomePerfil = sk.NomePerfil,
            Pais = sk.Pais,
            Email = sk.Email,
            Precohora = sk.Precohora
        }).ToList();

        return Ok(perfilFinal);
    
    }
}