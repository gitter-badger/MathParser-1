namespace MathParsing
{
    static class Extensions
    {
        public static bool Is<T>(this T Value, params T[] Candidates)
        {
            if (Candidates == null) return false;

            foreach (var t in Candidates) if (Value.Equals(t)) return true;

            return false;
        }
    }
}
