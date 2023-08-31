namespace ExceptionClasses;

public class VoiceException : Exception
{
    public VoiceException() { }
    public VoiceException(string message) : base(message) { }
    public VoiceException(string message, Exception e) : base(message, e) { }
}
