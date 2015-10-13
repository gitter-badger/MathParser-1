namespace MathParsing
{
    public abstract class Function : Token
    {
        public Function(string Keyword, int Priority = 10) : base(Keyword, Priority) { }
    }
}
