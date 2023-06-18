using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class PerfisModel
{
    public Guid IdPerfil { get; set; }

    public string NomePerfil { get; set; } = null!;

    public string Pais { get; set; } = null!;

    public string Email { get; set; } = null!;

    public double Precohora { get; set; }

    public bool Publico { get; set; }
    
}