using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MathParsing
{
    public class MathParser
    {
        #region Static
        static readonly Operator LeftParenthesis = new Operator("("),
            RightParenthesis = new Operator(")"),
            Comma = new Operator(",");

        static readonly UnaryOperator UnaryPlus = new UnaryOperator("un+", 6, (x) => x),
            UnaryMinus = new UnaryOperator("un-", 6, (x) => -x);

        static readonly BinaryOperator Plus = new BinaryOperator("+", 2, (x, y) => x + y),
                Minus = new BinaryOperator("-", 2, (x, y) => x - y);

        static readonly List<Operator> DefaultOperators =
            new List<Operator>
            {
                LeftParenthesis,
                RightParenthesis,
                Comma,
                UnaryPlus,
                UnaryMinus,
                Plus,
                Minus,
                new BinaryOperator("*", 4, (x, y) => x * y),
                new BinaryOperator("/", 4, (x, y) => x / y),
                new BinaryOperator("^", 8, Math.Pow),
                new BinaryOperator("%", 8, (x, y) => x % y)
            };

        static readonly List<Function> DefaultFunctions =
            new List<Function>
            {
                new UnaryFunction("sqrt", Math.Sqrt),
                new TrigonometricFunction("sin", Math.Sin),
                new TrigonometricFunction("cos", Math.Cos),
                new TrigonometricFunction("tan", Math.Tan),
                new TrigonometricFunction("sec", (x) => 1 / Math.Cos(x)),
                new TrigonometricFunction("cosec", (x) => 1 / Math.Sin(x)),
                new TrigonometricFunction("cot", (x) => 1 / Math.Tan(x)),
                new UnaryFunction("asin", Math.Asin),
                new UnaryFunction("acos", Math.Acos),
                new UnaryFunction("atan", Math.Atan),
                new UnaryFunction("sinh", Math.Sinh),
                new UnaryFunction("cosh", Math.Cosh),
                new UnaryFunction("tanh", Math.Tanh),
                new UnaryFunction("ln", Math.Log),
                new UnaryFunction("exp", Math.Exp),
                new UnaryFunction("abs", Math.Abs),
                new UnaryFunction("floor", Math.Floor),
                new UnaryFunction("ceiling", Math.Ceiling),
                new UnaryFunction("round", Math.Round),
                new UnaryFunction("sign", (x) => Math.Sign(x)),
                new UnaryFunction("truncate", Math.Truncate),
                new UnaryFunction("factorial", (x) => Combinatorics.Factorial((int)x)),
                new BinaryFunction("log", Math.Log),
                new BinaryFunction("max", Math.Max),
                new BinaryFunction("min", Math.Min),
                new BinaryFunction("c", (x, y) => Combinatorics.C((int)x, (int)y)),
                new BinaryFunction("p", (x, y) => Combinatorics.P((int)x, (int)y)),
                new TernaryFunction("clip", (val, min, max) =>
                    {
                        if (val < min) return min;
                        else if (val > max) return max;
                        else return val;
                    }),
                new PostfixFunction("!", (x) => Combinatorics.Factorial((int)x))
            };

        static readonly Dictionary<string, Constant> Constants =
            new Dictionary<string, Constant>
            {
                {"pi", Math.PI },
                {"e", Math.E }
            };        
        #endregion

        #region Fields
        List<Token> RPNExpression;

        public char DecimalSeparator { get; set; }

        public AngleType AngleType { get; set; }

        public readonly List<Operator> Operators = new List<Operator>();

        public readonly List<Function> Functions = new List<Function>();

        public readonly List<Variable> Variables = new List<Variable>();
        #endregion

        #region Find Token
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
        #endregion

        /// <summary>
        /// Initialize new instance of MathParser
        /// (Decimal Separator symbol is read from regional settings in system)
        /// </summary>
        public MathParser()
        {
            try { DecimalSeparator = Char.Parse(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator); }
            catch (FormatException) { DecimalSeparator = '.'; }

            AngleType = AngleType.Radians;
        }

        /// <summary>
        /// Parses the given math expression and Caches it in RPN
        /// </summary>
        /// <param name="Expression">Math expression (infix/standard notation)</param>
        public void Parse(string Expression) { RPNExpression = ConvertToRPN(FormatString(Expression)); }

        public double Evaluate() { return CalculateRPN(RPNExpression); }

        public double Evaluate(string Expression)
        {
            Parse(Expression);
            return Evaluate();
        }

        /// <summary>
        /// Produce formatted string from the given string
        /// </summary>
        string FormatString(string Expression)
        {
            if (string.IsNullOrEmpty(Expression)) throw new ArgumentNullException("Expression is null or empty");

            StringBuilder FormattedString = new StringBuilder();
            int UnbalancedParanthesis = 0; // Check number of parenthesis

            // Format String and check Number of Parenthesis in one Iteration
            // This function does 2 Tasks because of Performance Priority
            foreach (char ch in Expression)
            {
                if (ch == '(') UnbalancedParanthesis++;
                else if (ch == ')') UnbalancedParanthesis--;

                if (Char.IsWhiteSpace(ch)) continue;
                else if (Char.IsUpper(ch)) FormattedString.Append(Char.ToLower(ch));
                else FormattedString.Append(ch);
            }

            if (UnbalancedParanthesis != 0)
                throw new FormatException("Number of left and right parenthesis is not equal");

            return FormattedString.ToString().Replace(")(", ")*(");
        }

        #region Infix to RPN
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
        #endregion

        #region Calculate RPN Expression
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
        #endregion
    }
}