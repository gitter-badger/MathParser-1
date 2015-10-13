using System;
using System.Collections.Generic;

namespace MathParsing
{
    public partial class MathParser
    {
        /// <summary>
        /// Calculate expression in reverse-polish notation
        /// </summary>
        /// <param name="Expression">Math expression in reverse-polish notation</param>
        /// <returns>Result</returns>
        double CalculateRPN(List<Token> Expression)
        {
            var Stack = new Stack<double>(); // Contains operands

            // Analyse entire expression
            foreach (var token in Expression)
                SyntaxAnalysisRPN(Stack, token);

            // At end of analysis in stack should be only one operand (result)
            if (Stack.Count > 1) throw new ArgumentException("Excess operand");

            return Stack.Pop();
        }

        /// <summary>
        /// Syntax analysis of reverse-polish notation
        /// </summary>
        /// <param name="Stack">Stack which contains operands</param>
        /// <param name="Token">Token</param>
        /// <returns>Stack which contains operands</returns>
        void SyntaxAnalysisRPN(Stack<double> Stack, Token Token)
        {
            // if it's operand then just push it to stack
            if (Token.IsNumber)
                Stack.Push((Constant)Token);

            else if (Token.IsVariable)
                Stack.Push(((Variable)Token).Value);

            // Otherwise apply operator or function to elements in stack
            else if (Token.IsUnaryOperator)
                Stack.Push((Token as UnaryOperator).Invoke(Stack.Pop()));

            else if (Token.IsTrigonometricFunction)
                Stack.Push((Token as TrigonometricFunction).Invoke(Stack.Pop(), AngleType));

            else if (Token.IsUnaryFunction || Token.IsPostfixFunction)
                Stack.Push((Token as UnaryFunction).Invoke(Stack.Pop()));

            else if (Token.IsBinaryOperator)
            {
                double Argument2 = Stack.Pop();
                double Argument1 = Stack.Pop();

                Stack.Push((Token as BinaryOperator).Invoke(Argument1, Argument2));
            }

            else if (Token.IsBinaryFuncion)
            {
                double Argument2 = Stack.Pop();
                double Argument1 = Stack.Pop();

                Stack.Push((Token as BinaryFunction).Invoke(Argument1, Argument2));
            }

            else if (Token.IsTernaryFuncion)
            {
                double Argument3 = Stack.Pop();
                double Argument2 = Stack.Pop();
                double Argument1 = Stack.Pop();

                Stack.Push((Token as TernaryFunction).Invoke(Argument1, Argument2, Argument3));
            }
        }
    }
}