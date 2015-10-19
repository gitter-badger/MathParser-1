namespace MathParsing
{
    sealed class Punctuation : Token
    {
        string Word;

        Punctuation(string Word) : base(0) { this.Word = Word; }

        public static readonly Punctuation LeftParenthesis = new Punctuation("("),
            RightParenthesis = new Punctuation(")"),
            Comma = new Punctuation(",");
    }
}
