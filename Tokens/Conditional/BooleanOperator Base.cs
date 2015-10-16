using System;

namespace MathParsing
{
    public abstract class BooleanOperator : Operator
    {
        public BooleanOperator(string Keyword, int Priority) : base(Keyword, Priority) { }
    }
}
