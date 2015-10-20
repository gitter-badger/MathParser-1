using System;
using System.Collections.Generic;
using System.Collections;

namespace MathParsing
{
    public class TokenDictionary<T> : Dictionary<string, T>
    {
        internal Predicate<string> Validation;
        internal string ErrorMessage;

        public TokenDictionary(Predicate<string> Validation, string ErrorMessage)
        {
            this.Validation = Validation;
            this.ErrorMessage = ErrorMessage;
        }

        public new void Add(string Keyword, T Item)
        {
            if (!Validation(Keyword)) throw new KeywordFormatException(ErrorMessage);

            base.Add(Keyword, Item);
        }

        // To be used for Instances
        public BranchedTokenDictionary<T> Branch() { return new BranchedTokenDictionary<T>(this); }
    }

    public class BranchedTokenDictionary<T> : TokenDictionary<T>
    {
        TokenDictionary<T> Parent;

        public BranchedTokenDictionary(TokenDictionary<T> Parent)
            : base(Parent.Validation, Parent.ErrorMessage)
        {
            this.Parent = Parent;
        }

        public new bool ContainsKey(string Key)
        {
            return base.ContainsKey(Key) || Parent.ContainsKey(Key);
        }

        public new T this[string Key]
        {
            get
            {
                if (base.ContainsKey(Key)) return base[Key];
                else return Parent[Key];
            }
        }

        public new int Count { get { return base.Count + Parent.Count; } }
    }
}
