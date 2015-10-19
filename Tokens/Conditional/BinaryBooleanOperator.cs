using System;

namespace MathParsing
{
    public sealed class BinaryBooleanOperator : BinaryOperator
    {
        Func<bool, bool, bool> Procedure;

        public BinaryBooleanOperator(int Priority, Func<bool, bool, bool> Procedure)
            : base(Priority, (x, y) => (Boolean)Procedure((Boolean)x, (Boolean)y))
        {
            this.Procedure = Procedure;
        }
    }
}
