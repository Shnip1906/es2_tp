using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Utilizador
{
    public Guid IdUtilizador { get; set; }

    public string NomeUtilizador { get; set; } = null!;

    public Guid? IdTipoUtilizador { get; set; }

    public virtual Tipoutilizador? IdTipoUtilizadorNavigation { get; set; }
}
