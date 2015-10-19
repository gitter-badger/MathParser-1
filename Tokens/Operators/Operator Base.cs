using System;
using MathParsing.Properties;

namespace MathParsing
{
    public abstract class Operator : Token, IEvaluatable
    {
        public int ParameterCount { get; private set; }

        public Operator(string Keyword, int ParameterCount, int Priority)
            : base(Keyword, Priority)
        {
            this.ParameterCount = ParameterCount;

            if (Char.IsLetter(Keyword[0]))
            {
                foreach (var Character in Keyword)
                    if (!Char.IsLetter(Character))
                        throw new FormatException(Resources.OperatorFormatError);
            }
            else
            {
                foreach (var Character in Keyword)
                    if (Char.IsLetterOrDigit(Character) || Character.Is(')', '(', ','))
                        throw new FormatException(Resources.OperatorFormatError);
            }
        }


        public abstract double Invoke(double[] Parameters);
    }
}
