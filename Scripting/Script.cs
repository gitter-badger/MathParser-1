using System;
using System.Collections.Generic;
using System.Linq;

namespace MathParsing.Scripting
{
    /// <summary>
    /// ALPHA
    /// 
    /// Support for Script-Like Execution.
    /// Statements should be separated by either NewLine or ';'
    /// 
    /// var X = 3; // Variable Declaration
    /// 
    /// X += 9; // Variable Manipulation
    /// 
    /// return X*8; // Returning Value
    /// 
    /// If No Return Statement is present, Double.NaN is returned
    /// </summary>
    public class Script
    {
        MathParser P = new MathParser();

        public AngleType AngleType { get { return P.AngleType; } set { P.AngleType = value; } }

        public char DecimalSeparator { get { return P.DecimalSeparator; } set { P.DecimalSeparator = value; } }

        public double Run(string Code)
        {
            Stack<string> Statements = new Stack<string>();

            foreach (string Statement in Code.Split("\n;".ToCharArray()).Reverse())
            {
                if (string.IsNullOrWhiteSpace(Statement)) continue;
                Statements.Push(Statement.Trim());
            }

            while (Statements.Count > 0)
            {
                string Statement = Statements.Pop();

                if (Statement.StartsWith("var"))
                    DeclareVariable(Statement);
                else if (Statement.StartsWith("return"))
                    return P.Evaluate(Statement.Remove(0, 6));
                else
                {
                    //Read Variable Name
                    string VarName = null;

                    while (Char.IsLetter(Statement[0]))
                    {
                        VarName += Statement[0];
                        Statement = Statement.Remove(0, 1);
                    }

                    Statement = Statement.Trim();

                    if (!P.IsDefined(VarName, P.EnumerateVariables())) throw new Exception("Unsupported Statement");

                    string Operator = null;

                    while (Statement[0] != '=')
                    {
                        Operator += Statement[0];
                        Statement = Statement.Remove(0, 1);
                    }

                    Operator += '=';
                    Statement = Statement.Remove(0, Statement.IndexOf('=') + 1);

                    ScriptingOperators.VariableAssignmentOperators[Operator]
                        .Invoke(P.Variables.Find((V) => V.Keyword == VarName), P.Evaluate(Statement));
                }
            }

            return double.NaN;
        }

        void DeclareVariable(string Statement)
        {
            Statement = Statement.Remove(0, 3).Trim();

            string VarName = null;

            while (Char.IsLetter(Statement[0]))
            {
                VarName += Statement[0];
                Statement = Statement.Remove(0, 1);
            }

            Statement = Statement.Remove(0, Statement.IndexOf('=') + 1);

            P.Variables.Add(new Variable(VarName, P.Evaluate(Statement)));
        }
    }
}
