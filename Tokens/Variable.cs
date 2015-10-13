namespace MathParsing
{
    public class Variable : Token
    {
        public double Value { get; set; }

        public Variable(string Keyword, double Value = 0) : base(Keyword, 0) { this.Value = Value; }
    }
}