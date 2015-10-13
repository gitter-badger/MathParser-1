using System;

namespace MathParsing
{
    public class BinaryFunction : Function
    {
        Func<double, double, double> Procedure;

        public BinaryFunction(string Keyword, Func<double, double, double> Procedure)
            : base(Keyword)
        {
            this.Procedure = Procedure;
        }

        public double Invoke(double Arg1, double Arg2) { return Procedure(Arg1, Arg2); }
    }
}
