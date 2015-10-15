namespace MathParsing
{
    class Boolean : Token
    {
        Boolean(bool Keyword) : base(Keyword.ToString(), 0) { }

        public static readonly Boolean True = new Boolean(true),
            False = new Boolean(false);

        public static implicit operator double(Boolean B) { return B.Keyword.Contains("true") ? 1 : 0; }
        public static implicit operator Boolean(double D) { return D == 0 ? False : True; }
    }
}
