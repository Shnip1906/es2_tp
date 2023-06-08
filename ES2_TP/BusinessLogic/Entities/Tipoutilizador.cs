using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Tipoutilizador
{
    public Guid IdTipoUtilizador { get; set; }

    public string TipoUtilizador1 { get; set; } = null!;

    public virtual ICollection<Utilizador> Utilizadors { get; set; } = new List<Utilizador>();
}
