using System;

namespace MathParsing
{
    public sealed class ComparisonOperator : BinaryOperator
    {
        Func<double, double, bool> Procedure;

        public ComparisonOperator(int Priority, Func<double, double, bool> Procedure)
            : base(Priority, (x, y) => (Boolean)Procedure(x, y))
        {
            this.Procedure = Procedure;
        }
    }
}
