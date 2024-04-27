using System;
using System.Collections.Generic;

namespace GestorTask.Infraestructure.Models;

public partial class User
{
    public decimal Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public DateTime DateRegister { get; set; }

    public string Status { get; set; } = null!;
}
