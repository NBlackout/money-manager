namespace MoneyManager.Infrastructure.Write.OfxProcessor;

public class CannotProcessOfxContent : Exception
{
    public CannotProcessOfxContent(string message) : base(message)
    {
    }
}