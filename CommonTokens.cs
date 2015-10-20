using System;
using System.Collections.Generic;
using MathParsing.Properties;

namespace MathParsing
{
    static class CommonTokens
    {
        public static readonly UnaryOperator UnaryPlus = new UnaryOperator(8, (x) => x),
            UnaryMinus = new UnaryOperator(8, (x) => -x);

        public static readonly BinaryOperator Plus = new BinaryOperator(4, (x, y) => x + y),
                Minus = new BinaryOperator(4, (x, y) => x - y),
                Multiply = new BinaryOperator(6, (x, y) => x * y),
                Power = new BinaryOperator(10, Math.Pow);

        public static readonly TokenDictionary<Operator> Operators =
            new TokenDictionary<Operator>((S) =>
                {
                    if (Char.IsLetter(S[0]))
                    {
                        foreach (var Character in S)
                            if (!Char.IsLetter(Character))
                                return false;

                        return true;
                    }
                    else
                    {
                        foreach (var Character in S)
                            if (Char.IsLetterOrDigit(Character) || Character.Is(')', '(', ','))
                                return false;

                        return true;
                    }
                }, Resources.OperatorFormatError)
            {
                //{ "un+", UnaryPlus },
                //{ "un-", UnaryMinus },
                { "+", Plus },
                { "-", Minus },
                { "*", Multiply },
                { "^", Power },
                { "/", new BinaryOperator(7, (x, y) => x / y) },
                { "%", new BinaryOperator(10, (x, y) => x % y) },
                { "==", new ComparisonOperator(3, (x, y) => x == y) },
                { "!=", new ComparisonOperator(3, (x, y) => x != y) },
                { ">", new ComparisonOperator(3, (x, y) => x > y) },
                { "<", new ComparisonOperator(3, (x, y) => x < y) },
                { ">=", new ComparisonOperator(3, (x, y) => x >= y) },
                { "<=", new ComparisonOperator(3, (x, y) => x <= y) },
                { "&", new BinaryBooleanOperator(1, (x, y) => x && y) },
                { "|", new BinaryBooleanOperator(1, (x, y) => x || y) },
                { "~", new UnaryOperator(8, (x) => (Boolean) !(Boolean)x) }
            };

        public static readonly TokenDictionary<Function> Functions =
            new TokenDictionary<Function>((S)=>
                {
                    if (Char.IsLetter(S[0]))
                    {
                        foreach (var Character in S)
                            if (!Char.IsLetter(Character))
                                return false;

                        return true;
                    }
                    else
                    {
                        if (S.Length != 1 || S[0].Is(')', '(', ',') || Char.IsDigit(S[0]))
                            return false;

                        return true;
                    }
                }, Resources.FunctionFormatError)
            {
                { "sqrt", new UnaryFunction(Math.Sqrt) },

                // Trigonometric
                { "sin", new TrigonometricFunction(Math.Sin) },
                { "cos", new TrigonometricFunction(Math.Cos) },
                { "tan", new TrigonometricFunction(Math.Tan) },
                { "sec", new TrigonometricFunction((x) => 1 / Math.Cos(x)) },
                { "cosec", new TrigonometricFunction((x) => 1 / Math.Sin(x)) },
                { "cot", new TrigonometricFunction((x) => 1 / Math.Tan(x)) },
                
                // Inverse Trigonometric
                { "asin", new UnaryFunction(Math.Asin) },
                { "acos", new UnaryFunction(Math.Acos) },
                { "atan", new UnaryFunction(Math.Atan) },

                // Hyperbolic
                { "sinh", new UnaryFunction(Math.Sinh) },
                { "cosh", new UnaryFunction(Math.Cosh) },
                { "tanh", new UnaryFunction(Math.Tanh) },
                
                { "ln", new UnaryFunction(Math.Log) },
                { "log", new BinaryFunction(Math.Log) },
                { "exp", new UnaryFunction(Math.Exp) },

                { "abs", new UnaryFunction(Math.Abs) },
                
                { "floor", new UnaryFunction(Math.Floor) },
                { "ceiling", new UnaryFunction(Math.Ceiling) },
                { "truncate", new UnaryFunction(Math.Truncate)},
                { "round", new BinaryFunction((x, y) => Math.Round(x, (int)y)) },

                { "sign", new UnaryFunction((x) => Math.Sign(x)) },
                
                { "max", new BinaryFunction(Math.Max) },
                { "min", new BinaryFunction(Math.Min) },
                
                { "!", new PostfixFunction((x) => Combinatorics.Factorial((int)x)) },
                { "c", new BinaryFunction((x, y) => Combinatorics.C((int)x, (int)y)) },
                { "p", new BinaryFunction((x, y) => Combinatorics.P((int)x, (int)y)) },

                { "clip", new TernaryFunction((val, min, max) =>
                    {
                        if (val < min) return min;
                        else if (val > max) return max;
                        else return val;
                    })
                },
                
                { "if", new TernaryFunction((Condition, If, Else) => (Boolean)Condition ? If : Else) }
            };

        public static readonly TokenDictionary<Constant> Constants = 
            new TokenDictionary<Constant>((S) =>
            {
                if (Char.IsLetter(S[0]))
                {
                    foreach (var Character in S)
                        if (!Char.IsLetter(Character))
                            return false;

                    return true;
                }
                else
                {
                    if (S.Length != 1 || S[0].Is(')', '(', ',') || Char.IsDigit(S[0]))
                        return false;

                    return true;
                }
            }, Resources.ConstantFormatError)
            {
                {"pi", Math.PI },
                {"e", Math.E },
                {"true", 1},
                {"false", 0}
            };
    }
}