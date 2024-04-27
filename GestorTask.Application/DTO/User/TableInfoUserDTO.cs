﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorTask.Applications.DTO.User;

public record TableInfoUserDTO
{
    public decimal Id { get; set; }
    public string UserName { get; set; }
    public bool Status { get; set; }
    public string Token { get; set; }

}

