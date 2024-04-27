using GestorTask.Application.Interfaces;
using GestorTask.Applications.Helpers;
using GestorTask.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GestorTask.Application.Services;

public class UserServices : IUserServices
{
    private readonly IDataRepository<User> _repository;

    public UserServices(IDataRepository<User> repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>> predicate)
    => await _repository.GetAllAsync(x => x, predicate, null, null);
    public async Task<User> GetAsync(Expression<Func<User, bool>> predicate)
        => await _repository.GetAsync(x => x, predicate, null, null);
}