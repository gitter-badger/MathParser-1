namespace MathParsing
{
    public abstract class Token
    {
        #region Hashing
        static int LastHashCode = 0;

        int HashCode;

        public override int GetHashCode() { return HashCode; }
        #endregion

        public int Priority { get; private set; }

        protected Token(int Priority)
        {
            this.Priority = Priority;
            LastHashCode++;
            HashCode = LastHashCode;
        }

        public override bool Equals(object obj) { return (obj is Token) ? (obj as Token).HashCode == HashCode : false; }

        public static bool IsRightAssociated(Operator Op) { return Op == CommonTokens.Power; }

        public static bool operator <=(Token T1, Token T2)
        {
            return (T1 is Operator && IsRightAssociated(T1 as Operator)) 
                ? T1.Priority < T2.Priority : T1.Priority <= T2.Priority;
        }

        public static bool operator >=(Token T1, Token T2) { return T1.Priority >= T2.Priority; }
    }
}