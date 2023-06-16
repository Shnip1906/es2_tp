using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

public class ClienteController : Controller
{
    private readonly MyDbContext _context;

    public ClienteController()
    {
        _context = new MyDbContext();
    }
    
    public async Task<IActionResult> ListarClientesUtilizadores(Guid id)
    {
        var myDbContext = _context.Utilizadors.Where(f => f.TipoUtilizador == 4);
        return View(await myDbContext.OrderBy(u => u.NomeUtilizador).ToListAsync());
    }
    
    public IActionResult RegistarCliente()
    {
        return View();
    }
    
    public IActionResult Registar([FromForm] string NomeUtilizador, [FromForm] string Username, [FromForm] string Password)
    {
        var db5 = new MyDbContext();
        var exist = db5.Utilizadors.Where(p => p.Username == Username).FirstOrDefault();
        if (exist != null)
        {
            ViewBag.erro = "O username já existe no sistema!!!";
            return View("Erro");
        }
        
        var db = new MyDbContext();
        var db3 = new MyDbContext();
        Utilizador user2 = new Utilizador();
        user2.Username = Username;
        user2.Password = Password;
        user2.TipoUtilizador = 4;
        user2.NomeUtilizador = NomeUtilizador;
        db.Utilizadors.Add(user2);
        db.SaveChanges();
        
        var client = db3.Utilizadors.Where(p => p.Username == Username).SingleOrDefault();

        var db2 = new MyDbContext();
        Cliente cliente = new Cliente();
        cliente.IdUtilizadorCliente = client.IdUtilizador;
        cliente.IdUtilizador = SharedVariables.SharedVariables.userID;
        db2.Clientes.Add(cliente);
        db2.SaveChanges();
        return RedirectToAction("ListarClientesUtilizadores");
    }

    public IActionResult EditarCliente(Guid Id)
    {
        var db = new MyDbContext();

        var result = db.Utilizadors.SingleOrDefault(b => b.IdUtilizador == Id);
        ViewBag.Id = result.IdUtilizador;
        ViewBag.Nome = result.NomeUtilizador;
        ViewBag.Username = result.Username;
        ViewBag.Password = result.Password;

        return View();
    }
    
    public async Task<IActionResult> RegistarEditarClientes([FromForm] Guid Id, [FromForm] string NomeUtilizador,
        [FromForm] string Username)
    {
        var db = new MyDbContext();
        var result = db.Utilizadors.SingleOrDefault(b => b.IdUtilizador == Id);
        result.NomeUtilizador = NomeUtilizador;
        result.Username = Username;
        db.SaveChanges();
        return RedirectToAction("ListarClientesUtilizadores");
    }
    
    public async Task<IActionResult> EliminarCliente(Guid Id)
    {
        var db = new MyDbContext();
        var db2 = new MyDbContext();
        var db3 = new MyDbContext();
        var db4 = new MyDbContext();
        var db5 = new MyDbContext();
        var db6 = new MyDbContext();
        
        db2.Clientes.RemoveRange(db2.Clientes.Where(u => u.IdCliente == Id));
        db2.SaveChanges();

        var props = db6.Propostas.Where(u => u.IdCliente == Id).ToList();
        foreach (var prop in props)
        {
            db5.Skillsproposta.RemoveRange(db5.Skillsproposta.Where(u => u.IdPropostas == prop.IdPropostas));
            db5.SaveChanges();
        }
        
        db4.Propostas.RemoveRange(db4.Propostas.Where(u => u.IdCliente == Id));
        db4.SaveChanges();
        
        var result = new Utilizador() { IdUtilizador = Id };
        db.Utilizadors.Attach(result);
        db.Utilizadors.Remove(result);
        db.SaveChanges();
        
        return RedirectToAction("ListarClientesUtilizadores");
    }
    
    public async Task<IActionResult> ListarPropostas(Guid id)
    {
        var myDbContext = _context.Propostas.Where(u => u.IdCliente == id);

        ViewBag.Id = id;
        return View("ListarPropostas", await myDbContext.OrderBy(u => u.NomePropostas).ToListAsync());
    }
    
    public async Task<IActionResult> AdicionarProp(Guid Id)
    {
        var item = new List<SelectListItem>();

        foreach ( Areaprofissional areaProfissional in _context.Areaprofissionals )
        {
            item.Add(new SelectListItem(text: areaProfissional.NomeAreaPrfossional, value:areaProfissional.IdAreaProfissional.ToString()));
        }

        ViewData["IdAreaProfissional"] = new SelectList(_context.Areaprofissionals, "IdAreaProfissional", "NomeAreaPrfossional");
        ViewBag.Id = Id;
        return View();
    }
    
    public async Task<IActionResult> RegistarProposta([FromForm] string NomePropostas, [FromForm] Guid IdAreaProfissional, [FromForm] int Ntotalhoras, [FromForm] string Descricao, [FromForm] Guid Id)
    {
        var db = new MyDbContext();

        /*Console.WriteLine(NomePropostas);
        Console.WriteLine(IdAreaProfissional);
        Console.WriteLine(Ntotalhoras);
        Console.WriteLine(Descricao);
        Console.WriteLine(Id);*/
        
        Proposta ct = new Proposta();
        ct.NomePropostas = NomePropostas;
        ct.Ntotalhoras = Ntotalhoras;
        ct.Descricao = Descricao;
        ct.IdAreaProfissional = IdAreaProfissional;
        ct.IdCliente = Id;
        ct.IdUtilizador = SharedVariables.SharedVariables.userID;
        db.Propostas.Add(ct);
        db.SaveChanges();

        return RedirectToAction("ListarPropostas", new RouteValueDictionary { { "id", Id } });
    }
    
    public async Task<IActionResult> EliminarProposta(Guid id)
    {
        var db = new MyDbContext();
        var result = new Proposta { IdPropostas = id };
        db.Propostas.Attach(result);
        db.Propostas.Remove(result);
        db.SaveChanges();
        return RedirectToAction("ListarClientesUtilizadores");
    }
    
    public async Task<IActionResult> ListarSkills(Guid id)
    {
        ViewBag.Id = id;
        
        var myDbContext = _context.Skillsproposta.Where(u => u.IdPropostas == id).Include(u => u.IdSkillsNavigation);;
        return View("ListarSkills", await myDbContext.OrderBy(u => u.IdSkills).ToListAsync());
    }
    
    public async Task<IActionResult> AdicionarSkills(Guid id)
    {
        ViewBag.Id = id;
        var myDbContext = _context.Skills;
        return View("AdicionarSkill", await myDbContext.OrderBy(u => u.NomeSkills).ToListAsync());
    }
    
    public async Task<IActionResult> RegistarAdicionarSkills(Guid idSkill, Guid idProposta)
    {
        var db = new MyDbContext();
        Skillspropostum ct = new Skillspropostum();
        ct.IdPropostas = idProposta;
        ct.IdSkills = idSkill;
        db.Add(ct);
        db.SaveChanges();
        return RedirectToAction("ListarSkills",new RouteValueDictionary{{"id", idProposta}});
    }
    
    public async Task<IActionResult> EliminarSkillProposta(Guid id)
    {
        
        var db1 = new MyDbContext();
        var sk = db1.Skillsproposta.Where(p => p.IdSkillsProposta == id).FirstOrDefault();
        
        var db = new MyDbContext();
        var result = new Skillspropostum() { IdSkillsProposta= id };
        db.Skillsproposta.Attach(result);
        db.Skillsproposta.Remove(result);
        db.SaveChanges();
        return RedirectToAction("ListarSkills", new RouteValueDictionary{{"id", sk.IdPropostas}});
    }
    
    public async Task<IActionResult> ListaPerfis(Guid id)
    {
        var db = new MyDbContext();
        //var teste = db.Proposta.Where(p => p.IdProposta == id).SingleOrDefault();
        var propskills = db.Skillsproposta.Where(p => p.IdPropostas == id).ToList();
        var perfis = db.Skillprofs;
        List<Skillprof> talents = new List<Skillprof>();
        List<Skillprof> repetidos = new List<Skillprof>();
        
        foreach (var props in propskills)
        {
            var idprop = props.IdSkills.GetValueOrDefault();
            var tal = perfis.Where(p => p.IdSkills == idprop).Include(p => p.IdPerfilNavigation).ToList();
            foreach (var t in tal)
            {
                bool repete = false;
                foreach (var x in repetidos)
                {
                    if (x.IdPerfil == t.IdPerfil)
                    {
                        repete = true;
                    }  
                }

                if (!repete)
                {
                    talents.Add(t);
                    repetidos.Add(t);
                }
            }
        }
        
        //ViewBag.teste = talentos;
        //return View("teste");
        return View("ListPerfilSkill",talents);
    }
}