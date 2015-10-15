﻿using System;
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

        RPNEvaluator Evaluator;

        public char DecimalSeparator { get; set; }

        public AngleType AngleType { get { return Evaluator.AngleType; } set { Evaluator.AngleType = value; } }

        public readonly List<Operator> Operators = new List<Operator>();

        public readonly List<Function> Functions = new List<Function>();

        public readonly List<Variable> Variables = new List<Variable>();
        #endregion

        #region TokenSearch
        bool IsOperatorDefined(string Keyword)
        {
            foreach (var Operator in CommonTokens.Operators.Union(Operators))
                if (Operator.Keyword == Keyword)
                    return true;

            return false;
        }

        bool IsFunctionDefined(string Keyword)
        {
            foreach (var Function in CommonTokens.Functions.Union(Functions))
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
            foreach (var Operator in CommonTokens.Operators.Union(Operators))
                if (Operator.Keyword == Keyword) return Operator;

            throw new ArgumentException("Invalid Operator Token");
        }

        Function FindFunction(string Keyword)
        {
            foreach (var Function in CommonTokens.Functions.Union(Functions))
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
        public MathParser(AngleType AngleType = AngleType.Radians)
        {
            try { DecimalSeparator = Char.Parse(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator); }
            catch (FormatException) { DecimalSeparator = '.'; }

            Evaluator = new RPNEvaluator(AngleType);
        }

        /// <summary>
        /// Parses the given math expression and Caches it in RPN
        /// </summary>
        /// <param name="Expression">Math expression (infix/standard notation)</param>
        public void Parse(string Expression) { RPNExpression = ConvertToRPN(FormatString(Expression)); }

        public double Evaluate()
        {
            if (RPNExpression == null) throw new ArgumentNullException("RPN has not been Generated");

            return Evaluator.Evaluate(RPNExpression);
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

        List<Token> ConvertToRPN(string Expression) { return RPNGenerator.Generate(ParseInfix(Expression)); }

        List<Token> ParseInfix(string Expression)
        {
            int Position = 0;
            var Infix = new List<Token>();

            while (Position < Expression.Length)
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
                            Infix.Add(IsUnary ? (Operator)CommonTokens.UnaryPlus : CommonTokens.Plus);
                            break;
                        case "-":
                            Infix.Add(IsUnary ? (Operator)CommonTokens.UnaryMinus : CommonTokens.Minus);
                            break;
                        case ",":
                            Infix.Add(CommonTokens.Comma);
                            break;
                        default:
                            Infix.Add(FindOperator(Word.ToString()));
                            break;
                    }
                }
                else if (Char.IsLetter(Word[0]) || IsFunctionDefined(Word.ToString())
                            || CommonTokens.Constants.ContainsKey(Word.ToString()) || IsVariableDefined(Word.ToString()))
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
                        Infix.Add((Constant)Convert.ToDouble(Word.ToString()));
                    }

                    Infix.Add((Constant)Convert.ToDouble(Word.ToString()));
                }
                else throw new ArgumentException("Unknown token in expression");
            }

            return Infix;
        }
    }
}