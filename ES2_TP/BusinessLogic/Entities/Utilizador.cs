using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Utilizador
{
    public Guid IdUtilizador { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string NomeUtilizador { get; set; } = null!;

    public int TipoUtilizador { get; set; }

    public virtual ICollection<Cliente> ClienteIdUtilizadorClienteNavigations { get; set; } = new List<Cliente>();

    public virtual ICollection<Cliente> ClienteIdUtilizadorNavigations { get; set; } = new List<Cliente>();

    public virtual ICollection<Proposta> PropostaIdClienteNavigations { get; set; } = new List<Proposta>();

    public virtual ICollection<Proposta> PropostaIdUtilizadorNavigations { get; set; } = new List<Proposta>();
}
