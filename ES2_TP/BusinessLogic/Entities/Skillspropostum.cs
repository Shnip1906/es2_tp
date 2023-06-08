using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Skillspropostum
{
    public Guid IdSkillsProposta { get; set; }

    public int NMinimoHorasSkill { get; set; }

    public Guid? IdSkills { get; set; }

    public Guid? IdPropostas { get; set; }

    public virtual Proposta? IdPropostasNavigation { get; set; }

    public virtual Skill? IdSkillsNavigation { get; set; }
}
