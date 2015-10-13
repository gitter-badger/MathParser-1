using System;

namespace MathParsing
{
    public class UnaryFunction : Function
    {
        Func<double, double> Procedure;

        public UnaryFunction(string Keyword, Func<double, double> Procedure)
            : base(Keyword)
        {
            this.Procedure = Procedure;
        }

        public double Invoke(double Arg) { return Procedure(Arg); }
    }
}
