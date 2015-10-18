namespace MathParsing
{
    sealed class Punctuation : Token
    {
        Punctuation(string Keyword) : base(Keyword, 0) { }

        public static readonly Punctuation LeftParenthesis = new Punctuation("("),
            RightParenthesis = new Punctuation(")"),
            Comma = new Punctuation(",");
    }
}
