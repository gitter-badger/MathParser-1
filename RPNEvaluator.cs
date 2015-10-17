using System;
using System.Collections.Generic;

namespace MathParsing
{
    public static class RPNEvaluator
    {
        /// <summary>
        /// Calculate expression in reverse-polish notation
        /// </summary>
        /// <param name="RPNExpression">Math expression in reverse-polish notation</param>
        /// <returns>Result</returns>
        public static double Evaluate(List<Token> RPNExpression, AngleType AngleType = AngleType.Radians)
        {
            var Stack = new Stack<double>(); // Contains operands

            // Analyse entire expression
            foreach (var Token in RPNExpression)
            {
                // if it's operand then just push it to stack
                if (Token.Is(typeof(Constant)))
                    Stack.Push((Constant)Token);

                else if (Token.Is(typeof(Variable)))
                    Stack.Push(((Variable)Token).Value);

                // Otherwise apply operator or function to elements in stack
                else if (Token.Is(typeof(UnaryOperator)))
                    Stack.Push((Token as UnaryOperator).Invoke(Stack.Pop()));

                else if (Token.Is(typeof(TrigonometricFunction)))
                    Stack.Push((Token as TrigonometricFunction).Invoke(Stack.Pop(), AngleType));

                else if (Token.Is(typeof(UnaryFunction), typeof(PostfixFunction)))
                    Stack.Push((Token as UnaryFunction).Invoke(Stack.Pop()));

                else if (Token.Is(typeof(BinaryOperator)))
                {
                    double Argument2 = Stack.Pop();
                    double Argument1 = Stack.Pop();

                    Stack.Push((Token as BinaryOperator).Invoke(Argument1, Argument2));
                }

                else if (Token.Is(typeof(ComparisonOperator)))
                {
                    double Argument2 = Stack.Pop();
                    double Argument1 = Stack.Pop();

                    Stack.Push((Boolean)((Token as ComparisonOperator).Invoke(Argument1, Argument2)));
                }

                else if (Token.Is(typeof(BinaryFunction)))
                {
                    double Argument2 = Stack.Pop();
                    double Argument1 = Stack.Pop();

                    Stack.Push((Token as BinaryFunction).Invoke(Argument1, Argument2));
                }

                else if (Token.Is(typeof(BinaryBooleanOperator)))
                {
                    Boolean Argument2 = Stack.Pop();
                    Boolean Argument1 = Stack.Pop();

                    Stack.Push((Token as BinaryBooleanOperator).Invoke(Argument1, Argument2));
                }

                else if (Token.Is(typeof(TernaryFunction)))
                {
                    double Argument3 = Stack.Pop();
                    double Argument2 = Stack.Pop();
                    double Argument1 = Stack.Pop();

                    Stack.Push((Token as TernaryFunction).Invoke(Argument1, Argument2, Argument3));
                }

                else if (Token.Is(typeof(IfFunction)))
                {
                    double Argument3 = Stack.Pop();
                    double Argument2 = Stack.Pop();
                    Boolean Argument1 = Stack.Pop();

                    Stack.Push((Token as IfFunction).Invoke(Argument1, Argument2, Argument3));
                }
            }

            // At end of analysis in stack should be only one operand (result)
            if (Stack.Count > 1) throw new ArgumentException("Excess operand");

            return Stack.Pop();
        }
    }
}