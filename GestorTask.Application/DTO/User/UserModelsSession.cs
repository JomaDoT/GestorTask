using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorTask.Applications.DTO.User;

public record UserModelsSession
{
    public string Id { get; set; }
    public string NameUser { get; set; }
}
