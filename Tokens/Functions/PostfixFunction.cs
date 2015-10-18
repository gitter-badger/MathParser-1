using System;

namespace MathParsing
{
    public sealed class PostfixFunction : UnaryFunction
    {
        public PostfixFunction(string Keyword, Func<double, double> Procedure) : base(Keyword, Procedure) { }
    }
}
