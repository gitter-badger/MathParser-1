using System;

namespace MathParsing
{
    public class UnaryFunction : Function
    {
        Func<double, double> Procedure;

        public UnaryFunction(string Keyword, Func<double, double> Procedure)
            : base(Keyword, 1)
        {
            this.Procedure = Procedure;
        }

        public double Invoke(double Arg) { return Procedure(Arg); }

        public override double Invoke(double[] Parameters) { return Invoke(Parameters[0]); }
    }
}
