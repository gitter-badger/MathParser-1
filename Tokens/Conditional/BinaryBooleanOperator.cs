using System;

namespace MathParsing
{
    public class BinaryBooleanOperator : BooleanOperator
    {
        Func<bool, bool, bool> Procedure;

        public BinaryBooleanOperator(string Keyword, int Priority, Func<bool, bool, bool> Procedure)
            : base(Keyword, Priority)
        {
            this.Procedure = Procedure;
        }

        public Boolean Invoke(Boolean Arg1, Boolean Arg2) { return Procedure(Arg1, Arg2); }
    }
}
