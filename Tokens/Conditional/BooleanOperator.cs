using System;

namespace MathParsing
{
    abstract class BooleanOperator : Operator
    {
        public BooleanOperator(string Keyword, int Priority) : base(Keyword, Priority) { }
    }

    class ComparisonOperator : BooleanOperator
    {
        Func<double, double, bool> Procedure;

        public ComparisonOperator(string Keyword, int Priority, Func<double, double, bool> Procedure)
            : base(Keyword, Priority)
        {
            this.Procedure = Procedure;
        }

        public bool Invoke(double Arg1, double Arg2) { return Procedure(Arg1, Arg2); }

        public static readonly ComparisonOperator EqualTo = new ComparisonOperator("==", 0, (x, y) => x == y),
            NotEqualTo = new ComparisonOperator("!=", 0, (x, y) => x != y),
            GreaterThan = new ComparisonOperator(">", 0, (x, y) => x > y),
            LessThan = new ComparisonOperator("<", 0, (x, y) => x < y),
            GreaterThanOrEqualTo = new ComparisonOperator(">=", 0, (x, y) => x >= y),
            LessThanOrEqualTo = new ComparisonOperator("<=", 0, (x, y) => x <= y);
    }

    class BinaryBooleanOperator : BooleanOperator
    {
        Func<bool, bool, bool> Procedure;

        public BinaryBooleanOperator(string Keyword, int Priority, Func<bool, bool, bool> Procedure)
            : base(Keyword, Priority)
        {
            this.Procedure = Procedure;
        }

        public bool Invoke(bool Arg1, bool Arg2) { return Procedure(Arg1, Arg2); }

        public static readonly BinaryBooleanOperator And = new BinaryBooleanOperator("&", 0, (x, y) => x && y),
            Or = new BinaryBooleanOperator("|", 0, (x, y) => x || y);
    }

    class IfFunction : Function
    {
        IfFunction(string Keyword) : base(Keyword) { }

        public double Invoke(bool Arg, double True, double False) { return Arg ? True : False; }

        public static readonly IfFunction Instance = new IfFunction("If");
    }
}
