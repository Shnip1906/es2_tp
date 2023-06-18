using System.ComponentModel.DataAnnotations;
namespace Backend.Models;

public class ClientModel
{
    public Guid IdCliente { get; set; }

    public Guid? IdUtilizador { get; set; }

    public Guid? IdUtilizadorCliente { get; set; }
}