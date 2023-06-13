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
    public IActionResult Registar([FromForm] string nome, [FromForm] string username,[FromForm] string password, [FromForm] int Tipo)
    {
        //User user = new User();
        
        var db2 = new MyDbContext();
        var exist = db2.Utilizadors.Where(p => p.Username == username).FirstOrDefault();
        if (exist != null)
        {
            ViewBag.teste = "O username já existe no sistema!!!";
            return View("teste");
        }
        
        var db = new MyDbContext();
        Utilizador users = new Utilizador();
        users.Username = username;
        users.Password = password;
        users.TipoUtilizador = Tipo;
        users.NomeUtilizador = nome;
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
    public async Task<IActionResult> Edits([FromForm] Guid id,[FromForm] string nome, [FromForm] string username,[FromForm] string password)
    {
        var db = new MyDbContext();
        var result = db.Utilizadors.SingleOrDefault(b => b.IdUtilizador == id);
        //ViewBag.teste = result.Nome;
        //return View("teste");
        result.NomeUtilizador = nome;
        result.Username = username;
        result.Password = password;
        db.SaveChanges();
        return RedirectToAction("Index");
    }
  

    public IActionResult Delete(Guid id)
    {
        /*var db = new MyDbContext();
        var db2 = new MyDbContext();
        var db3 = new MyDbContext();
        var db4 = new MyDbContext();
        var db5 = new MyDbContext();
        var db6 = new MyDbContext();
        db2.Clientes.RemoveRange(db2.Clientes.Where(u => u.IdCliente == id));
        db2.SaveChanges();
        
        db3.TalentClientes.RemoveRange(db3.TalentClientes.Where(u => u.IdCliente == id));
        db3.SaveChanges();

        var props = db6.Proposta.Where(u => u.IdCliente == id).ToList();
        foreach (var prop in props)
        {
            db5.PropSkills.RemoveRange(db5.PropSkills.Where(u => u.IdProp == prop.IdProposta));
            db5.SaveChanges();
        }
        //db5.PropSkills.Remove(db5.PropSkills.Where(p => p.IdProp == ));
        //db5.SaveChanges();
        
        db4.Proposta.RemoveRange(db4.Proposta.Where(u => u.IdCliente == id));
        db4.SaveChanges();
        
        
        var db10 = new MyDbContext();
        var result = new User() { IdUsers = id };
        db10.Users.Attach(result);
        db10.Users.Remove(result);
        db10.SaveChanges();*/
        return RedirectToAction("Index");
    }
}