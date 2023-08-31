namespace ExceptionClasses;

public class RecognizeException : Exception
{
    public RecognizeException() { }
    public RecognizeException(string message) : base(message) { }
    public RecognizeException(string message, Exception e) : base(message, e) { }
}