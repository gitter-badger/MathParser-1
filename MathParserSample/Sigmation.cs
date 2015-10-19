using System;
using MathParsing;

namespace Sequention
{
    public static class Sigmation
    {
        public static double Sigma(string Expression, double LowerIndex, double UpperIndex, char IterationVariable, double Step)
        {
            double Result = 0;

            var parser = new MathParser();

            var Var = new Variable();

            parser.Variables.Add(IterationVariable.ToString(), Var);
            parser.Parse(Expression);

            for (double i = LowerIndex; i <= UpperIndex; i += Step)
            {
                Var.Value = i;
                Result += parser.Evaluate();
            }

            return Result;
        }

        public static double Pi(string Expression, double LowerIndex, double UpperIndex, char IterationVariable, double Step)
        {
            double Result = 1;

            var parser = new MathParser();

            var Var = new Variable();

            parser.Variables.Add(IterationVariable.ToString(), Var);
            parser.Parse(Expression);

            for (double i = LowerIndex; i <= UpperIndex; i += Step)
            {
                Var.Value = i;
                Result *= parser.Evaluate();
            }

            return Result;
        }

        public static double Integral(string Expression, double LowerIndex, double UpperIndex, char IterationVariable, int Kind)
        {
            var parser = new MathParser();

            var Var = new Variable();

            parser.Variables.Add(IterationVariable.ToString(), Var);
            parser.Parse(Expression);

            Func<double,double> proc = (x) =>
                {
                    Var.Value = x;
                    return parser.Evaluate();
                };
            
            if (Kind == 0) return Integration.Trapezoidal(LowerIndex, UpperIndex, proc);
            else if (Kind == 1) return Integration.LeftHand(LowerIndex, UpperIndex, proc);
            else return Integration.MidPoint(LowerIndex, UpperIndex, proc);
        }
    }
}