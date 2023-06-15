using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Entities;
using BusinessLogic.Context;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

public class RelatorioUserManagerController : Controller
{
    private readonly MyDbContext _context;


    public  RelatorioUserManagerController()
    {
        _context = new MyDbContext();
    }
    public async  Task<IActionResult> RelatorioUserManager()
    {
        return View();
    }
    public async  Task<IActionResult> RelatorioSkill()
    {
        return View();
    }
    public async  Task<IActionResult> RelSkill([FromForm] string nome)
    {
        var value = 0.0;
        var count = 0;
        var ski = _context.Skills.Where(p => p.NomeSkills == nome).SingleOrDefault();
        
        var talent = _context.Skillprofs.Where(p => p.IdSkills == ski.IdSkills).Include(p => p.IdPerfilNavigation).ToList();
        foreach (var tal in talent)
        {
            
            count = count + 1;
            value = value + tal.IdPerfilNavigation.Precohora;
        }
        var media = value / count;
        var final = media * 176;
        ViewBag.final = final;
        return View("RelatorioUserManagerSkill");
        //return View();
    }
    public async  Task<IActionResult> RelPerfilPais()
    {
        return View();
    }
    public async  Task<IActionResult> RelPerfil([FromForm] string pais)
    {
        
        var value = 0.0;
        var count = 0;
        var talento = _context.Perfils.Where(p => p.Pais == pais).ToList();
        foreach (var tal in talento)
        {
            count = count + 1;
            value = value + tal.Precohora;
        }
        
        var media = value / count;
        var final = media * 176;
        
        ViewBag.teste = final;
        return View("RelatorioUserManagerSkill");
        //return View();
    }
}