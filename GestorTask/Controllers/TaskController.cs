using FluentValidation;
using GestorTask.Application.DTO.Task;
using GestorTask.Application.Interfaces;
using GestorTask.Application.Mappers;
using GestorTask.Application.Services;
using GestorTask.Applications.DTO;
using GestorTask.Infraestructure.Models;
using GestorTask.Utilitys.Responses;
using GestorTask.Validators;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Net;
using System.Threading.Tasks;

namespace GestorTask.Controllers;

[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
public class TaskController : BaseController
{
    private readonly ITaskServices _taskServices;
    private readonly IUserServices _userServices;

    public TaskController(ITaskServices taskServices, IUserServices userServices)
    {
        _taskServices = taskServices;
        _userServices = userServices;
    }
    /// <summary>
    /// Buscar las tareas
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var response = await _taskServices.GetAllAsync();
        if (response is null || !response.Any())
            return NotFound(ErrorHelper<string>.Response("", (int)HttpStatusCode.BadRequest, "No existen tareas."));

        var dto = TaskMapper.ToListTable(response);
        return Ok(dto);
    }
    /// <summary>
    /// Buscar una tarea por el id
    /// </summary>
    /// <param name="id">id de la tarea</param>
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        if (id <= 0)
            return BadRequest(ErrorHelper<int>.Response(id, (int)HttpStatusCode.BadRequest, $"No se permiten valores igual o menores a 0"));

        var predicate = PredicateBuilder.New<Tasks>(x => x.Id == id);
        var response = await _taskServices.GetAsync(predicate);

        if (response is null)
            return NotFound(ErrorHelper<int>.Response(id, (int)HttpStatusCode.BadRequest, $"No existe una tarea con este id {id}"));

        var dto = TaskMapper.ToTable(response);

        return Ok(dto);
    }
    /// <summary>
    /// Guardar una tarea
    /// </summary>
    /// <param name="task">dto de guardar tareas</param>
    [HttpPost]
    public async Task<ActionResult> Post(AddTaskDTO task)
    {
        if (task is null)
            return BadRequest(ErrorHelper<AddTaskDTO>.Response(task, (int)HttpStatusCode.BadRequest, $"Verifique los datos ingresados"));

        var predicate = PredicateBuilder.New<User>(x => x.Id == task.RegisterBy);
        var user = await _userServices.GetAsync(predicate);
        if (user is null)
            return BadRequest(ErrorHelper<decimal>.Response(task.RegisterBy, (int)HttpStatusCode.BadRequest, $"No existe un usuario con el id {task.RegisterBy}"));

        var dto = TaskMapper.Add(task);

        await _taskServices.InsertAsync(dto);

        return Ok(task);
    }
    /// <summary>
    /// Editar una tarea
    /// </summary>
    /// <param name="task">dto de editar tareas</param>
    [HttpPut]
    public async Task<ActionResult> Put(UpdateTaskDTO task)
    {
        if (task is null || task.Id <= 0)
            return BadRequest(ErrorHelper<UpdateTaskDTO>.Response(task, (int)HttpStatusCode.BadRequest, "Revise la informacion enviada."));

        var predicate = PredicateBuilder.New<Tasks>(x => x.Id == task.Id);
        var response = await _taskServices.GetAsync(predicate);

        if (response is null)
            return NotFound(ErrorHelper<decimal>.Response(task.Id, (int)HttpStatusCode.BadRequest, $"No existe una tarea con este id {task.Id}"));

        var dto = TaskMapper.Update(task);

        await _taskServices.UpdateAsync(dto);
        return Ok(response);
    }
    /// <summary>
    /// desactivar una tarea
    /// </summary>
    /// <param name="id">id de la tarea</param>
    [HttpPut("{id}")]
    public async Task<ActionResult> Desactivation(int id)
    {
        if (id <= 0)
            return BadRequest(ErrorHelper<int>.Response(id, (int)HttpStatusCode.BadRequest, "Revise la informacion enviada."));

        var predicate = PredicateBuilder.New<Tasks>(x => x.Id == id);
        var response = await _taskServices.GetAsync(predicate);

        if (response is null)
            return NotFound(ErrorHelper<int>.Response(id, (int)HttpStatusCode.BadRequest, $"No existe una tarea con este id {id}"));
        
        await _taskServices.DesactivationAsync(id);

        var dto = TaskMapper.ToTable(response);
        return Ok(dto);
    }

    /// <summary>
    /// eliminar una tarea
    /// </summary>
    /// <param name="id">id de la tarea</param>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest(ErrorHelper<int>.Response(id, (int)HttpStatusCode.BadRequest, "Revise la informacion enviada."));

        var predicate = PredicateBuilder.New<Tasks>(x => x.Id == id);
        var response = await _taskServices.GetAsync(predicate);
        if (response is null)
            return BadRequest(ErrorHelper<int>.Response(id, (int)HttpStatusCode.BadRequest, "No existe una tarea con este id."));

        await _taskServices.DeleteAsync(response);

        return Ok();
    }
    
}
