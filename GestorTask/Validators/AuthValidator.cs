
using FluentValidation;
using GestorTask.Applications.DTO;
using GestorTask.Applications.DTO.User;

namespace GestorTask.Validators;

public class AuthValidator : AbstractValidator<UserLoginDTO>
{
    public AuthValidator()
    {

        RuleFor(x => x.UserName).CustomNotNullEmptyEqual("string", "Nombres");
        RuleFor(x => x.Password).CustomNotNullEmptyEqual("string", "Apellidos");
    }
}
public class AuthChangePasswordValidator : AbstractValidator<ChangePassWordDTO>
{
    public AuthChangePasswordValidator()
    {

        RuleFor(x => x.UserName).CustomNotNullEmptyEqual("string", "Nombres");
        RuleFor(x => x.PasswordBefore).CustomNotNullEmptyEqual("string", "contraseña anterior");
        RuleFor(x => x.PasswordNew).CustomNotNullEmptyEqual("string", "contraseña nueva");
        RuleFor(x => x.ConfirmPassword).CustomNotNullEmptyEqual("string", "La confirmacion de contraseña nueva");
    }        
}
public class AuthRegisterPasswordValidator : AbstractValidator<AddUserDTO>
{
    public AuthRegisterPasswordValidator()
    {

        RuleFor(x => x.UserName).CustomNotNullEmptyEqual("string", "Nombres");
        RuleFor(x => x.PassWord).CustomNotNullEmptyEqual("string", "contraseña");
        RuleFor(x => x.ConfirmPassWord).CustomNotNullEmptyEqual("string", "La confirmacion de contraseña nueva");
    }
}
