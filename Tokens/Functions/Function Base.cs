using System;
using MathParsing.Properties;

namespace MathParsing
{
    public abstract class Function : Token, IEvaluatable
    {
        public int ParameterCount { get; private set; }

        public Function(string Keyword, int ParamCount) 
            : base(Keyword, 10) 
        {
            if (Char.IsLetter(Keyword[0]))
            {
                foreach (var Character in Keyword)
                    if (!Char.IsLetter(Character))
                        throw new FormatException(Resources.FunctionFormatError);
            }
            else
            {
                if (Keyword.Length != 1 || Keyword[0].Is(')', '(', ',') || Char.IsDigit(Keyword[0]))
                    throw new FormatException(Resources.FunctionFormatError);
            }

            this.ParameterCount = ParamCount;
        }


        public abstract double Invoke(double[] Parameters);
    }
}
