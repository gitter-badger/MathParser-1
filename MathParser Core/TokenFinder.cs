using System;
using System.Linq;

namespace MathParsing
{
    public partial class MathParser
    {
        bool IsOperatorDefined(string Keyword)
        {
            foreach (var Operator in DefaultOperators.Union(Operators))
                if (Operator.Keyword == Keyword)
                    return true;

            return false;
        }

        bool IsFunctionDefined(string Keyword)
        {
            foreach (var Function in DefaultFunctions.Union(Functions))
                if (Function.Keyword == Keyword)
                    return true;

            return false;
        }

        bool IsVariableDefined(string Keyword)
        {
            foreach (var Variable in Variables)
                if (Variable.Keyword == Keyword)
                    return true;

            return false;
        }

        Operator FindOperator(string Keyword)
        {
            foreach (var Operator in DefaultOperators.Union(Operators))
                if (Operator.Keyword == Keyword) return Operator;

            throw new ArgumentException("Invalid Operator Token");
        }

        Function FindFunction(string Keyword)
        {
            foreach (var Function in DefaultFunctions.Union(Functions))
                if (Function.Keyword == Keyword) return Function;

            throw new ArgumentException("Invalid Function Token");
        }

        Variable FindVariable(string Keyword)
        {
            foreach (var Variable in Variables)
                if (Variable.Keyword == Keyword) return Variable;

            throw new ArgumentException("Undefined Variable");
        }
    }
}