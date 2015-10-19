using System;

namespace MathParsing
{
    public sealed class UnaryOperator : Operator
    {
        Func<double, double> Function;

        public UnaryOperator(int Priority, Func<double, double> Function)
            : base(1, Priority)
        {
            this.Function = Function;
        }

        public double Invoke(double Arg) { return Function(Arg); }

        public override double Invoke(double[] Parameters) { return Invoke(Parameters[0]); }
    }
}
