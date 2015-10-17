using System;
using MathParsing.Properties;

namespace MathParsing
{
    public class Variable : Token
    {
        public double Value { get; set; }

        public Variable(string Keyword, double Value = 0)
            : base(Keyword, 0)
        {
            if (Char.IsLetter(Keyword[0]))
            {
                foreach (var Character in Keyword)
                    if (!Char.IsLetter(Character))
                        throw new FormatException(Resources.VariableFormatError);
            }
            else
            {
                if (Keyword.Length != 1 || Keyword[0].Is(')', '(', ',') || Char.IsDigit(Keyword[0]))
                    throw new FormatException(Resources.VariableFormatError);
            }

            this.Value = Value;
        }
    }
}