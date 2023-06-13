using System.Diagnostics;
using Backend.Models;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

public class RegistoController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    //Handling errors
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    public IActionResult Registar([FromForm] string NomeUtilizador, [FromForm] string username,[FromForm] string password)
    {
        //User user = new User();
        var db = new MyDbContext();
        Utilizador user = new Utilizador();
        user.Username = username;
        user.Password = password;
        user.TipoUtilizador = 3;
        user.NomeUtilizador = NomeUtilizador;
        db.Utilizadors.Add(user);
        db.SaveChanges();
        return RedirectToAction(controllerName:"Home", actionName: "Index");
    }
}