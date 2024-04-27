using GestorTask.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GestorTask.Application.Interfaces;
public interface IBaseServices<T>
{
    Task<T> GetAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null);

}
