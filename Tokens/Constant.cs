namespace MathParsing
{
    public sealed class Constant : Token
    {
        double Value;

        Constant(double Value) : base(0) { this.Value = Value; }

        public static implicit operator Constant(double Number) { return new Constant(Number); }

        public static implicit operator double(Constant Token) { return Token.Value; }

        public override string ToString() { return Value.ToString(); }
    }
}