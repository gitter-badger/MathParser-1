using System;
using MathParsing.Properties;

namespace MathParsing
{
    public abstract class Function : Token
    {
        public int ParameterCount { get; private set; }

        public Function(string Keyword, int ParamCount, int Priority = 10) 
            : base(Keyword, Priority) 
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
    }
}
