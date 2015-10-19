using System;

namespace MathParsing
{
    public sealed class ComparisonOperator : BinaryOperator
    {
        Func<double, double, bool> Procedure;

        public ComparisonOperator(string Keyword, int Priority, Func<double, double, bool> Procedure)
            : base(Keyword, Priority, (x, y) => (Boolean)Procedure(x, y))
        {
            this.Procedure = Procedure;
        }
    }
}
