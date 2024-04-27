using GestorTask.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GestorTask.Application.Interfaces;
public interface IAuthenticationServices
{
    Task<bool> ValidateLogin(Expression<Func<User, bool>> predicate, string username, string password);
    Task<decimal> GetUserId(string username);
    Task RegisterAsync(User user);
    Task UpdatePassWordAsync(User user);

}