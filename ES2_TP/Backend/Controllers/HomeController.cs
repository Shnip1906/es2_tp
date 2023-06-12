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

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var db = new MyDbContext();
        var db2 = new MyDbContext();
        var db3 = new MyDbContext();
        var db4 = new MyDbContext();
        var isthere = db.Utilizadors.FirstOrDefault();
        if (isthere == null)
        {
            Utilizador user = new Utilizador();
            user.NomeUtilizador = "admin";
            user.Username = "admin";
            user.TipoUtilizador = 1;
            user.Password = "admin";
            db.Utilizadors.Add(user);
            db.SaveChanges();
        }

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
    
    /*public IActionResult Registo()
    {
        return View();
    }*/

    public IActionResult Privacy()
    {
        return View();
    }

    /*[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Registar([FromForm] string nome, [FromForm] string username,[FromForm] string password)
    {
        //User user = new User();
        var db = new MyDbContext();
        User user2 = new User();
        user2.Username = username;
        user2.Password = password;
        user2.Tipo = 3;
        user2.Nome = nome;
        db.Users.Add(user2);
        db.SaveChanges();
        return RedirectToAction("index");
    }
*/

}
