using GestorTask.Application.Interfaces;
using GestorTask.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GestorTask.Application.Services;
public class TaskServices : ITaskServices
{
    private readonly IDataRepository<Tasks> _repository;

    public TaskServices(IDataRepository<Tasks> dataRepository)
    {
        _repository = dataRepository;
    }
    public async Task InsertAsync(Tasks task)
        => await _repository.AddAsync(task);

    public async Task UpdateAsync(Tasks task)
        => await _repository.UpdateAsync(task);
    public async Task DesactivationAsync(int id)
    {
        var task = await _repository.GetEntityAsync(id);
        task.Status = "f";
        await _repository.UpdateAsync(task);
    }
    public async Task<IEnumerable<Tasks>> GetAllAsync(Expression<Func<Tasks, bool>> predicate)
        => await _repository.GetAllAsync(x => x, predicate, null);

    public async Task<Tasks> GetAsync(Expression<Func<Tasks, bool>> predicate)
        => await _repository.GetAsync(x => x, predicate, null);

    public async Task<bool> ExistsAsync(Expression<Func<Tasks, bool>> predicate)
        => await _repository.ExistsAsync(x => x, predicate);


    public async Task DeleteAsync(Tasks task)
    => await _repository.RemoveAsync(task);

}