using System;

namespace MathParsing
{
    public sealed class BinaryBooleanOperator : BinaryOperator
    {
        Func<bool, bool, bool> Procedure;

        public BinaryBooleanOperator(string Keyword, int Priority, Func<bool, bool, bool> Procedure)
            : base(Keyword, Priority, (x, y) => (Boolean)Procedure((Boolean)x, (Boolean)y))
        {
            this.Procedure = Procedure;
        }
    }
}
