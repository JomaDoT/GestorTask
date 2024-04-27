using GestorTask.Application.Interfaces;
using GestorTask.Infraestructure.Models;
using GestorTask.Utilitys.Responses;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GestorTask.Controllers;

//[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
public class UserController : BaseController
{
    private readonly IUserServices _userServices;

    public UserController(IUserServices userServices)
    {
        _userServices = userServices;
    }
    /// <summary>
    /// Buscar los usuarios
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var response = await _userServices.GetAllAsync();
        if (response is null || !response.Any())
            return NotFound($"No existen usuarios.");

        return Ok(response);
    }
    /// <summary>
    /// Buscar un usuario por el id
    /// </summary>
    /// <param name="id">id del usuario</param>
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        if (id <= 0)
            return BadRequest(ErrorHelper<int>.Response(id, (int)HttpStatusCode.BadRequest, $"No se permiten valores igual o menores a 0"));

        var predicate = PredicateBuilder.New<User>(x => x.Id == id);
        var response = await _userServices.GetAsync(predicate);
        if (response is null)
            return NotFound($"No existe un usuario con este id {id}");

        return Ok(response);
    }

}