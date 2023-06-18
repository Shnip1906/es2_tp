using Backend.Models;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers.api;

public class client : ControllerBase
{
    private readonly MyDbContext _context;

    public client()
    {
        _context = new MyDbContext();
    }

    [HttpGet]
    public async Task<IActionResult> ListarClientes()
    {
        var utilizadores = await _context.Utilizadors
            .Where(u => u.TipoUtilizador == 4)
            .OrderBy(u => u.NomeUtilizador)
            .ToListAsync();

        return Ok(utilizadores);
    }
    
    [HttpPost]
    public IActionResult RegistarCli([FromBody] UsersModel model)
    {
        var exist = _context.Utilizadors.FirstOrDefault(p => p.Username == model.username);
        if (exist != null)
        {
            return BadRequest("O username já existe no sistema!!!");
        }
    
        var user = new Utilizador()
        {
            Username = model.username,
            Password = model.password,
            TipoUtilizador = 4,
            NomeUtilizador = model.nomeUtilizador
        };
    
        _context.Utilizadors.Add(user);
        _context.SaveChanges();
    
        var client = _context.Utilizadors.FirstOrDefault(p => p.Username == model.username);
    
        var cliente = new Cliente
        {
            IdUtilizadorCliente = client.IdUtilizador,
            IdUtilizador = SharedVariables.SharedVariables.userID
        };
    
        _context.Clientes.Add(cliente);
        _context.SaveChanges();

        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> VerDadosEditCli([FromRoute] Guid id)
    {
        if (id.Equals(null))
        {
            return BadRequest("Invalid ID"); 
        }

        var result = _context.Utilizadors.FirstOrDefault(b => b.IdUtilizador == id);
        if (result == null)
        {
            return NotFound("Entity not found"); // Return a 404 Not Found response if the entity is not found
        }

        var model = new
        {
            id = result.IdUtilizador,
            nome = result.NomeUtilizador,
            username = result.Username,
            password = result.Password
        };

        return Ok(model); // Return the entity in the response body
    }
    
    [HttpPost]
    public IActionResult EditarCliente([FromBody] UsersModel usersModel)
    {
        if (usersModel == null)
        {
            return BadRequest("Invalid model");
        }

        var db = new MyDbContext();
        var result = db.Utilizadors.SingleOrDefault(b => b.IdUtilizador == usersModel.idUtilizador);

        if (result == null)
        {
            return NotFound("Entity not found"); // Return a 404 Not Found response if the entity is not found
        }

        result.NomeUtilizador = usersModel.nomeUtilizador;
        result.Username = usersModel.username;
        db.SaveChanges();

        return Ok(); 
    }

    [HttpDelete]
    public IActionResult EliminarCliente(Guid id)
    {
        var db = new MyDbContext();
        var db2 = new MyDbContext();
        var db3 = new MyDbContext();
        var db4 = new MyDbContext();
        var db5 = new MyDbContext();
        var db6 = new MyDbContext();
    
        db2.Clientes.RemoveRange(db2.Clientes.Where(u => u.IdCliente == id));
        db2.SaveChanges();

        var props = db6.Propostas.Where(u => u.IdCliente == id).ToList();
        foreach (var prop in props)
        {
            db5.Skillsproposta.RemoveRange(db5.Skillsproposta.Where(u => u.IdPropostas == prop.IdPropostas));
            db5.SaveChanges();
        }
    
        db4.Propostas.RemoveRange(db4.Propostas.Where(u => u.IdCliente == id));
        db4.SaveChanges();
    
        var result = new Utilizador() { IdUtilizador = id };
        db.Utilizadors.Attach(result);
        db.Utilizadors.Remove(result);
        db.SaveChanges();
    
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> ListarPropostas(Guid id)
    {
        var myDbContext = _context.Propostas.Where(u => u.IdCliente == id);
        var propostas = await myDbContext.OrderBy(u => u.NomePropostas).ToListAsync();
        
        return Ok(propostas);
    }

    [HttpPost]
    public IActionResult RegistarProposta([FromBody] PropostaModel model)
    {
        var proposta = new Proposta()
        {
            NomePropostas = model.NomePropostas,
            Ntotalhoras = model.Ntotalhoras,
            Descricao = model.Descricao,
            IdAreaProfissional = model.IdAreaProfissional,
            IdCliente = model.IdCliente,
            IdUtilizador = model.IdUtilizador
        };
    
        _context.Propostas.Add(proposta);
        _context.SaveChanges();
        
        return Ok();
    }
    
    [HttpDelete]
    public IActionResult EliminarProposta(Guid id)
    {
        var db6 = new MyDbContext();
        var db5 = new MyDbContext();
        var props = db6.Propostas.Where(u => u.IdCliente == id).ToList();
        foreach (var prop in props)
        {
            db5.Skillsproposta.RemoveRange(db5.Skillsproposta.Where(u => u.IdPropostas == prop.IdPropostas));
            db5.SaveChanges();
        }
        
        var db = new MyDbContext();
        var result = new Proposta { IdPropostas = id };
        db.Propostas.Attach(result);
        db.Propostas.Remove(result);
        db.SaveChanges();
        
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> ListarSkillsProposta(Guid id)
    {
        var db6 = new MyDbContext();
        var db5 = new MyDbContext();
    
        var props = db6.Skillsproposta.Where(u => u.IdPropostas == id).ToList();
    
        List<SkillsPropostaModel> skFinal = new List<SkillsPropostaModel>();
    
        var myDbContext = _context.Skills;
        var data = await myDbContext.Where(m => props.Select(p => p.IdSkills).Contains(m.IdSkills)).OrderBy(m => m.IdSkills).ToListAsync();
    
        foreach (var prop in props)
        {
            foreach (var sk in data)
            {
                if (sk.IdSkills == prop.IdSkills)
                {
                    SkillsPropostaModel sk1 = new SkillsPropostaModel
                    {
                        IdPropostas = prop.IdPropostas,
                        IdSkills = prop.IdSkills,
                        IdSkillsProposta = prop.IdSkillsProposta,
                        nomeSkill = sk.NomeSkills
                    };
       
                    skFinal.Add(sk1);   
                }
            }
        }
    
        return Ok(skFinal);
    
    }
    
    [HttpGet]
    public async Task<IActionResult> ListarSkill(Guid id)
    {
        var myDbContext = _context.Skills;
        var skill = await myDbContext.OrderBy(u => u.NomeSkills).ToListAsync();
        return Ok(skill);
    }
    
    [HttpPost]
    public IActionResult RegistarAdicionarSkills([FromBody] SkillsPropostaModel model)
    {
        var skillprop = new Skillspropostum()
        {
            IdPropostas = model.IdPropostas,
            IdSkills = model.IdSkills
        };
    
        _context.Skillsproposta.Add(skillprop);
        _context.SaveChanges();
        
        return Ok();
    }
    
    [HttpDelete]
    public IActionResult EliminarSkillProposta(Guid id)
    {
        var db = new MyDbContext();
        var result = new Skillspropostum() { IdSkillsProposta= id };
        db.Skillsproposta.Attach(result);
        db.Skillsproposta.Remove(result);
        db.SaveChanges();
        
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> ListaPerfis(Guid id)
    {
        var propskills = _context.Skillsproposta.Where(p => p.IdPropostas == id).ToList();
        var perfis = _context.Skillprofs;
        List<Skillprof> talents = new List<Skillprof>();
        List<Skillprof> repetidos = new List<Skillprof>();
        
        foreach (var props in propskills)
        {
            var idprop = props.IdSkills.GetValueOrDefault();
            var tal = perfis.Where(p => p.IdSkills == idprop).ToList();
            //var tal = perfis.Where(p => p.IdSkills == idprop).Include(p => p.IdPerfilNavigation).ToList();
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
        
        var myDbContext = _context.Perfils;
        var data = await myDbContext.Where(m => talents.Select(p => p.IdPerfil).Contains(m.IdPerfil)).OrderBy(m => m.IdPerfil).ToListAsync();
        
        List<PerfisModel> perfilFinal = new List<PerfisModel>();
        
        foreach (var t in talents)
        {
            foreach (var sk in data)
            {
                if (sk.IdPerfil == t.IdPerfil)
                {
                    PerfisModel sk1 = new PerfisModel()
                    {
                        IdPerfil = sk.IdPerfil,
                        NomePerfil = sk.NomePerfil,
                        Pais = sk.Pais,
                        Email = sk.Email,
                        Precohora = sk.Precohora
                    };
       
                    perfilFinal.Add(sk1);   
                }
            }
        }
    
        return Ok(perfilFinal);
    }
}