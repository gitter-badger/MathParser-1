using System;

namespace MathParsing
{
    public sealed class TrigonometricFunction : UnaryFunction
    {
        public TrigonometricFunction(Func<double, double> Procedure)
            : base(Procedure) { }

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
