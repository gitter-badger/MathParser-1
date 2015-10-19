using System;
using System.Collections.Generic;

namespace MathParsing.Scripting
{
    static class ScriptingOperators
    {
        public static readonly Dictionary<string, Action<Variable, double>> VariableAssignmentOperators = 
            new Dictionary<string, Action<Variable, double>>()
            {
                { "=", (V, Val) => V.Value = Val },
                { "+=", (V, Val) => V.Value += Val },
                { "-=", (V, Val) => V.Value -= Val },
                { "/=", (V, Val) => V.Value /= Val },
                { "*=", (V, Val) => V.Value *= Val },
                { "%=", (V, Val) => V.Value %= Val }
            };

        public static readonly Dictionary<string, Action<Variable>> VariableShorthandOperators =
            new Dictionary<string, Action<Variable>>()
            {
                { "++", (V) => V.Value++ },
                { "--", (V) => V.Value-- }
            };
    }
}