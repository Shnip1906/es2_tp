using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Backend.Controllers;

public class UsersController : Controller
{

    private readonly MyDbContext _context;

    public UsersController()
    {
        _context = new MyDbContext();
    }
    
    public IActionResult Create()
    {
        return View();
    }
    public IActionResult Registar([FromForm] string NomeUtilizador, [FromForm] string Username,[FromForm] string Password, [FromForm] int TipoUtilizador)
    {
        //User user = new User();
        
        var db2 = new MyDbContext();
        var exist = db2.Utilizadors.Where(p => p.Username == Username).FirstOrDefault();
        if (exist != null)
        {
            ViewBag.teste = "O username já existe no sistema!!!";
            return View("teste");
        }
        
        var db = new MyDbContext();
        Utilizador users = new Utilizador();
        users.NomeUtilizador = NomeUtilizador;
        users.Username = Username;
        users.Password = Password;
        users.TipoUtilizador = TipoUtilizador;
        db.Utilizadors.Add(users);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Index()
    {
        var myDbContext = _context.Utilizadors;
        return View(await myDbContext.OrderBy(u => u.NomeUtilizador).ToListAsync());
    }

    public IActionResult Edit(Guid id)
    {
        var db = new MyDbContext();
        //var u = new Talento { Id = id };
        
        var result = db.Utilizadors.SingleOrDefault(b => b.IdUtilizador == id);
        ViewBag.Id = result.IdUtilizador;
        ViewBag.Nome = result.NomeUtilizador;
        //ViewBag.teste = result.Nome;
        //return View("teste");
        ViewBag.Username = result.Username;
        ViewBag.Password = result.Password;
        
        return View();
    }
    public async Task<IActionResult> Edits([FromForm] Guid Id,[FromForm] string NomeUtilizador, [FromForm] string Username,[FromForm] string Password)
    {
        var db = new MyDbContext();
        var result = db.Utilizadors.SingleOrDefault(b => b.IdUtilizador == Id);
        //ViewBag.teste = result.Nome;
        //return View("teste");
        result.NomeUtilizador = NomeUtilizador;
        result.Username = Username;
        result.Password = Password;
        db.SaveChanges();
        return RedirectToAction("Index");
    }
  

    /*public IActionResult Delete(Guid IdUtilizador)
    {
        var db = new MyDbContext();
        var db2 = new MyDbContext();
        var db3 = new MyDbContext();
        var db4 = new MyDbContext();
        var db5 = new MyDbContext();
        var db6 = new MyDbContext();
        db2.Clientes.RemoveRange(db2.Clientes.Where(u => u.IdCliente == IdUtilizador));
        db2.SaveChanges();

        db3..RemoveRange(db3.TalentClientes.Where(u => u.IdCliente == id));
        db3.SaveChanges();

        var props = db6.Propostas.Where(u => u.IdCliente == IdUtilizador).ToList();
        foreach (var prop in props)
        {
            db5.Skillsproposta.RemoveRange(db5.Skillsproposta.Where(u => u.IdPropostas == prop.IdPropostas));
            db5.SaveChanges();
        }
        
        db4.Propostas.RemoveRange(db4.Propostas.Where(u => u.IdCliente == IdUtilizador));
        db4.SaveChanges();
        
        
        var db10 = new MyDbContext();
        var result = new Utilizador() { IdUtilizador = IdUtilizador };
        db10.Utilizadors.Attach(result);
        db10.Utilizadors.Remove(result);
        db10.SaveChanges();
        return RedirectToAction("Index");
    }*/
}