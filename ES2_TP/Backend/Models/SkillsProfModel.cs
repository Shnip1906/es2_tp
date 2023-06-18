using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class SkillsProfModel
{
    public Guid IdSkillsprof { get; set; }

    public int Nhoras { get; set; }

    public Guid? IdPerfil { get; set; }

    public Guid? IdSkills { get; set; }
    
    public string NomeSkills { get; set; } = null!;
    
}