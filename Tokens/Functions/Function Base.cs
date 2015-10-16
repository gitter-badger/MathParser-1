namespace MathParsing
{
    public abstract class Function : Token
    {
        public int ParameterCount { get; private set; }

        public Function(string Keyword, int ParamCount, int Priority = 10) 
            : base(Keyword, Priority) 
        {
            this.ParameterCount = ParamCount;
        }
    }
}
