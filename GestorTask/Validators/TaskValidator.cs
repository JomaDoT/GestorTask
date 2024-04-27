using FluentValidation;
using GestorTask.Applications.DTO.User;
using GestorTask.Applications.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorTask.Application.DTO.Task;

namespace GestorTask.Validators;
public class TaskValidator : AbstractValidator<TaskDTO>
{
    public TaskValidator()
    {

        RuleFor(x => x.Name).CustomNotNullEmptyEqual("string", "Nombre");
        RuleFor(x => x.Description).CustomNotNullEmptyEqual("string", "Descripcion");
        RuleFor(x => x.DateInit).NotEmpty();
        RuleFor(x => x.DateEnd).NotEmpty();
    }
}
public class TaskAddValidator : AbstractValidator<AddTaskDTO>
{
    public TaskAddValidator()
    {
        Include(new TaskValidator());
        RuleFor(x => x.RegisterBy).NotEmpty().GreaterThan(0);
    }
    public class TaskUpdateValidator : AbstractValidator<UpdateTaskDTO>
    {
        public TaskUpdateValidator()
        {
            Include(new TaskValidator());
            RuleFor(x => x.Id).GreaterThan(0);

        }
    }
}