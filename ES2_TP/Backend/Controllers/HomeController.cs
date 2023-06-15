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
        var UserDb = db.Utilizadors.FirstOrDefault();
        if (UserDb == null)
        {
            Utilizador user = new Utilizador();
            user.NomeUtilizador = "admin";
            user.Username = "admin";
            user.TipoUtilizador = 1;
            user.Password = "admin";
            db.Utilizadors.Add(user);
            db.SaveChanges();
        }

        var isthere2 = db.Areaprofissionals.FirstOrDefault();
        if (isthere2 == null)
        {
            Areaprofissional pro = new Areaprofissional();
            Areaprofissional pro2 = new Areaprofissional();
            Areaprofissional pro3 = new Areaprofissional();
            Areaprofissional pro4 = new Areaprofissional();
            
            pro.NomeAreaPrfossional = "developers";
            pro2.NomeAreaPrfossional = "designers";
            pro3.NomeAreaPrfossional = "product managers";
            pro4.NomeAreaPrfossional = "project managers";
            
            db.Areaprofissionals.Add(pro);
            db.Areaprofissionals.Add(pro2);
            db.Areaprofissionals.Add(pro3);
            db.Areaprofissionals.Add(pro4);
            
            db.SaveChanges();
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

}
