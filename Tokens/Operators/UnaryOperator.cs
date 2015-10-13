using System;

namespace MathParsing
{
    public class UnaryOperator : Operator
    {
        Func<double, double> Function;

        public UnaryOperator(string Keyword, int Priority, Func<double, double> Function)
            : base(Keyword, Priority)
        {
            this.Function = Function;
        }

        public double Invoke(double Arg) { return Function(Arg); }
    }
}
