using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace MathParsing
{
    public class InvalidTokenNameException : Exception { }

    public class TokenDictionary<T> : Dictionary<string, T>
    {
        internal Predicate<string> Validation;

        public TokenDictionary(Predicate<string> Validation) { this.Validation = Validation; }

        public new void Add(string Keyword, T Item)
        {
            if (!Validation(Keyword)) throw new InvalidTokenNameException();

            base.Add(Keyword, Item);
        }

        // To be used for Instances
        public BranchedTokenDictionary<T> Branch() { return new BranchedTokenDictionary<T>(this); }
    }

    public class BranchedTokenDictionary<T> : TokenDictionary<T>
    {
        TokenDictionary<T> Parent;

        public BranchedTokenDictionary(TokenDictionary<T> Parent)
            : base(Parent.Validation)
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

        public IEnumerable<KeyValuePair<string, T>> Items { get { return Parent.Union(this); } }
    }
}
