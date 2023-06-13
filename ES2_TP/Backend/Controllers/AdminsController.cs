using BusinessLogic.Context;
using Backend.Controllers;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

public class AdminsController : Controller
{
    //  [HttpGet] //
    private readonly MyDbContext _context;

    public AdminsController()
    {
        _context = new MyDbContext();
    }

    public IActionResult IndexAdmin()
    {
        return View();
    }


    public async Task<IActionResult>  AreaProfissional()
    {

        var myDbContext = _context.Areaprofissionals;
        return View(await myDbContext.OrderBy(m =>m.NomeAreaPrfossional).ToListAsync());

    }

/*
    public IActionResult Create()
    {
        throw new NotImplementedException();
    }

    public IActionResult Edit()
    {
        throw new NotImplementedException();
    }

    public IActionResult Delete()
    {
        throw new NotImplementedException();
    }
    */


    public IActionResult Edit(Guid? id)
    {
        var db = new MyDbContext();
        var result = db.Areaprofissionals.SingleOrDefault(b => b.IdAreaProfissional == id);
        ViewBag.Id = result.IdAreaProfissional;
        ViewBag.Nome = result.NomeAreaPrfossional;
        //ViewBag.teste = result.Nome;
        //return View("teste");

        
        return View();
    }
    public async Task<IActionResult> Edits ([FromForm] Guid id,[FromForm] string nome)
    {
        var db = new MyDbContext();
        var result = db.Areaprofissionals.SingleOrDefault(b => b.IdAreaProfissional == id);
        //ViewBag.teste = result.Nome;
        //return View("teste");
        result.NomeAreaPrfossional = nome;
        db.SaveChanges();
        return RedirectToAction("AreaProfissional");
    }

    
}