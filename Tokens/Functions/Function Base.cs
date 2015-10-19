using System;
using MathParsing.Properties;

namespace MathParsing
{
    public abstract class Function : Token, IEvaluatable
    {
        public int ParameterCount { get; private set; }

        public Function(int ParamCount) 
            : base(20) 
        {
            this.ParameterCount = ParamCount;
        }


        public abstract double Invoke(double[] Parameters);
    }
}
