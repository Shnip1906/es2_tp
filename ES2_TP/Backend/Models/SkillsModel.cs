using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class SkillsModel
{
    public Guid IdSkills { get; set; }

    public string NomeSkills { get; set; } = null!;

    public Guid? IdAreaProfissional { get; set; }
    
}