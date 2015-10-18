using System;

namespace MathParsing
{
    public sealed class TernaryFunction : Function
    {
        Func<double, double, double, double> Procedure;

        public TernaryFunction(string Keyword, Func<double, double, double, double> Procedure)
            : base(Keyword, 3)
        {
            this.Procedure = Procedure;
        }

        public double Invoke(double Arg1, double Arg2, double Arg3) { return Procedure(Arg1, Arg2, Arg3); }
    }
}
