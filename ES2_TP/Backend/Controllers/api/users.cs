using Backend.Models;
using BusinessLogic.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers.api;

public class users : ControllerBase
{
    private readonly MyDbContext _context;

    public users()
    {
        _context = new MyDbContext();
    }

    [HttpGet]
    public async Task<IActionResult> ListarUtilizadores()
    {
        var utilizadores = await _context.Utilizadors
            .Where(u => u.TipoUtilizador == 3)
            .OrderBy(u => u.NomeUtilizador)
            .ToListAsync();

        return Ok(utilizadores);
    }
    
    [HttpGet]
    public async Task<IActionResult> ListarUtilizadorEspecifico(Guid? id)
    {
        var myDbContext = _context.Utilizadors.Where(f => f.IdUtilizador == id);
        var utilizadores = await myDbContext.OrderBy(u => u.NomeUtilizador).ToListAsync();

        return Ok(utilizadores);
    }

    
    [HttpGet]
    public async Task<IActionResult> ListarTodosUtilizadores()
    {
        var utilizadores = await _context.Utilizadors
            .OrderBy(u => u.NomeUtilizador)
            .ToListAsync();

        return Ok(utilizadores);
    }
    
    [HttpGet]
    public async Task<IActionResult> VerDadosEditUtilizador([FromRoute] Guid id)
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

        UsersModel model = new UsersModel()
        {
            idUtilizador = result.IdUtilizador,
            nomeUtilizador = result.NomeUtilizador,
            username = result.Username,
            password = result.Password,
            tipoUtilizador = result.TipoUtilizador
        };

        return Ok(model);
    }
    
    [HttpPost]
    public IActionResult EditarUtilizador([FromBody] UsersModel userModel)
    {
        if (userModel == null)
        {
            return BadRequest("Invalid model");
        }

        var db = new MyDbContext();
        var result = db.Utilizadors.SingleOrDefault(b => b.IdUtilizador == userModel.idUtilizador);

        if (result == null)
        {
            return NotFound("Entity not found"); // Return a 404 Not Found response if the entity is not found
        }

        result.NomeUtilizador = userModel.nomeUtilizador;
        result.Username = userModel.username;
        result.Password = userModel.password;

        db.SaveChanges();

        return Ok(); 
    }
}