using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Skill
{
    public Guid IdSkills { get; set; }

    public string NomeSkills { get; set; } = null!;

    public Guid IdAreaProfissional { get; set; }

    public virtual Areaprofissional? IdAreaProfissionalNavigation { get; set; }

    public virtual ICollection<Skillprof> Skillprofs { get; set; } = new List<Skillprof>();

    public virtual ICollection<Skillspropostum> Skillsproposta { get; set; } = new List<Skillspropostum>();
}
