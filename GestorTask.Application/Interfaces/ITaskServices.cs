using GestorTask.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GestorTask.Application.Interfaces;

public interface ITaskServices: IBaseServices<Tasks>
{
    Task InsertAsync(Tasks entity);
    Task UpdateAsync(Tasks task);
    Task DesactivationAsync(int id);
    Task DeleteAsync(Tasks task);
}
