namespace GestorTask.Utilitys.GlobalException;
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    { }
}