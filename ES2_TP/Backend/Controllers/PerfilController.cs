using BusinessLogic.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Backend.Controllers;

public class PerfilController : Controller
{
    private readonly MyDbContext _context;

    public PerfilController()
    {
        _context = new MyDbContext();
    }
    
    public async Task<IActionResult> ListarPerfis()
    {
        var myDbContext = _context.Perfils;
        return View(await myDbContext.OrderBy(u => u.NomePerfil).ToListAsync());
    }

    public async Task<IActionResult> EliminarPerfil(Guid id)
    {
        var db = new MyDbContext();
        var db2 = new MyDbContext();
        var db3 = new MyDbContext();
        var db4 = new MyDbContext();
        db2.Skillprofs.RemoveRange(db2.Skillprofs.Where(u => u.IdSkills == id));
        db2.SaveChanges();
        
        /*db3.TalentClientes.RemoveRange(db3.TalentClientes.Where(u => u.IdTalento == id));
        db3.SaveChanges();
        
        db4.Historicos.RemoveRange(db4.Historicos.Where(u => u.IdTalento == id));
        db4.SaveChanges();*/
        
        var result = new Perfil { IdPerfil = id };
        db.Perfils.Attach(result);
        db.Perfils.Remove(result);
        db.SaveChanges();
        return RedirectToAction("ListarPerfis");
    }

    public IActionResult CriarPerfil()
    {
        return View();
    }

    public IActionResult EditarPerfil(Guid id)
    {
        var db = new MyDbContext();
        var u = new Perfil { IdPerfil = id };

        var result = db.Perfils.SingleOrDefault(b => b.IdPerfil == id);
        if (result != null)
        {
            ViewBag.Id = result.IdPerfil;
            ViewBag.Nome = result.NomePerfil;
            //ViewBag.teste = result.Nome;
            //return View("teste");
            ViewBag.Pais = result.Pais;
            ViewBag.Email = result.Email;
            ViewBag.Preco = result.Precohora;
        }

        return View();
    }

    public async Task<IActionResult> EditarPerfis([FromForm] Guid id, [FromForm] string nome, [FromForm] string pais,
        [FromForm] string email, [FromForm] double precohora)
    {
        var db = new MyDbContext();
        var result = db.Perfils.SingleOrDefault(b => b.IdPerfil == id);
        if (result != null)
        {
            result.NomePerfil = nome;
            result.Pais = pais;
            result.Email = email;
            result.Precohora = precohora;
        }

        db.SaveChanges();
        return RedirectToAction("ListarPerfis");
    }

    public IActionResult RegistarPerfil([FromForm] string nome, [FromForm] string pais, [FromForm] string email,
        [FromForm] double precohora)
    {
        //User user = new User();

        var db = new MyDbContext();
        Perfil perfil = new Perfil();
        perfil.NomePerfil = nome;
        perfil.Email = email;
        perfil.Pais = pais;
        perfil.Precohora = precohora;
        db.Perfils.Add(perfil);
        db.SaveChanges();
        return RedirectToAction("ListarPerfis");
    }

    public async Task<IActionResult> ExperienciaPerfil(Guid id)
    {
        var myDbContext = _context.Experiencia;
        ViewBag.id = id;
        return View(await myDbContext.Where(u => u.IdExperiencia == id).OrderBy(u => u.IdExperiencia == id).ToListAsync());
    }

    public async Task<IActionResult> CriarExp(Guid id)
    {
        ViewBag.Id = id;
        return View();
    }
    
    public async Task<IActionResult> CriarExps(int id,[FromForm] string nomeExp, [FromForm] string Empresa,[FromForm] int AnoInicial, [FromForm] int AnoFinal)
    {
        if (AnoInicial > AnoFinal)
        {
            return View("ErroView");
        }
        var db = new MyDbContext();
        Experiencium exp = new Experiencium();
        //exp.id = id;
        exp.NomeExperiencia = nomeExp;
        exp.NomeEmpresa = Empresa;
        exp.Anoinicio = AnoInicial;
        exp.Anofim = AnoFinal;
        db.Experiencia.Add(exp);
        db.SaveChanges();
        return RedirectToAction("ListarPerfis");
    }
    
    public async Task<IActionResult> EliminarExp(Guid id)
    {
        var db = new MyDbContext();
        var result = new Experiencium() { IdExperiencia = id };
        db.Experiencia.Attach(result);
        db.Experiencia.Remove(result);
        db.SaveChanges();
        return RedirectToAction("ListarPerfis");
    }
    
    public async Task<IActionResult> ListarSkillTalento(Guid id)
    {
        {
            var myDbContext = _context.Skillprofs.Where(t => t.IdPerfil == id).Include(t=>t.IdSkills);
            // ViewBag.id = id;
            return View(await myDbContext.OrderBy(u =>u.IdPerfil == id).ToListAsync());
        }
    }
    
    public IActionResult CriarSkillPerfil()
    {
        var item = new List<SelectListItem>();

        foreach ( Skill skil in _context.Skills )
        {
            item.Add(new SelectListItem(text: skil.NomeSkills, value:skil.IdSkills.ToString()));
        }

        ViewData["IdSkills"] = new SelectList(_context.Skills, "IdSkills", "NomeSkills");
        
        var item2 = new List<SelectListItem>();

        foreach ( Perfil talento in _context.Perfils )
        {
            item.Add(new SelectListItem(text: talento.NomePerfil, value:talento.IdPerfil.ToString()));
        }

        ViewData["IdPerfil"] = new SelectList(_context.Perfils, "IdPerfil", "NomePerfil");
        
        return View();
    }
    
    public IActionResult CriaSKT([FromForm] int Anos, [FromForm] Guid fkIdSkill,[FromForm] Guid fkIdTalento)
    {

        var db = new MyDbContext();
        
        Skillprof skillprof = new Skillprof();
        skillprof.Nhoras = Anos;
        skillprof.IdSkills= fkIdSkill;
        skillprof.IdPerfil = fkIdTalento;
        db.Skillprofs.Add(skillprof);
        db.SaveChanges();

        return RedirectToAction("ListarPerfis");
    }
    public async Task<IActionResult> ListarPerfil2(string id, string searchTalento)
    {
        var myDbContext = _context.Perfils;
        return View(await myDbContext.OrderBy(u => u.NomePerfil).ToListAsync());
    }
    /*
     * Listar skills dos Perfis
     */
    public async Task<IActionResult> ListarSkillPerfil2(Guid id)
    {
        {
            var myDbContext = _context.Skillprofs.Where(t => t.IdPerfil == id).Include(t => t.IdSkillsNavigation);
            // ViewBag.id = id;
            return View(await myDbContext.OrderBy(u => u.IdPerfil == id).ToListAsync());
        }
    }

    public IActionResult RegistarPerfilUM([FromForm] string nome, [FromForm] string pais, [FromForm] string email,
        [FromForm] double precohora)
    {
        //User user = new User();

        var db = new MyDbContext();
        Perfil perfil = new Perfil();
        perfil.NomePerfil = nome;
        perfil.Email = email;
        perfil.Pais = pais;
        //ViewBag.teste = precohora;
        //return View("teste");
        perfil.Precohora = precohora;
        db.Perfils.Add(perfil);
        db.SaveChanges();
        return RedirectToAction("ListarPerfil2");
    }

    public IActionResult CriarPerfilUm()
    {
        return View();
    }

    public IActionResult CriarSkillTalentoUm()
    {
        var item = new List<SelectListItem>();

        foreach ( Skill skil in _context.Skills )
        {
            item.Add(new SelectListItem(text: skil.NomeSkills, value:skil.IdSkills.ToString()));
        }

        ViewData["IdSkills"] = new SelectList(_context.Skills, "IdSkills", "NomeSkills");
        
        var item2 = new List<SelectListItem>();

        foreach ( Perfil talento in _context.Perfils )
        {
            item.Add(new SelectListItem(text: talento.NomePerfil, value:talento.IdPerfil.ToString()));
        }

        ViewData["IdPerfil"] = new SelectList(_context.Perfils, "IdPerfil", "NomePerfil");
        
        return View();
    }

    public IActionResult CriaSKTUM([FromForm] int Anos, [FromForm] Guid fkIdSkill,[FromForm] Guid fkIdTalento)
    {

        var db = new MyDbContext();
        
        Skillprof tSkillprof = new Skillprof();
        tSkillprof.Nhoras = Anos;
        tSkillprof.IdSkills= fkIdSkill;
        tSkillprof.IdPerfil = fkIdTalento;
        db.Skillprofs.Add(tSkillprof);
        db.SaveChanges();
        
       /* 
        var itens2 = new List<SelectListItem>();

        foreach (Talento talentos in _context.Talentos)
        {
            itens2.Add(new SelectListItem{Text = talentos.Nome, Value = talentos.Id.ToString()});
        }

        ViewData["FkIdTalento"] = new SelectList(_context.Talentos, "FkIdTalento", "Talentos");
        */
        return RedirectToAction("ListarPerfil2");
    }
}