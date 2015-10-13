using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MathParsing
{
    public partial class MathParser
    {
        /// <summary>
        /// Produce math expression in reverse polish notation
        /// by the given string
        /// </summary>
        /// <param name="Expression">Math expression in infix notation</param>
        /// <returns>Math expression in postfix notation (RPN)</returns>
        List<Token> ConvertToRPN(string Expression)
        {
            int Position = 0; // Current position of lexical analysis
            var RPNOutput = new List<Token>();
            var Stack = new Stack<Token>();

            // While there is unhandled char in expression
            while (Position < Expression.Length)
                SyntaxAnalysisInfixNotation(LexicalAnalysisInfixNotation(Expression, ref Position),
                    RPNOutput, Stack);

            // Pop all elements from stack to output string            
            while (Stack.Count > 0)
            {
                // There should be only operators
                if (Stack.Peek().IsOperator) RPNOutput.Add(Stack.Pop());
                else throw new FormatException("Format exception, there is function without parenthesis");
            }

            return RPNOutput;
        }

        /// <summary>
        /// Produce token by the given math expression
        /// </summary>
        /// <param name="Expression">Math expression in infix notation</param>
        /// <param name="Position">Current position in string for lexical analysis</param>
        Token LexicalAnalysisInfixNotation(string Expression, ref int Position)
        {
            // Receive first char
            StringBuilder Word = new StringBuilder();
            Word.Append(Expression[Position]);

            // If it is a operator
            if (IsOperatorDefined(Word.ToString()))
            {
                // Determine it is unary or binary operator
                bool IsUnary = Position == 0 || Expression[Position - 1] == '(';
                Position++;

                switch (Word.ToString())
                {
                    case "+":
                        return IsUnary ? (Operator)UnaryPlus : Plus;
                    case "-":
                        return IsUnary ? (Operator)UnaryMinus : Minus;
                    case ",":
                        return Comma;
                    default:
                        return FindOperator(Word.ToString());
                }
            }
            else if (Char.IsLetter(Word[0]) || IsFunctionDefined(Word.ToString())
                        || Constants.ContainsKey(Word.ToString()) || IsVariableDefined(Word.ToString()))
            {
                // Read function or constant name
                while (++Position < Expression.Length && Char.IsLetter(Expression[Position]))
                    Word.Append(Expression[Position]);

                if (IsFunctionDefined(Word.ToString()))
                    return FindFunction(Word.ToString());
                else if (Constants.ContainsKey(Word.ToString()))
                    return Constants[Word.ToString()];
                else if (IsVariableDefined(Word.ToString()))
                    return FindVariable(Word.ToString());
                else throw new ArgumentException("Unknown token");
            }

            // Read number
            else if (Char.IsDigit(Word[0]) || Word[0] == DecimalSeparator)
            {
                // Read the whole part of number
                if (Char.IsDigit(Word[0]))
                    while (++Position < Expression.Length && Char.IsDigit(Expression[Position]))
                        Word.Append(Expression[Position]);

                // Because system decimal separator will be added below
                else Word.Clear();

                // Read the fractional part of number
                if (Position < Expression.Length && Expression[Position] == DecimalSeparator)
                {
                    // Add current system specific decimal separator
                    Word.Append(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

                    while (++Position < Expression.Length
                    && Char.IsDigit(Expression[Position]))
                        Word.Append(Expression[Position]);
                }

                // Read scientific notation (suffix)
                if (Position + 1 < Expression.Length && Expression[Position] == 'e'
                    && (Char.IsDigit(Expression[Position + 1])
                        || (Position + 2 < Expression.Length
                            && (Expression[Position + 1] == '+'
                                || Expression[Position + 1] == '-')
                                    && Char.IsDigit(Expression[Position + 2]))))
                {
                    Word.Append(Expression[Position++]); // e

                    if (Expression[Position] == '+' || Expression[Position] == '-')
                        Word.Append(Expression[Position++]); // sign

                    while (Position < Expression.Length && Char.IsDigit(Expression[Position]))
                        Word.Append(Expression[Position++]); // power

                    // Convert number from scientific notation to decimal notation
                    return (Constant)Convert.ToDouble(Word.ToString());
                }

                return (Constant)Convert.ToDouble(Word.ToString());
            }
            else throw new ArgumentException("Unknown token in expression");
        }

        /// <summary>
        /// Syntax analysis of infix notation
        /// </summary>
        /// <param name="Token">Token</param>
        /// <param name="OutputList">Token List (math expression in RPN)</param>
        /// <param name="Stack">Stack which contains operators (or functions)</param>
        /// <returns>Token List (math expression in RPN)</returns>
        void SyntaxAnalysisInfixNotation(Token Token, List<Token> OutputList, Stack<Token> Stack)
        {
            if (Token == Comma) return;

            // If it's a number just put to list           
            else if (Token.IsNumber || Token.IsVariable) OutputList.Add(Token);

            // if it's a function push to stack
            else if (Token.IsFunction)
            {
                if (Token.IsPostfixFunction) OutputList.Add(Token);
                else Stack.Push(Token);
            }

            // If its '(' push to stack
            else if (Token == LeftParenthesis) Stack.Push(Token);

            else if (Token == RightParenthesis)
            {
                // If its ')' pop elements from stack to output list
                // until find the '('
                Token Element;
                while ((Element = Stack.Pop()) != LeftParenthesis) OutputList.Add(Element);

                // if after this a function is in the peek of stack then put it to list
                if (Stack.Count > 0 && Stack.Peek().IsFunction)
                    OutputList.Add(Stack.Pop());
            }
            else
            {
                // While priority of elements at peek of stack >= (>) token's priority
                // put these elements to output list
                while (Stack.Count > 0 && (Token <= Stack.Peek()))
                    OutputList.Add(Stack.Pop());

                Stack.Push(Token);
            }
        }
    }
}