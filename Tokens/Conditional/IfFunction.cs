using System;

namespace MathParsing
{
    public class IfFunction : Function
    {
        IfFunction(string Keyword) : base(Keyword, 3) { }

        public double Invoke(Boolean Condition, double If, double Else) { return Condition ? If : Else; }

        public static readonly IfFunction Instance = new IfFunction("If");
    }
}
