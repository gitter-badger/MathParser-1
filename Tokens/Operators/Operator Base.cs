using System;
using MathParsing.Properties;

namespace MathParsing
{
    public abstract class Operator : Token, IEvaluatable
    {
        public int ParameterCount { get; private set; }

        public Operator(int ParameterCount, int Priority)
            : base(Priority)
        {
            this.ParameterCount = ParameterCount;           
        }


        public abstract double Invoke(double[] Parameters);
    }
}
