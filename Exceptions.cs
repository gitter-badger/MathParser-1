using System;

namespace MathParsing
{
    public class KeywordFormatException : Exception
    {
        public KeywordFormatException(string Message) : base(Message) { }
    }

    public class TokenNotDefinedException : Exception { }
}