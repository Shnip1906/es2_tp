using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Proposta
{
    public Guid IdPropostas { get; set; }

    public string NomePropostas { get; set; } = null!;

    public int Ntotalhoras { get; set; }

    public string Descricao { get; set; } = null!;

    public Guid? IdAreaProfissional { get; set; }

    public Guid? IdCliente { get; set; }

    public Guid? IdUtilizador { get; set; }

    public virtual Areaprofissional? IdAreaProfissionalNavigation { get; set; }

    public virtual Utilizador? IdClienteNavigation { get; set; }

    public virtual Utilizador? IdUtilizadorNavigation { get; set; }

    public virtual ICollection<Skillspropostum> Skillsproposta { get; set; } = new List<Skillspropostum>();
}
