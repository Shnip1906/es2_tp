using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class PropostaModel
{
    public Guid IdPropostas { get; set; }

    public string NomePropostas { get; set; } = null!;

    public int Ntotalhoras { get; set; }

    public string Descricao { get; set; } = null!;

    public Guid? IdAreaProfissional { get; set; }

    public Guid? IdCliente { get; set; }

    public Guid? IdUtilizador { get; set; }
    
}