using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class UsersModel
{
    public Guid idUtilizador { get; set; }
    public string nomeUtilizador { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public  int tipoUtilizador { get; set; }
    
}