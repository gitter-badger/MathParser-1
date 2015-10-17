using System;
using System.Collections.Generic;

namespace MathParsing
{
    public static class RPNGenerator
    {
        public static List<Token> Generate(List<Token> Input)
        {
            List<Token> Output = new List<Token>();
            Stack<Token> Stack = new Stack<Token>();

            foreach (var Token in Input)
            {
                if (Token == Punctuation.Comma)
                {
                    Token Peek;
                    while (Stack.Count > 0)
                    {
                        Peek = Stack.Peek();

                        if (Peek == Punctuation.LeftParenthesis) break;
                        if (Peek == Punctuation.Comma)
                        {
                            Stack.Pop();
                            break;
                        }

                        Output.Add(Stack.Pop());
                    }

                    Stack.Push(Punctuation.Comma);
                }

                // If it's a number just put to list           
                else if (Token.IsNumber || Token.IsVariable) Output.Add(Token);

                // if it's a function push to stack
                else if (Token.IsFunction)
                {
                    if (Token.IsPostfixFunction) Output.Add(Token);
                    else Stack.Push(Token);
                }

                // If its '(' push to stack
                else if (Token == Punctuation.LeftParenthesis) Stack.Push(Token);

                else if (Token == Punctuation.RightParenthesis)
                {
                    // If its ')' pop elements from stack to output list
                    // until find the '('
                    Token Element;
                    while ((Element = Stack.Pop()) != Punctuation.LeftParenthesis) Output.Add(Element);

                    // if after this a function is in the peek of stack then put it to list
                    if (Stack.Count > 0 && Stack.Peek().IsFunction)
                        Output.Add(Stack.Pop());
                }
                else
                {
                    // While priority of elements at peek of stack >= (>) token's priority
                    // put these elements to output list
                    while (Stack.Count > 0 && (Token <= Stack.Peek()))
                        Output.Add(Stack.Pop());

                    Stack.Push(Token);
                }
            }

            // Pop all elements from stack to output string            
            while (Stack.Count > 0)
            {
                // There should be only operators
                if (Stack.Peek().IsOperator) Output.Add(Stack.Pop());
                else throw new FormatException("Format exception, there is function without parenthesis");
            }

            return Output;
        }
    }
}
