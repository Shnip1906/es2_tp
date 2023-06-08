using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class UsersModel
{
    public UsersModel(BusinessLogic.Entities.Utilizador user)
    {
        this.Nome = user.NomeUtilizador;
        this.username = user.Username;
        this.Password = user.Password;
        this.tipo = (int)user.Tipo;
    }

    [EmailAddress]
    public string Nome { get; set; }
    public string username { get; set; }
    [MinLength(6)]
    public string Password { get; set; }
    
    [MinLength(6)]
    public  int tipo { get; set; }
    
}