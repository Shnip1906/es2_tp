using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

public class UserManagerController : Controller{


    private readonly MyDbContext _context;

    public UserManagerController()
    {
        _context = new MyDbContext();
    }
        
    public IActionResult Create()
    {
        return View();
    }
    
    public IActionResult IndexUManager()
    {
        return View();
    }
        
    public async Task<IActionResult> Index()
    {
        var myDbContext = _context.Utilizadors.Where(f => f.Tipo == 3 );
        return View(await myDbContext.OrderBy(u => u.Nome).ToListAsync());
    }
    public IActionResult Registar([FromForm] string nome, [FromForm] string username,[FromForm] string password)
    {
        //User user = new User();
        var db2 = new MyDbContext();
        var exist = db2.Utilizadors.Where(p => p.Username == username).FirstOrDefault();
        if (exist != null)
        {
            ViewBag.teste = "O username já existe no sistema!!!";
            return View("userExist");
        }
        var db = new MyDbContext();
        Utilizador users = new Utilizador();
        users.Username = username;
        users.Password = password;
        users.Tipo = 3;
        users.NomeUtilizador = nome;
        db.Utilizadors.Add(users);
        db.SaveChanges();
        return RedirectToAction("Index");
    }
}