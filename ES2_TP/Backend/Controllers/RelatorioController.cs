using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Backend.Controllers;

public class RelatorioController : Controller
{
    private readonly MyDbContext _context;


    public  RelatorioController()
    {
        _context = new MyDbContext();
    }
    public async  Task<IActionResult> Relatorio()
    {
        return View();
    }
    public async  Task<IActionResult> RelSkill()
    {
        return View();
    }
    public async  Task<IActionResult> RelS([FromForm] string nome)
    {
        var value = 0.0;
        var count = 0;
        var ski = _context.Skills.Where(p => p.NomeSkills == nome).SingleOrDefault();
        
        var talent = _context.Skillprofs.Where(p => p.IdSkills == ski.IdSkills).Include(p => p.IdPerfilNavigation).ToList();
        foreach (var tal in talent)
        {
            
            count = count + 1;
            value = value + tal.IdPerfilNavigation!.Precohora;
        }
        var media = value / count;
        var final = media * 176;
        ViewBag.teste = final;
        return View("RelatorioS");
        //return View();
    }
    public async  Task<IActionResult> RelTalentCountry()
    {
        return View("RelPerfilC");
    }
    public async  Task<IActionResult> RelT([FromForm] string pais)
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
        return View("RelatorioS");
        //return View();
    }
    public async  Task<IActionResult> PesquisaSkills()
    {
        var myDbContext = _context.Skills;
        return View(await myDbContext.OrderBy(u => u.NomeSkills).ToListAsync());
    }
    public async  Task<IActionResult> ListarTalentosSkill(Guid id)
    {
        //var ski = _context.Skils.Where(p => p.IdSki == id).SingleOrDefault();
        
        var myDbContext = _context.Skillprofs.Where(f => f.IdSkills == id).Include(u => u.IdPerfilNavigation);
        
        return View(await myDbContext.OrderBy(u => u.IdPerfilNavigation!.NomePerfil).ToListAsync());
    }
    
}