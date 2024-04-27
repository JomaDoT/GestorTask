using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorTask.Application.DTO.Task;
public record GetTaskDTO : TaskDTO
{
    public decimal Id { get; set; }
    public DateTime DateRegister { get; set; }

    public decimal RegisterBy { get; set; }

    public bool Status { get; set; }
}
