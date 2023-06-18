using Backend.Models;
using BusinessLogic.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers.api;

public class relatorio: ControllerBase
{
    private readonly MyDbContext _context;

    public relatorio()
    {
        _context = new MyDbContext();
    }

    [HttpPost]
    public ActionResult<double> RelS([FromBody] SkillsModel skillModel)
    {
        var value = 0.0;
        var count = 0;
        double media;
        var ski = _context.Skills.SingleOrDefault(p => p.NomeSkills == skillModel.NomeSkills);

        if (ski == null)
        {
            return NotFound(); // Handle the case when the skill is not found
        }

        var talent = _context.Skillprofs
            .Where(p => p.IdSkills == ski.IdSkills)
            .Include(p => p.IdPerfilNavigation)
            .ToList();

        foreach (var tal in talent)
        {
            count++;
            value += tal.IdPerfilNavigation!.Precohora;
        }

        media = value / count;
        var final = media * 176;

        return Ok(final);
    }

    [HttpPost]
    public ActionResult<double> RelT([FromBody] PerfisModel perfil)
    {
        var value = 0.0;
        var count = 0;
        var talento = _context.Perfils.Where(p => p.Pais == perfil.Pais).ToList();
        foreach (var tal in talento)
        {
            count++;
            value += tal.Precohora;
        }

        var media = value / count;
        var final = media * 176;

        return Ok(final);
    }
    
}