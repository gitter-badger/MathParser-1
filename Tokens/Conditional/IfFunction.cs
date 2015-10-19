using System;

namespace MathParsing
{
    public sealed class IfFunction : TernaryFunction
    {
        IfFunction(string Keyword) : base(Keyword, (Condition, If, Else) => (Boolean)Condition ? If : Else) { }

        public static readonly IfFunction Instance = new IfFunction("If");
    }
}
