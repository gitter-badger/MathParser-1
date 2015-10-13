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

        #region Type Validation
        public bool IsNumber { get { return GetType() == typeof(Constant); } }

        public bool IsVariable { get { return GetType() == typeof(Variable); } }

        public bool IsOperator { get { return GetType().IsSubclassOf(typeof(Operator)); } }

        public bool IsUnaryOperator { get { return GetType() == typeof(UnaryOperator); } }

        public bool IsBinaryOperator { get { return GetType() == typeof(BinaryOperator); } }

        public bool IsUnaryFunction { get { return GetType() == typeof(UnaryFunction); } }

        public bool IsPostfixFunction { get { return GetType() == typeof(PostfixFunction); } }

        public bool IsBinaryFuncion { get { return GetType() == typeof(BinaryFunction); } }

        public bool IsTernaryFuncion { get { return GetType() == typeof(TernaryFunction); } }

        public bool IsFunction { get { return GetType().IsSubclassOf(typeof(Function)); } }

        public bool IsTrigonometricFunction { get { return GetType() == typeof(TrigonometricFunction); } }
        #endregion

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