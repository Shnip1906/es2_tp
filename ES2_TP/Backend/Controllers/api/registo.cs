using Backend.Models;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.api;

public class registo: ControllerBase
{
    //  [HttpGet] //
    private readonly MyDbContext _context;

    public registo()
    {
        _context = new MyDbContext();
    }

    [HttpPost]
    public IActionResult RegistarUtilizador([FromBody] UsersModel model)
    {
        if (model == null)
        {
            return BadRequest("Invalid model");
        }
        
        var exist = _context.Utilizadors.FirstOrDefault(p => p.Username == model.username);
        if (exist != null)
        {
            return BadRequest("O username já existe no sistema!!!");
        }
        
        var utilizador = new Utilizador()
        {
            NomeUtilizador = model.nomeUtilizador,
            Username = model.username,
            Password = model.password,
            TipoUtilizador = model.tipoUtilizador
        };
    
        _context.Utilizadors.Add(utilizador);
        _context.SaveChanges();
        
        return Ok();
    }
    
}