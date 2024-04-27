using GestorTask.Application.DTO.Task;
using GestorTask.Applications.DTO.User;
using GestorTask.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorTask.Application.Mappers;
public static class TaskMapper
{
    public static GetTaskDTO ToTable(Tasks entity)
            => new GetTaskDTO()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                DateInit = entity.DateInit,
                DateEnd = entity.DateEnd,
                DateRegister = entity.DateRegister,
                Status = entity.Status == "f" ? false : true,
                RegisterBy = entity.RegisterBy
            };

    public static IEnumerable<GetTaskDTO> ToListTable(IEnumerable<Tasks> entity)
        => entity.Select(x => new GetTaskDTO()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            DateInit = x.DateInit,
            DateEnd = x.DateEnd,
            DateRegister = x.DateRegister,
            Status = x.Status == "f" ? false : true,
            RegisterBy = x.RegisterBy

        });

    public static Tasks Add(AddTaskDTO dto)
        => new()
        {
            Name = dto.Name,
            Description = dto.Description,
            DateInit = dto.DateInit,
            DateEnd = dto.DateEnd,
            RegisterBy = dto.RegisterBy,
            Status = "t"
            
        };
    public static Tasks Update(UpdateTaskDTO dto)
    => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        DateInit = dto.DateInit,
        DateEnd = dto.DateEnd,
        Status = dto.Status == true ? "t" : "f"
    };
}
