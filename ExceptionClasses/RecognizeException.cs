namespace ExceptionClasses
{
    public class RecognizeException : Exception
    {
        public RecognizeException() { }
        public RecognizeException(string message) : base(message) { }
        public RecognizeException(string message, Exception e) : base(message, e) { }
    }
    public class VoiceException : Exception
    {
        public VoiceException() { }
        public VoiceException(string message) : base(message) { }
        public VoiceException(string message, Exception e) : base(message, e) { }
    }
    public class EmptyFieldException : Exception
    {
        public EmptyFieldException() { }
        public EmptyFieldException(string message) : base(message) { }
        public EmptyFieldException(string message, Exception e) : base(message, e) { }
    }
}