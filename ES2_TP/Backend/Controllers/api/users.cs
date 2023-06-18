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
    
}