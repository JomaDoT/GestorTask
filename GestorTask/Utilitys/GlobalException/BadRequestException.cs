namespace GestorTask.Utilitys.GlobalException;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    { }
}