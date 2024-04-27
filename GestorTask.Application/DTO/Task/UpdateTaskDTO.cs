using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorTask.Application.DTO.Task;
public record UpdateTaskDTO : TaskDTO
{
    public decimal Id { get; set; }
    public bool Status { get; set; }
    public decimal ModifyBy { get; set; }
}
