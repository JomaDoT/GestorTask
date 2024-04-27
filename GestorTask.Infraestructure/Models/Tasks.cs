using System;
using System.Collections.Generic;

namespace GestorTask.Infraestructure.Models;

public partial class Tasks
{
    public decimal Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? DateInit { get; set; }

    public DateTime? DateEnd { get; set; }

    public DateTime DateRegister { get; set; }

    public decimal RegisterBy { get; set; }

    public string Status { get; set; } = null!;
}
