using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Skillprof
{
    public Guid IdSkillsprof { get; set; }

    public int Nhoras { get; set; }

    public Guid? IdPerfil { get; set; }

    public Guid? IdSkills { get; set; }

    public virtual Perfil? IdPerfilNavigation { get; set; }

    public virtual Skill? IdSkillsNavigation { get; set; }
}
