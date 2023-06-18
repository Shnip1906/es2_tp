using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class ExperienciaModel
{
    public Guid IdExperiencia { get; set; }

    public string NomeExperiencia { get; set; } = null!;

    public string NomeEmpresa { get; set; } = null!;

    public int Anoinicio { get; set; }

    public int Anofim { get; set; }

    public bool? Continuo { get; set; }

    public Guid? IdPerfil { get; set; }
    
}