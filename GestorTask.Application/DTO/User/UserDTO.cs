using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorTask.Applications.DTO.User;

public record UserDTO
{
    public string UserName { get; set; }
    public string Salt { get; set; }
    public string PassWord { get; set; }
}
