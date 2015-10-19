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
                if (Token is Constant)
                    Stack.Push((Constant)Token);

                else if (Token is Variable)
                    Stack.Push(((Variable)Token).Value);

                else if (Token is TrigonometricFunction)
                    Stack.Push((Token as TrigonometricFunction).Invoke(Stack.Pop(), AngleType));

                // Otherwise apply operator or function to elements in stack
                else if (Token is IEvaluatable)
                {
                    int Count = (Token as IEvaluatable).ParameterCount;

                    List<double> Arguments = new List<double>(Count);

                    for (int i = 0; i < Count; ++i) 
                        Arguments.Add(Stack.Pop());

                    Arguments.Reverse();

                    Stack.Push((Token as IEvaluatable).Invoke(Arguments.ToArray()));
                }
            }

            // At end of analysis in stack should be only one operand (result)
            if (Stack.Count > 1) throw new ArgumentException("Excess operand");

            return Stack.Pop();
        }
    }
}