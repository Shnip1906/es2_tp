using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

public class UtilizadorController : Controller
{
    
    private readonly MyDbContext _context;

    public UtilizadorController()
    {
        
        _context = new MyDbContext();
    }

    public async Task<IActionResult> IndexUser( Guid id)
    {
        ViewBag.Id = id;
        var myDbContext = _context.Utilizadors.Where(f => f.IdUtilizador == id);
        return View(await myDbContext.OrderBy(u => u.NomeUtilizador).ToListAsync());
       
    }
    
    /*public async Task<IActionResult> Cliente3(int id)
    {
        id = Global.counter;
        ViewBag.Id = id;
        var myDbContext = _context.Clientes.Where(f => f.IdUser == id ).Include(p => p.IdClienteNavigation);
       
        
        return View(await myDbContext.OrderBy(u => u.IdClienteNavigation.Nome).ToListAsync());
    }*/

    /*public async Task<IActionResult> ListarSkillTalento(int id)
    {
        ViewBag.Id = id;
        var myDbContext = _context.TalentSkills.Where(t => t.FkIdTalento == id).Include(t => t.FkIdSkillNavigation);
        // ViewBag.id = id;
        return View(await myDbContext.OrderBy(u => u.Anos == id).ToListAsync());
    }
    
   

    public async Task<IActionResult> ListarTalentos2(int id)
    {
        ViewBag.Id = id;
        var myDbContext = _context.Talentos;
        return View(await myDbContext.OrderBy(u => u.Nome).ToListAsync());
    }

    public async Task<IActionResult> Proposta(int id)
    {  
        ViewBag.Id = id;
        var myDbContext = _context.Proposta.Where(u => u.IdUser == id).Include(u => u.IdAreaProNavigation);
     
        return View("ListarPropostas", await myDbContext.OrderBy(u => u.Nome).ToListAsync());
    }

    public IActionResult RegistarTalento([FromForm] string nome, [FromForm] string pais, [FromForm] string email,
        [FromForm] double precohora)
    {
        //User user = new User();

        var db = new MyDbContext();
        Talento talento = new Talento();
        talento.Nome = nome;
        talento.Email = email;
        talento.Pais = pais;
        //ViewBag.teste = precohora;
        //return View("teste");
        talento.PrecoHora = precohora;
        db.Talentos.Add(talento);
        db.SaveChanges();
        return RedirectToAction("ListarTalentos2");
    }

    public IActionResult CriarTalento()
    {
        return  View();
    }

    public IActionResult Registar([FromForm] string nome, [FromForm] string username, [FromForm] string password)
    {
        //User user = new User();
        var db5 = new MyDbContext();
        var exist = db5.Users.Where(p => p.Username == username).FirstOrDefault();
        if (exist != null)
        {
            ViewBag.teste = "O username já existe no sistema!!!";
            return View("teste");
        }
        var iduser = Global.counter;
        var db3 = new MyDbContext();
        var db = new MyDbContext();
        User user2 = new User();
        user2.Username = username;
        user2.Password = password;
        user2.Tipo = 4;
        user2.Nome = nome;
        db.Users.Add(user2);
        db.SaveChanges();
        var client = db3.Users.Where(p => p.Username == username).SingleOrDefault();

        var db2 = new MyDbContext();
        Cliente cli = new Cliente();
        cli.IdCliente = client.IdUsers;
        cli.IdUser = iduser;
        db2.Clientes.Add(cli);
        db2.SaveChanges();
        return RedirectToAction("Cliente3");
    }

    public IActionResult Create()
    {
        return View();
    }

    public IActionResult CriarSkillTalento()
    {
        var item = new List<SelectListItem>();

        foreach ( Skil skil in _context.Skils )
        {
            item.Add(new SelectListItem(text: skil.Nome, value:skil.IdSki.ToString()));
        }

        ViewData["IdSki"] = new SelectList(_context.Skils, "IdSki", "Nome");
        
        var item2 = new List<SelectListItem>();

        foreach ( Talento talento in _context.Talentos )
        {
            item.Add(new SelectListItem(text: talento.Nome, value:talento.Id.ToString()));
        }

        ViewData["Id"] = new SelectList(_context.Talentos, "Id", "Nome");
        
        return View();
    }
    
    public IActionResult CriaSKT([FromForm] int Anos, [FromForm] int fkIdSkill,[FromForm] int fkIdTalento)
    {

        var db = new MyDbContext();
        
        TalentSkill talentSkill2 = new TalentSkill();
        talentSkill2.Anos = Anos;
        talentSkill2.FkIdSkill= fkIdSkill;
        talentSkill2.FkIdTalento = fkIdTalento;
        db.TalentSkills.Add(talentSkill2);
        db.SaveChanges();
        
        return RedirectToAction("ListarTalentos2");
    }
    public async Task<IActionResult> ListarTalentos(int id)
    {
        var myDbContext = _context.TalentClientes.Where(f => f.IdCliente == id).Include(u => u.IdTalentoNavigation);
        @ViewBag.idCliente = id;
        return View(await myDbContext.OrderBy(u => u.IdCliente).ToListAsync());
    }
    public async Task<IActionResult> AdicionarTalento(int id)
    {
        var myDbContext = _context.Talentos;

        ViewBag.cliente = id;
        return View("ListarTodosTalentos", await myDbContext.OrderBy(u => u.Nome).ToListAsync());
    }
    public async Task<IActionResult> EliminarTalentoC(int id)
    {
        var db = new MyDbContext();
        var result = new TalentCliente() { IdTalentClientes = id };
        db.TalentClientes.Attach(result);
        db.TalentClientes.Remove(result);
        db.SaveChanges();
        return RedirectToAction("Cliente3");
    }
    public async Task<IActionResult> AdicionarTalentoC(int id, int cliente)
    {
        var db = new MyDbContext();
        TalentCliente ct = new TalentCliente();
        //idTalento = 14;
        //id = 7;
        ct.IdCliente = cliente;
        ct.IdTalento = id;
        db.TalentClientes.Add(ct);
        db.SaveChanges();
        return RedirectToAction("Cliente3");
    }
    public async Task<IActionResult> Propostas(int id)
    {
        var myDbContext = _context.Proposta.Where(u => u.IdCliente == id).Include(u => u.IdAreaProNavigation);

        ViewBag.Id = id;
        return View("ListarPropostas", await myDbContext.OrderBy(u => u.Nome).ToListAsync());
    }
    public IActionResult EditarCliente(int id)
    {
        var db = new MyDbContext();
        var u = new Talento { Id = id };

        var result = db.Users.SingleOrDefault(b => b.IdUsers == id);
        ViewBag.Id = result.IdUsers;
        ViewBag.Nome = result.Nome;
        ViewBag.Username = result.Username;
        ViewBag.Password = result.Password;

        return View();
    }
    public async Task<IActionResult> EditarClientes([FromForm] int id, [FromForm] string nome,
        [FromForm] string username, [FromForm] string password)
    {
        var db = new MyDbContext();
        var result = db.Users.SingleOrDefault(b => b.IdUsers == id);
        //ViewBag.teste = result.Nome;
        //return View("teste");
        result.Nome = nome;
        result.Username = username;
        result.Password = password;
        db.SaveChanges();
        return RedirectToAction("Cliente3");
    }
    public async Task<IActionResult> DeleteCliente(int id)
    {
        var db = new MyDbContext();
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
        
        var result = new User() { IdUsers = id };
        db.Users.Attach(result);
        db.Users.Remove(result);
        db.SaveChanges();
        return RedirectToAction("Cliente3");
    }
    public async Task<IActionResult> LTP(int id)
    {
        var db = new MyDbContext();
        //var teste = db.Proposta.Where(p => p.IdProposta == id).SingleOrDefault();
        var propskills = db.PropSkills.Where(p => p.IdProp == id).ToList();
        var talentos = db.TalentSkills;
        List<TalentSkill> talents = new List<TalentSkill>();
        foreach (var props in propskills)
        {
            var idprop = props.IdSkill.GetValueOrDefault();
            var tal = talentos.Where(p => p.FkIdSkill == idprop).Include(p => p.FkIdTalentoNavigation).ToList();
            foreach (var t in tal)
            {
                talents.Add(t);
            }
        }
        
        //ViewBag.teste = talentos;
        //return View("teste");
        return View("ListTalentSkill",talents);
    }
    public async Task<IActionResult> Skills(int id)
    {
        ViewBag.Id = id;
        
        var myDbContext = _context.PropSkills.Where(u => u.IdProp == id).Include(u => u.IdSkillNavigation);;
        return View("ListarSkills", await myDbContext.OrderBy(u => u.Id).ToListAsync());
    }
    public async Task<IActionResult> EliminarProp(int id)
    {
        var db = new MyDbContext();
        var result = new Propostum { IdProposta = id };
        db.Proposta.Attach(result);
        db.Proposta.Remove(result);
        db.SaveChanges();
        return RedirectToAction("Cliente3");
    }
    public async Task<IActionResult> ListSkills(int id)
    {
        ViewBag.Id = id;
        var myDbContext = _context.Skils;
        return View("ListSkill", await myDbContext.OrderBy(u => u.Nome).ToListAsync());
    }
    public async Task<IActionResult> EliminarSP(int id)
    {
        var db = new MyDbContext();
        var result = new PropSkill() { Id= id };
        db.PropSkills.Attach(result);
        db.PropSkills.Remove(result);
        db.SaveChanges();
        return RedirectToAction("Cliente3");
    }
    public async Task<IActionResult> AdicionarSkills(int id, int prop)
    {
        var db = new MyDbContext();
        PropSkill ct = new PropSkill();
        ct.IdProp = prop;
        ct.IdSkill = id;
        db.Add(ct);
        db.SaveChanges();
        return RedirectToAction("Cliente3");
    }
    
    public async Task<IActionResult> AdicionarProp(int id)
    {
        var item = new List<SelectListItem>();

        foreach ( AreaProfissional areaProfissional in _context.AreaProfissionals )
        {
            item.Add(new SelectListItem(text: areaProfissional.Nome, value:areaProfissional.IdProf.ToString()));
        }

        ViewData["IdProf"] = new SelectList(_context.AreaProfissionals, "IdProf", "Nome");
        ViewBag.Id = id;
        return View();
   
   
    }
    public async Task<IActionResult> AddProp([FromForm] string nome, [FromForm] int idAreaPro, [FromForm] int horas, [FromForm] int id)
    {
        var db = new MyDbContext();
        
        var user = db.Clientes.Where(u => u.IdCliente == id).FirstOrDefault();
        
        Propostum ct = new Propostum();
        ct.Nome = nome;
        ct.Horas = horas;
        ct.IdAreaPro = idAreaPro;
        ct.IdCliente = id;
        ct.IdUser = Global.counter;
        db.Proposta.Add(ct);
        db.SaveChanges();

        return RedirectToAction("Cliente3");
    }
    */
    
}