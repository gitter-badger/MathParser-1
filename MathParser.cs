using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MathParsing
{
    public partial class MathParser
    {
        #region Fields
        public List<Token> RPNExpression { get; private set; }

        public char DecimalSeparator { get; set; }

        public AngleType AngleType { get; set; }

        public readonly BranchedTokenDictionary<Operator> Operators = CommonTokens.Operators.Branch();

        public readonly BranchedTokenDictionary<Function> Functions = CommonTokens.Functions.Branch();

        public readonly TokenDictionary<Variable> Variables
            = new TokenDictionary<Variable>((S) =>
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
            });
        #endregion

        /// <summary>
        /// Initialize new instance of MathParser
        /// (Decimal Separator symbol is read from regional settings in system)
        /// </summary>
        public MathParser(AngleType AngleType = AngleType.Radians)
        {
            try { DecimalSeparator = Char.Parse(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator); }
            catch (FormatException) { DecimalSeparator = '.'; }

            this.AngleType = AngleType;
        }

        /// <summary>
        /// Parses the given math expression and Caches it in RPN
        /// </summary>
        /// <param name="Expression">Math expression (infix/standard notation)</param>
        public void Parse(string Expression) { RPNExpression = RPNGenerator.Generate(Tokenize(FormatString(Expression))); }

        public double Evaluate()
        {
            if (RPNExpression == null) throw new ArgumentNullException("RPN has not been Generated");

            return RPNEvaluator.Evaluate(RPNExpression);
        }

        public double Evaluate(string Expression)
        {
            Parse(Expression);
            return Evaluate();
        }

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

        GeneratedMethod GenerateMethod(params string[] Parameters)
        {
            foreach (var Param in Parameters)
                if (!Variables.ContainsKey(Param))
                    throw new ArgumentException("Use of Undefined Variable");

            return (VariableValues) =>
                {
                    for (int i = 0; i < Parameters.Length; ++i)
                        Variables[Parameters[i]].Value = VariableValues[i];

                    return Evaluate();
                };
        }

        List<Token> Tokenize(string Expression)
        {
            int Position = 0;
            var Infix = new List<Token>();

            while (Position < Expression.Length)
            {
                // Receive the first char
                StringBuilder Word = new StringBuilder();
                Word.Append(Expression[Position]);

                if (Word[0].Is('(', ',', ')')) ++Position;

                if (Word[0] == '(')
                {
                    if (Infix.Count > 0)
                    {
                        var Last = Infix.Last();
                        if (Last is Constant || Last is Variable) Infix.Add(CommonTokens.Multiply);
                    }
                    Infix.Add(Punctuation.LeftParenthesis);
                }

                else if (Word[0] == ',') Infix.Add(Punctuation.Comma);
                else if (Word[0] == ')') Infix.Add(Punctuation.RightParenthesis);

                // If it is an operator
                else if (Operators.ContainsKey(Word.ToString()))
                    Infix.Add(ParseOperator(Expression, ref Position, Word));

                else if (Char.IsLetter(Word[0]))
                {
                    // Read function or constant name
                    while (++Position < Expression.Length && Char.IsLetter(Expression[Position]))
                        Word.Append(Expression[Position]);

                    // Function
                    if (Functions.ContainsKey(Word.ToString()))
                    {
                        var Function = Functions[Word.ToString()];

                        if (!(Function is PostfixFunction) && Infix.Count > 0 && Infix.Last() is Constant) Infix.Add(CommonTokens.Multiply);
                        Infix.Add(Function);
                    }

                    // Variable
                    else if (Variables.ContainsKey(Word.ToString()))
                    {
                        if (Infix.Count > 0 && Infix.Last() is Constant) Infix.Add(CommonTokens.Multiply);
                        Infix.Add(Variables[Word.ToString()]);
                    }

                    // Constant
                    else if (CommonTokens.Constants.ContainsKey(Word.ToString()))
                        Infix.Add(CommonTokens.Constants[Word.ToString()]);

                    else throw new ArgumentException("Unknown token");
                }

                // Read number
                else if (Char.IsDigit(Word[0]) || Word[0] == DecimalSeparator)
                    Infix.Add(ParseNumber(Expression, ref Position, Word));

                else if (Functions.ContainsKey(Word.ToString()))
                {
                    var Function = Functions[Word.ToString()];

                    if (!(Function is PostfixFunction) && Infix.Count > 0 && Infix.Last() is Constant) Infix.Add(CommonTokens.Multiply);
                    ++Position;
                    Infix.Add(Function);
                }

                else if (Variables.ContainsKey(Word.ToString()))
                {
                    if (Infix.Count > 0 && Infix.Last() is Constant) Infix.Add(CommonTokens.Multiply);
                    ++Position;
                    Infix.Add(Variables[Word.ToString()]);
                }

                else if (CommonTokens.Constants.ContainsKey(Word.ToString()))
                {
                    ++Position;
                    Infix.Add(CommonTokens.Constants[Word.ToString()]);
                }

                else
                {
                    while (++Position < Expression.Length
                        && !Char.IsLetterOrDigit(Expression[Position])
                        && !Expression[Position].Is('(', ')', ','))
                        Word.Append(Expression[Position]);

                    if (Operators.ContainsKey(Word.ToString()))
                        Infix.Add(Operators[Word.ToString()]);

                    else throw new ArgumentException("Unknown token in expression");
                }
            }

            return Infix;
        }

        Token ParseOperator(string Expression, ref int Position, StringBuilder Word)
        {
            if (!Char.IsLetterOrDigit(Word[0])
                && !Char.IsLetterOrDigit(Expression[Position + 1])
                && !Expression[Position + 1].Is('(', ')', ','))
            {
                while (++Position < Expression.Length
                    && !Char.IsLetterOrDigit(Expression[Position])
                    && !Expression[Position].Is('(', ')', ','))
                    Word.Append(Expression[Position]);

                if (Operators.ContainsKey(Word.ToString()))
                    return Operators[Word.ToString()];

                else throw new ArgumentException("Unknown token in expression");
            }
            else
            {
                // Determine whether it is unary or binary operator
                bool IsUnary = Position == 0 || Expression[Position - 1] == '(';
                Position++;

                switch (Word.ToString())
                {
                    case "+":
                        return IsUnary ? (Operator)CommonTokens.UnaryPlus : CommonTokens.Plus;
                    case "-":
                        return IsUnary ? (Operator)CommonTokens.UnaryMinus : CommonTokens.Minus;
                    default:
                        return Operators[Word.ToString()];
                }
            }
        }

        Constant ParseNumber(string Expression, ref int Position, StringBuilder Word)
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
                        && Expression[Position + 1].Is('+', '-')
                                && Char.IsDigit(Expression[Position + 2]))))
            {
                Word.Append(Expression[Position++]); // e

                if (Expression[Position].Is('+', '-'))
                    Word.Append(Expression[Position++]); // sign

                while (Position < Expression.Length && Char.IsDigit(Expression[Position]))
                    Word.Append(Expression[Position++]); // power

                // Convert number from scientific notation to decimal notation
                return Convert.ToDouble(Word.ToString());
            }

            return Convert.ToDouble(Word.ToString());
        }
    }
}