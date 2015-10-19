using System;

namespace MathParsing
{
    public class TernaryFunction : Function
    {
        Func<double, double, double, double> Procedure;

        public TernaryFunction(Func<double, double, double, double> Procedure)
            : base(3)
        {
            this.Procedure = Procedure;
        }

        public double Invoke(double Arg1, double Arg2, double Arg3) { return Procedure(Arg1, Arg2, Arg3); }

        public override double Invoke(double[] Parameters) { return Invoke(Parameters[0], Parameters[1], Parameters[2]); }
    }
}
