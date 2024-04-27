using GestorTask.Application.Interfaces;
using GestorTask.Application.Mappers;
using GestorTask.Applications.DTO;
using GestorTask.Applications.DTO.User;
using GestorTask.Infraestructure.Models;
using GestorTask.Utilitys;
using GestorTask.Utilitys.Responses;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.RegularExpressions;

namespace GestorTask.Controllers;

public class AuthenticationsController : BaseController
{
    private readonly IAuthenticationServices _authServices;
    private readonly IUserServices _userServices;
    private readonly IConfiguration _configuration;
    public AuthenticationsController(IAuthenticationServices auth, IUserServices user,
        IConfiguration configuration)
    {
        _authServices = auth;
        _userServices = user;
        _configuration = configuration;
    }
    /// <summary>
    /// acceso Login
    /// </summary>
    /// <param name="userLogin">Objeto de login</param>
    [HttpPost]
    public async Task<IActionResult> Login(UserLoginDTO userLogin)
    {
        var predicate = PredicateBuilder.New<User>(x => x.Username == userLogin.UserName);
        bool response = await _authServices.ValidateLogin(predicate, userLogin.UserName, userLogin.Password);

        //acceso incorrecto
        if (!response)
            return BadRequest(ErrorHelper<UserLoginDTO>.Response(userLogin, (int)HttpStatusCode.BadRequest, "Usuario y/o contraseña incorrecta."));

        //acceso valido                

        var predicateUser = PredicateBuilder.New<User>(x => x.Username == userLogin.UserName);
        User responseUser = await _userServices.GetAsync(predicate);

        TableInfoUserDTO mapFullUser = UserMapper.ToTable(responseUser);
        mapFullUser.Token = JwtGenerateToken.GenerateToken(responseUser, _configuration["Jwt:Key"], _configuration["Jwt:Issuer"]);

        return Ok(mapFullUser);
    }

    /// <summary>
    /// Registrar usuario
    /// </summary>
    /// <param name="user">Objeto de usuario</param>
    /// <returns></returns>
    /// 
    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register(AddUserDTO user)
    {
        if (string.IsNullOrEmpty(user.UserName))
            return BadRequest(ErrorHelper<string>.Response(user.UserName, (int)HttpStatusCode.BadRequest, "Debe escribir un nombre de usuario."));

        if (string.IsNullOrEmpty(user.PassWord) || string.IsNullOrEmpty(user.ConfirmPassWord))
            return BadRequest(ErrorHelper<string>.Response(user.PassWord, (int)HttpStatusCode.BadRequest, "Verifique que haya escrito ambas contraseñas."));

        if (user.PassWord != user.ConfirmPassWord)
            return BadRequest(ErrorHelper<string>.Response(user.ConfirmPassWord, (int)HttpStatusCode.BadRequest, "Ambas contraseñas no coinciden."));

        Match matchLongitud = Regex.Match(user.PassWord, @"^\w{8,15}\b");

        if (!matchLongitud.Success)
            return BadRequest(ErrorHelper<string>.Response(user.PassWord, (int)HttpStatusCode.BadRequest, "Contraseña debe tener como minimo 8 caracteres."));

        var map = UserMapper.AddMapper(user);

        await _authServices.RegisterAsync(map);

        return Ok();
    }


    /// <summary>
    /// Cambiar Contraseña
    /// </summary>
    /// <param name="dto">objeto de cambiar contaseña</param>
    /// <returns></returns>
    [HttpPut, Authorize]
    public async Task<IActionResult> ResetPassWord(ChangePassWordDTO dto)
    {

        if (string.IsNullOrEmpty(dto.UserName))
            return BadRequest(ErrorHelper<string>.Response(dto.UserName, (int)HttpStatusCode.BadRequest, "Usuario esta vacio."));
        if (string.IsNullOrEmpty(dto.PasswordBefore))
            return BadRequest(ErrorHelper<string>.Response(dto.PasswordBefore, (int)HttpStatusCode.BadRequest, "Contraseña actual esta vacia."));
        if (string.IsNullOrEmpty(dto.PasswordNew))
            return BadRequest(ErrorHelper<string>.Response(dto.PasswordNew, (int)HttpStatusCode.BadRequest, "Nueva contraseña esta vacia."));
        if (string.IsNullOrEmpty(dto.ConfirmPassword))
            return BadRequest(ErrorHelper<string>.Response(dto.ConfirmPassword, (int)HttpStatusCode.BadRequest, "Confirmacion nueva contraseña esta vacia."));
        if (dto.PasswordNew != dto.ConfirmPassword)
            return BadRequest(ErrorHelper<string>.Response(dto.PasswordNew, (int)HttpStatusCode.BadRequest, "Contraseñas no coinciden."));

        Match matchLongitud = Regex.Match(dto.PasswordNew, @"^\w{8,15}\b");

        if (!matchLongitud.Success)
            return BadRequest(ErrorHelper<string>.Response(dto.PasswordNew, (int)HttpStatusCode.BadRequest, "Contraseña debe tener como minimo 8 caracteres."));

        //se busca en la vista un usuario por el username y password           
        var predicate = PredicateBuilder.New<User>(x => x.Username == dto.UserName);
        bool valid = await _authServices.ValidateLogin(predicate, dto.UserName, dto.PasswordBefore);

        if (!valid)
            return BadRequest(ErrorHelper<string>.Response(dto.UserName, (int)HttpStatusCode.BadRequest, "Usuario o contraseña incorrecta."));

        //se busca en usuario por el id del usuario general
        var predicateValidUser = PredicateBuilder.New<User>(x => x.Username == dto.UserName);
        var user = await _userServices.GetAsync(predicateValidUser);

        user.Password = dto.PasswordNew;
        await _authServices.UpdatePassWordAsync(user);

        return Ok();
    }

}