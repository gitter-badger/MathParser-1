using System;

namespace MathParsing
{
    public sealed class ComparisonOperator : BooleanOperator
    {
        Func<double, double, bool> Procedure;

        public ComparisonOperator(string Keyword, int Priority, Func<double, double, bool> Procedure)
            : base(Keyword, Priority)
        {
            this.Procedure = Procedure;
        }

        public bool Invoke(double Arg1, double Arg2) { return Procedure(Arg1, Arg2); }
    }
}
