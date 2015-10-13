using System;

namespace Sequention
{
    public static class Integration
    {
        /// <summary>
        /// Return the integral from a to b of function f
        /// using the left hand rule
        /// </summary>
        public static double LeftHand(double a, double b, Func<double, double> f, int strips = -1)
        {
            if (a >= b) return Double.NaN;  // constraint: a must be less than b

            // if strips is not provided, calculate it
            if (strips == -1) strips = GetStrips(a, b, f);

            double h = (b - a) / strips,
                acc = 0;

            for (int i = 0; i < strips; i++) acc += h * f(a + i * h);

            return acc;
        }

        /// <summary>
        /// Return the integral from a to b of function f 
        /// using the midpoint rule
        /// </summary>
        public static double MidPoint(double a, double b, Func<double, double> f, int strips = -1)
        {
            if (a >= b) return Double.NaN;  // constraint: a must be less than b

            // if strips is not provided, calculate it
            if (strips == -1) strips = GetStrips(a, b, f);

            double h = (b - a) / strips,
                x = a + h / 2,
                acc = 0;

            while (x < b)
            {
                acc += h * f(x);
                x += h;
            }

            return acc;
        }

        /// <summary>
        /// Return the integral from a to b of function f
        /// using trapezoidal rule
        /// </summary>
        public static double Trapezoidal(double a, double b, Func<double, double> f, int strips = -1)
        {
            if (a >= b) return Double.NaN;   // constraint: b must be less than a

            // if strips is not provided, calculate it
            if (strips == -1) strips = GetStrips(a, b, f);

            double h = (b - a) / strips,
                acc = (h / 2) * (f(a) + f(b));

            for (int i = 1; i < strips; i++) acc += h * f(a + i * h);

            return acc;
        }

        static int GetStrips(double a, double b, Func<double, double> f)
        {
            int strips = 100;

            for (int i = (int)a; i < b; i++)
                strips = (strips > f(i)) ? strips : (int)f(i);

            return strips;
        }
    }
}