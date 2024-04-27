using GestorTask.Application.Interfaces;
using GestorTask.Applications.Helpers;
using GestorTask.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GestorTask.Application.Services;

public class AuthenticationServices : IAuthenticationServices
{
    private readonly IDataRepository<User> _repository;
    public AuthenticationServices(IDataRepository<User> repository)
    {
        _repository = repository;
    }
    public async Task<decimal> GetUserId(string username)
    {
        var user = await _repository.GetAsync(x => x, x => x.Username == username);
        return user.Id;
    }

    public async Task<bool> ValidateLogin(Expression<Func<User, bool>> predicate, string username, string password)
    {
        var userrResult = await _repository.GetAsync(x => x, predicate, null);

        if (userrResult is null)
            return false;

        var passworEncrypt = AESCrypt.CreateMD5(password + userrResult.Salt);

        var existCredentialUser = userrResult.Username == username && userrResult.Password.ToLower() == passworEncrypt.ToLower();

        if (!existCredentialUser)
            return false;

        return true;
    }

    public async Task RegisterAsync(User user)
    {
        user.Salt = AESCrypt.GenerarSalt(25);
        user.Password = AESCrypt.CreateMD5(user.Password + user.Salt);
        await _repository.AddAsync(user);
    }
    public async Task UpdatePassWordAsync(User user)
    {
        user.Password = AESCrypt.CreateMD5(user.Password + user.Salt);
        await _repository.UpdateAsync(user);
    }
}
