using System;

namespace MathParsing
{
    public class UnaryOperator : Operator
    {
        Func<double, double> Function;

        public UnaryOperator(char Keyword, int Priority, Func<double, double> Function)
            : base(Keyword.ToString(), Priority)
        {
            this.Function = Function;
        }

        public double Invoke(double Arg) { return Function(Arg); }
    }
}
