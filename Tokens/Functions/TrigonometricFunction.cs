using System;

namespace MathParsing
{
    public class TrigonometricFunction : UnaryFunction
    {
        public TrigonometricFunction(string Keyword, Func<double, double> Procedure)
            : base(Keyword, Procedure) { }

        public double Invoke(double Arg, AngleType AngleType)
        {
            switch (AngleType)
            {
                case AngleType.Degrees:
                    return Invoke((Math.PI / 180) * Arg);

                case AngleType.Grades:
                    return Invoke((Math.PI / 200) * Arg);

                default:
                case AngleType.Radians:
                    return Invoke(Arg);
            }
        }
    }
}
