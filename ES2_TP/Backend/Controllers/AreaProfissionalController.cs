using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

public class AreaProfissionalController : Controller
{
    private readonly MyDbContext _context;

    public AreaProfissionalController()
    {
        _context = new MyDbContext();
         
    }

       
      
    /**
     * Area Profissional admin
     */
    /*public async Task<IActionResult>  AreaProfissional()
    {
        _context.Utilizadors.Where(u => u.TipoUtilizador == 1);
        var myDbContext = _context.Areaprofissionals;
        return View(await myDbContext.OrderBy(m =>m.NomeAreaPrfossional).ToListAsync());
    }*/

    /*
     * Editar 
     */
    public IActionResult Edit(Guid? id)
    {
        var db = new MyDbContext();
        var result = db.Areaprofissionals.SingleOrDefault(b => b.IdAreaProfissional == id);
        ViewBag.Id = result.IdAreaProfissional;
        ViewBag.Nome = result.NomeAreaPrfossional;
        
        return View();
    }
        
    /*
     * Permite editar a area profissional
     */
    public async Task<IActionResult> Edits ([FromForm] Guid id,[FromForm] string nome)
    {
        var db = new MyDbContext();
        var result = db.Areaprofissionals.SingleOrDefault(b => b.IdAreaProfissional == id);
        result.NomeAreaPrfossional = nome;
        db.SaveChanges();
        return RedirectToAction("AreaProfissionalUserManager");
        //return RedirectToAction("AreaProfissional");
    }

    public async Task<IActionResult>  AreaProfissionalUserManager()
    {
        var myDbContext = _context.Areaprofissionals;
        return View(await myDbContext.OrderBy(m =>m.NomeAreaPrfossional).ToListAsync());

    }
}