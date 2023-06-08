using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Areaprofissional
{
    public Guid IdAreaProfissional { get; set; }

    public string NomeAreaPrfossional { get; set; } = null!;

    public virtual ICollection<Proposta> Proposta { get; set; } = new List<Proposta>();

    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
}
