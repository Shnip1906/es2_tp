using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class SkillsPropostaModel
{
    public Guid IdSkillsProposta { get; set; }

    public int NMinimoHorasSkill { get; set; }
    
    public string nomeSkill { get; set; }

    public Guid? IdSkills { get; set; }

    public Guid? IdPropostas { get; set; }
    
}