namespace MathParsing
{
    public sealed class Boolean : Token
    {
        bool Value;

        Boolean(bool Value) : base(0) { this.Value = Value; }

        public static readonly Boolean True = new Boolean(true),
            False = new Boolean(false);

        public static implicit operator bool(Boolean B) { return B == True; }
        public static implicit operator Boolean(bool b) { return b ? True : False; }
        public static implicit operator double(Boolean B) { return B == True ? 1 : 0; }
        public static implicit operator Boolean(double D) { return D == 0 ? False : True; }
    }
}
