using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Context;
using BusinessLogic.Entities;

namespace Backend.Controllers;

public class LoginController : Controller
{
    // GET
    public IActionResult Login()
    {
        return View();
    } 
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult IndexAdmin([FromForm] string username,[FromForm] string password)
    {
        Utilizador user = new Utilizador();
        var db = new MyDbContext();
        IEnumerable<Utilizador> users = db.Utilizadors;
        foreach (var u in users)
        {
            if ((username.Equals(u.Username)) && password.Equals(u.Password) && u.TipoUtilizador == 1)
            {
                // ADMIN
                user = u;
                return RedirectToAction(controllerName:"Admins", actionName: "IndexAdmin");
            }
            if ((username.Equals(u.Username)) && password.Equals(u.Password) && u.TipoUtilizador == 2)
            {
                //User Manager
                user = u;
                return RedirectToAction(controllerName:"userManager", actionName: "IndexUManager");
            }
            if ((username.Equals(u.Username)) && password.Equals(u.Password) && u.TipoUtilizador == 3)
            {
                //ViewBag.id = u.IdUtilizador; 
                user = u;
                // USER
                SharedVariables.SharedVariables.userID = u.IdUtilizador;
                return RedirectToAction("IndexUser",  "Utilizador", new {@id=u.IdUtilizador});
            }
            /*if ((username.Equals(u.Username)) && password.Equals(u.Password) && u.Tipo == 4)
            {
                Global.counter = u.IdUsers;
                ViewBag.id = u.IdUsers; 
                user = u;
                //return RedirectToAction(controllerName:"UserCliente", actionName: "IndexUser", new {id = u.IdUsers});
                return RedirectToAction("UserCliente","UserCliente", new {@id=u.IdUsers});
            }*/
            
        }
        //ViewBag.admin = u.username;
        return RedirectToAction("index", Login);
    }

 
}