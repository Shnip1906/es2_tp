using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

public class UtilizadorController : Controller
{
    
    private readonly MyDbContext _context;

    public UtilizadorController()
    {
        _context = new MyDbContext();
    }

    public async Task<IActionResult> IndexUser()
    {
        var Id = SharedVariables.SharedVariables.userID;
        var myDbContext = _context.Utilizadors.Where(f => f.IdUtilizador == Id);
        return View(await myDbContext.OrderBy(u => u.NomeUtilizador).ToListAsync());
       
    }
    
    public async Task<IActionResult> ListarTalentos()
    {
        var myDbContext = _context.Perfils;
        return View(await myDbContext.OrderBy(u => u.NomePerfil).ToListAsync());
    }

    public IActionResult RegistarTalento([FromForm] string NomePerfil, [FromForm] string Pais, [FromForm] string Email,
        [FromForm] double Precohora)
    {
        //User user = new User();

        var db = new MyDbContext();
        Perfil perfil = new Perfil();
        perfil.NomePerfil = NomePerfil;
        perfil.Pais = Pais;
        perfil.Email = Email;
        perfil.Precohora = Precohora;
        perfil.Publico = true;
        
        db.Perfils.Add(perfil);
        db.SaveChanges();
        return RedirectToAction(controllerName:"Utilizador", actionName: "ListarTalentos");
    }

    public IActionResult CriarTalento()
    {
        return View();
    }

    public IActionResult EditarPerfil(Guid id)
    {
        //Console.WriteLine(id);
        var db = new MyDbContext();
        var result= db.Perfils.SingleOrDefault(p => p.IdPerfil == id);
        
        ViewBag.Id = result.IdPerfil;
        ViewBag.Nome = result.NomePerfil;
        ViewBag.Pais = result.Pais;
        ViewBag.Email = result.Email;
        ViewBag.Preco = result.Precohora;
        ViewBag.Publico = result.Publico;

        if (result.Publico)
        {
            ViewBag.PublicoTxt = "checked";
        }
        
        return View();
    }

    public RedirectToActionResult EditarTalentos([FromForm] string NomePerfil, [FromForm] string Pais, [FromForm] string Email,
        [FromForm] double Precohora, [FromForm] bool Publico, [FromForm] Guid Id)
    {
        Console.WriteLine(NomePerfil);
        Console.WriteLine(Pais);
        Console.WriteLine(Email);
        Console.WriteLine(Precohora);
        Console.WriteLine(Publico);
        Console.WriteLine(Id);
        
        var db = new MyDbContext();
        var result = db.Perfils.SingleOrDefault(p => p.IdPerfil == Id);
        if (result != null)
        {
            result.NomePerfil = NomePerfil;
            result.Pais = Pais;
            result.Email = Email;
            result.Precohora = Precohora;
            result.Publico = Publico;
        }

        db.SaveChanges();
        return RedirectToAction(controllerName:"Utilizador", actionName: "ListarTalentos");
    }
    public async Task<IActionResult> ListarSkills(int id)
    {
        ViewBag.Id = id;
        var myDbContext = _context.Skills;
        return View(await myDbContext.OrderBy(u => u.NomeSkills).ToListAsync());
    }

    public IActionResult CriarSkill()
    {
        return View();
    }

    public IActionResult RegistarSkill([FromForm] string NomeSkills)
    {
        var db = new MyDbContext();
        Skill skill = new Skill();
        skill.NomeSkills = NomeSkills;
        skill.IdAreaProfissional = new Guid("58a2aa6a-bb71-4040-8535-53ffcbecd19c");
        
        db.Skills.Add(skill);
        db.SaveChanges();
        
        //Areas profissionais necessitam já estar criadas
        return RedirectToAction("ListarSkills");
    }

    public  async Task<IActionResult> ElimininarSkill(Guid id)
    {
        
        //Falta implementar a verificação se está encontra associada a algum perfil de talento
        var db = new MyDbContext();
        var result = new Skill() { IdSkills = id };
        db.Skills.Attach(result);
        db.Skills.Remove(result);
        await db.SaveChangesAsync();
        return RedirectToAction("ListarSkills");
    }

    public async Task<IActionResult> ListarAssociacoesSkillTalento (int id)
    {
        ViewBag.Id = id;
        var myDbContext = _context.Skillprofs;
        return View(await myDbContext.OrderBy(u => u.Nhoras).ToListAsync());
    }
}