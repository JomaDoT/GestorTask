using GestorTask.Applications.DTO.User;
using GestorTask.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorTask.Application.Mappers;
public static class UserMapper
{
    public static TableInfoUserDTO ToTable(User entity)
            => new TableInfoUserDTO()
            {
                Id = entity.Id,
                UserName = entity.Username,
                Status = entity.Status == "f" ? false : true,
                Token = string.Empty
            };

    public static IEnumerable<TableInfoUserDTO> ToListTable(IEnumerable<User> entity)
        => entity.Select(x => new TableInfoUserDTO()
        {
            Id = x.Id,
            UserName = x.Username,
            Status = x.Status == "f" ? false : true,
            Token = string.Empty
        });

    public static User AddMapper(AddUserDTO dto)
        => new()
        {
            Username = dto.UserName,
            Password = dto.PassWord,
            DateRegister = DateTime.Now,
            Status = "t",
        };
}