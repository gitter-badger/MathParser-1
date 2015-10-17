using System;
using MathParsing.Properties;

namespace MathParsing
{
    public class Operator : Token
    {
        internal Operator(string Keyword, int Priority = 0)
            : base(Keyword, Priority)
        {
            if (Keyword.Length == 1 && Keyword[0].Is('(', ')', ',')) ;
            else if (Char.IsLetter(Keyword[0]))
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

        public static readonly Operator LeftParenthesis = new Operator("("),
            RightParenthesis = new Operator(")");
    }
}
