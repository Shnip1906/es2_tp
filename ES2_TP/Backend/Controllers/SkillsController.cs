using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Backend.Controllers;

public class SkillsController : Controller
{
    private readonly MyDbContext _context;


    public  SkillsController()
    {
        _context = new MyDbContext();
    }
    
    public async  Task<IActionResult> IndexSkills()
    {
        var myDbContext = _context.Skills.Include(u=>u.IdAreaProfissionalNavigation);
        return View(await myDbContext.OrderBy(u => u.NomeSkills).ToListAsync());
    }
    
    public IActionResult Edit(Guid id)
    {
        var db = new MyDbContext();
        var result = db.Skills.SingleOrDefault(s => s.IdSkills == id);
        ViewBag.Id = result.IdSkills;
        ViewBag.Nome = result.NomeSkills;
      //ViewBag.FkIdareaProfNavigation.Nome = result.FkIdareaProfNavigation;
       
        return View();
    }
    
    public async Task<IActionResult> Edits ([FromForm] Guid id,[FromForm] string nome, [FromForm] int fk_idareaProf, [FromForm] int FkIdareaProfNavigation, [FromForm] string NomeArea)
    {
        var db = new MyDbContext();
        var result = db.Skills.SingleOrDefault(s => s.IdSkills == id);
        //ViewBag.teste = result.Nome;
        //return View("teste");
        result.NomeSkills = nome;
       // result.FkIdareaProfNavigation.Nome = NomeArea ;
      
        db.SaveChanges();
        return RedirectToAction("IndexSkills");
    }

    public IActionResult Create()
    
    {
        var item = new List<SelectListItem>();

        foreach ( Areaprofissional areaProfissional in _context.Areaprofissionals )
        {
            item.Add(new SelectListItem(text: areaProfissional.NomeAreaPrfossional, value:areaProfissional.IdAreaProfissional.ToString()));
        }

        ViewData["IdProf"] = new SelectList(_context.Areaprofissionals, "IdProf", "Nome");

        return View();

    }

    public IActionResult Registar([FromForm] string nome, [FromForm] Guid fkIdareaProf)
    {
      
        var db = new MyDbContext();
        var skil2 = new Skill();
          //var categoryList = skil2.FkIdareaProfNavigation.Select(c => c.Nome).ToList();
       skil2.NomeSkills = nome;
       skil2.IdAreaProfissional = fkIdareaProf;
      // ViewBag.skil2 = ToSelectList(_context, "FkIdareaProf", "fk");
        db.Skills.Add(skil2);
        db.SaveChanges();
        return RedirectToAction("IndexSkills");
       
        
    }
    public async Task<IActionResult> Eliminar(Guid id)
    {
        var db = new MyDbContext();
        var db2 = new MyDbContext();
        var dSkillprof = db.Skillprofs.Where(p => p.IdSkills == id).FirstOrDefault();
        if (dSkillprof != null)
        {
            return View("erro");
        }
        var result = new Skill() { IdSkills = id };
     
        db.Skills.Attach(result);
        db.Skills.Remove(result);
        db.SaveChanges();
        return RedirectToAction("IndexSkills");
    }
   
    public async  Task<IActionResult> IndexSkillsManager()
    {
        var myDbContext = _context.Skills.Include(u=>u.IdAreaProfissionalNavigation);
        return View(await myDbContext.OrderBy(u => u.NomeSkills).ToListAsync());
    }

    public IActionResult CreateUm()
    {  
        var item = new List<SelectListItem>();

        foreach ( Areaprofissional areaProfissional in _context.Areaprofissionals )
        {
            item.Add(new SelectListItem(text: areaProfissional.NomeAreaPrfossional, value:areaProfissional.IdAreaProfissional.ToString()));
        }

        ViewData["IdAreaProfissional"] = new SelectList(_context.Areaprofissionals, "IdAreaProfissional", "NomeAreaPrfossional");
        return View();
        
    }

    public IActionResult RegistarUM([FromForm] string nome, [FromForm] Guid fkIdareaProf)
    {
      
        var db = new MyDbContext();

        Skill skill = new Skill();
        skill.NomeSkills = nome;
        skill.IdAreaProfissional = fkIdareaProf;
        db.Skills.Add(skill);
        db.SaveChanges();
        return RedirectToAction("IndexSkillsManager");
    }
}