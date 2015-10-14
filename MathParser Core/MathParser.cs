using System;
using System.Collections.Generic;
using System.Globalization;
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
    }
}