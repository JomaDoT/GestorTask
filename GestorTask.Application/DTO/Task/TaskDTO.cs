using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorTask.Application.DTO.Task;
public record TaskDTO
{
    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime? DateInit { get; set; }

    public DateTime? DateEnd { get; set; }

}
