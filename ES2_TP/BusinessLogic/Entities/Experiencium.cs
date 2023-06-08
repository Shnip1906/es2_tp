using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Experiencium
{
    public Guid IdExperiencia { get; set; }

    public string NomeExperiencia { get; set; } = null!;

    public string NomeEmpresa { get; set; } = null!;

    public int Anoinicio { get; set; }

    public int Anofim { get; set; }

    public bool? Continuo { get; set; }

    public virtual ICollection<Perfil> Perfils { get; set; } = new List<Perfil>();
}
