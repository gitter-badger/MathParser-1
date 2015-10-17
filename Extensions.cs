using System;

namespace MathParsing
{
    static class Extensions
    {
        public static bool Is<T>(this T Value, params T[] Candidates)
        {
            foreach (var t in Candidates)
                if (Value.Equals(t)) return true;

            return false;
        }

        public static bool DerivesFrom<T>(this T Obj, params Type[] Types) 
        {
            foreach (var Type in Types) 
                if (Obj.GetType().IsSubclassOf(Type)) return true;

            return false;
        }

        public static bool Is<T>(this T Obj, params Type[] Types)
        {
            foreach (var Type in Types)
                if (Obj.GetType() == Type) return true;

            return false;
        }
    }
}
