namespace ExceptionClasses;

public class EmptyFieldException : Exception
{
    public EmptyFieldException() { }
    public EmptyFieldException(string message) : base(message) { }
    public EmptyFieldException(string message, Exception e) : base(message, e) { }
}
