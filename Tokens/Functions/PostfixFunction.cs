using System;

namespace MathParsing
{
    public sealed class PostfixFunction : UnaryFunction
    {
        public PostfixFunction(Func<double, double> Procedure) : base(Procedure) { }
    }
}
