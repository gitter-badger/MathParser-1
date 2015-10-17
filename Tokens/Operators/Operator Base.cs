using System;
using MathParsing.Properties;

namespace MathParsing
{
    public abstract class Operator : Token
    {
        public Operator(string Keyword, int Priority = 0)
            : base(Keyword, Priority)
        {
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
    }
}
