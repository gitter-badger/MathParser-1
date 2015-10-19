using System;
using MathParsing.Properties;

namespace MathParsing
{
    public sealed class Variable : Token
    {
        public double Value { get; set; }

        public Variable(double Value = 0) : base(0) { this.Value = Value; }
    }
}