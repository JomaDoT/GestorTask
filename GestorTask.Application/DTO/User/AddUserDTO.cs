using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorTask.Applications.DTO.User;
public record AddUserDTO : UserDTO
{
    public string ConfirmPassWord { get; set; }
}
