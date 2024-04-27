using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorTask.Application.DTO.Task;
public record class AddTaskDTO : TaskDTO
{
    public decimal RegisterBy { get; set; }
}
