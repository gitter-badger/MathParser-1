using System;

namespace MathParsing
{
    public sealed class BinaryFunction : Function
    {
        Func<double, double, double> Procedure;

        public BinaryFunction(string Keyword, Func<double, double, double> Procedure)
            : base(Keyword, 2)
        {
            this.Procedure = Procedure;
        }

        public double Invoke(double Arg1, double Arg2) { return Procedure(Arg1, Arg2); }

        public override double Invoke(double[] Parameters) { return Invoke(Parameters[0], Parameters[1]); }
    }
}
