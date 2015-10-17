namespace MathParsing
{
    public abstract class Token
    {
        public int Priority { get; private set; }

        public string Keyword { get; private set; }

        protected Token(string Keyword, int Priority)
        {
            this.Keyword = Keyword.ToLower();
            this.Priority = Priority;
        }

        public override string ToString() { return Keyword; }

        public override bool Equals(object obj)
        {
            return (obj is Token) ? (obj as Token).Keyword == Keyword : false;
        }

        public static bool operator <=(Token T1, Token T2)
        {
            return T1.Keyword == "^" ? T1.Priority < T2.Priority : T1.Priority <= T2.Priority;
        }

        public static bool operator >=(Token T1, Token T2) { return T1.Priority >= T2.Priority; }
    }
}