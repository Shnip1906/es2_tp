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
}
