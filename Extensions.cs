﻿using System;

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
    }
}
