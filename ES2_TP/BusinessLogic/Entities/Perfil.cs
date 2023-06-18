using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Perfil
{
    public Guid IdPerfil { get; set; }

    public string NomePerfil { get; set; } = null!;

    public string Pais { get; set; } = null!;

    public string Email { get; set; } = null!;

    public double Precohora { get; set; }

    public bool Publico { get; set; }

    public virtual ICollection<Experiencium> Experiencia { get; set; } = new List<Experiencium>();

    public virtual ICollection<Skillprof> Skillprofs { get; set; } = new List<Skillprof>();
}
