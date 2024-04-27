using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorTask.Applications.DTO.User;
public record ChangePassWordDTO
{
   public string UserName { get; set; }
   public string PasswordBefore { get; set; }
   public string PasswordNew { get; set; }
   public string ConfirmPassword { get; set; }
}
