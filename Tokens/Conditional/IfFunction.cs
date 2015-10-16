using System;

namespace MathParsing
{
    public class IfFunction : Function
    {
        IfFunction(string Keyword) : base(Keyword, 3) { }

        public double Invoke(Boolean Arg, double OnTrue, double OnFalse) { return Arg ? OnTrue : OnFalse; }

        public static readonly IfFunction Instance = new IfFunction("If");
    }
}
