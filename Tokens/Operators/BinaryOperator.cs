using System;

namespace MathParsing
{
    public class BinaryOperator : Operator
    {
        Func<double, double, double> Function;

        public BinaryOperator(string Keyword, int Priority, Func<double, double, double> Function)
            : base(Keyword, 2, Priority)
        {
            this.Function = Function;
        }

        public double Invoke(double Arg1, double Arg2) { return Function(Arg1, Arg2); }

        public override double Invoke(double[] Parameters) { return Invoke(Parameters[0], Parameters[1]); }
    }
}
