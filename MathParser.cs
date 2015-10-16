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

        public readonly List<Operator> Operators = new List<Operator>();

        public readonly List<Function> Functions = new List<Function>();

        public readonly List<Variable> Variables = new List<Variable>();
        #endregion

        #region TokenSearch
        bool IsOperatorDefined(string Keyword)
        {
            foreach (var Operator in EnumerateOperators())
                if (Operator.Keyword == Keyword)
                    return true;

            return false;
        }

        IEnumerable<Operator> EnumerateOperators() { return CommonTokens.Operators.Union(Operators).Reverse(); }

        Operator FindOperator(string Keyword)
        {
            foreach (var Op in EnumerateOperators())
                if (Op.Keyword == Keyword)
                    return Op;

            throw new FormatException("Token not defined or Invalid Usage as Unary Operator");
        }

        bool IsFunctionDefined(string Keyword)
        {
            foreach (var Function in EnumerateFunctions())
                if (Function.Keyword == Keyword)
                    return true;

            return false;
        }

        IEnumerable<Function> EnumerateFunctions() { return CommonTokens.Functions.Union(Functions).Reverse(); }

        Function FindFunction(string Keyword)
        {
            foreach (var Function in EnumerateFunctions())
                if (Function.Keyword == Keyword) return Function;

            throw new ArgumentException("Invalid Function Token");
        }

        bool IsVariableDefined(string Keyword)
        {
            foreach (var Variable in EnumerateVariable())
                if (Variable.Keyword == Keyword)
                    return true;

            return false;
        }

        IEnumerable<Variable> EnumerateVariable() { return ((IEnumerable<Variable>)Variables).Reverse(); }

        Variable FindVariable(string Keyword)
        {
            foreach (var Variable in EnumerateVariable())
                if (Variable.Keyword == Keyword) return Variable;

            throw new ArgumentException("Undefined Variable");
        }
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
        public void Parse(string Expression) { RPNExpression = RPNGenerator.Generate(ParseInfix(FormatString(Expression))); }

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

            return FormattedString.ToString();
        }

        List<Token> ParseInfix(string Expression)
        {
            int Position = 0;
            var Infix = new List<Token>();

            while (Position < Expression.Length)
            {
                // Receive the first char
                StringBuilder Word = new StringBuilder();
                Word.Append(Expression[Position]);

                // If it is an operator
                if (IsOperatorDefined(Word.ToString()))
                    Infix.Add(ParseOperator(Expression, ref Position, Word));

                else if (Char.IsLetter(Word[0]))
                {
                    // Read function or constant name
                    while (++Position < Expression.Length && Char.IsLetter(Expression[Position]))
                        Word.Append(Expression[Position]);

                    if (IsFunctionDefined(Word.ToString()))
                        Infix.Add(FindFunction(Word.ToString()));
                    else if (CommonTokens.Constants.ContainsKey(Word.ToString()))
                        Infix.Add(CommonTokens.Constants[Word.ToString()]);
                    else if (IsVariableDefined(Word.ToString()))
                        Infix.Add(FindVariable(Word.ToString()));
                    else throw new ArgumentException("Unknown token");
                }

                // Read number
                else if (Char.IsDigit(Word[0]) || Word[0] == DecimalSeparator)
                    Infix.Add(ParseNumber(Expression, ref Position, Word));

                else
                {
                    while (++Position < Expression.Length
                        && !Char.IsLetterOrDigit(Expression[Position])
                        && Expression[Position] != '(')
                        Word.Append(Expression[Position]);

                    if (IsOperatorDefined(Word.ToString()))
                        Infix.Add(FindOperator(Word.ToString()));

                    else if (IsFunctionDefined(Word.ToString()))
                        Infix.Add(FindFunction(Word.ToString()));

                    else if (CommonTokens.Constants.ContainsKey(Word.ToString()))
                        Infix.Add(CommonTokens.Constants[Word.ToString()]);

                    else if (IsVariableDefined(Word.ToString()))
                        Infix.Add(FindVariable(Word.ToString()));

                    else throw new ArgumentException("Unknown token in expression");
                }
            }

            return Infix;
        }

        Operator ParseOperator(string Expression, ref int Position, StringBuilder Word)
        {
            if (Word[0] == '(' || Word[0] == ')' || Word[0] == ',')
            {
                ++Position;
                return FindOperator(Word.ToString());
            }

            else if (!Char.IsLetterOrDigit(Word[0])
                && !Char.IsLetterOrDigit(Expression[Position + 1])
                && Expression[Position + 1] != '(')
            {
                while (++Position < Expression.Length
                    && !Char.IsLetterOrDigit(Expression[Position])
                    && Expression[Position] != '(')
                    Word.Append(Expression[Position]);

                if (IsOperatorDefined(Word.ToString()))
                    return FindOperator(Word.ToString());

                else throw new ArgumentException("Unknown token in expression");
            }
            else
            {
                // Determine whether it is unary or binary operator
                bool IsUnary = Position == 0 || Expression[Position - 1] == '(';
                Position++;

                if (IsUnary)
                {
                    foreach (var Op in EnumerateOperators())
                        if (Op.IsUnaryOperator && Op.Keyword == Word.ToString())
                            return Op;

                    throw new FormatException("Token not defined or Invalid Usage as Unary Operator");
                }
                else
                {
                    foreach (var Op in EnumerateOperators())
                        if (!Op.IsUnaryOperator && Op.Keyword == Word.ToString())
                            return Op;

                    throw new FormatException("Token not defined");
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
                return Convert.ToDouble(Word.ToString());
            }

            return Convert.ToDouble(Word.ToString());
        }
    }
}