using System.Diagnostics;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backend.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MyDbContext _context;

    public HomeController(ILogger<HomeController> logger, MyDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        // Caso não tenha dados na tabela Utilizador criar o admin
        var db = new MyDbContext();
        /*var UserDb = db.Utilizadors.FirstOrDefault();
        if (UserDb == null)
        {
            Utilizador user = new Utilizador();
            user.NomeUtilizador = "admin";
            user.Username = "admin";
            user.TipoUtilizador = 1;
            user.Password = "admin";
            db.Utilizadors.Add(user);
            db.SaveChanges();
        }*/

       /* var isthere2 = db.AreaProfissionals.FirstOrDefault();
        if (isthere2 == null)
        {
            AreaProfissional pro = new AreaProfissional();
            AreaProfissional pro2 = new AreaProfissional();
            AreaProfissional pro3 = new AreaProfissional();
            AreaProfissional pro4 = new AreaProfissional();
            pro.Nome = "developers";
            db.AreaProfissionals.Add(pro);
            db.SaveChanges();
            
            pro2.Nome = "designers";
            db2.AreaProfissionals.Add(pro2);
            db2.SaveChanges();
            
            pro3.Nome = "product managers";
            db3.AreaProfissionals.Add(pro3);
            db3.SaveChanges();
            
            pro4.Nome = "project managers";
            db4.AreaProfissionals.Add(pro4);
            db4.SaveChanges();
        }*/
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

}
