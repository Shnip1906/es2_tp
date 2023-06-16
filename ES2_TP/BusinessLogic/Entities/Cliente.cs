using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Cliente
{
    public Guid IdCliente { get; set; }

    public Guid? IdUtilizador { get; set; }

    public Guid? IdUtilizadorCliente { get; set; }

    public virtual Utilizador? IdUtilizadorClienteNavigation { get; set; }

    public virtual Utilizador? IdUtilizadorNavigation { get; set; }
}
