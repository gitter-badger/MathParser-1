public static class Combinatorics
{
    public static double Factorial(int n)
    {
        if (n < 0) return double.NaN;
        else if (n == 0) return 1;
        else
        {
            double Product = 1;
            for (int i = 1; i <= n; ++i) Product *= i;
            return Product;
        }
    }

    public static double C(int n, int r)
    {
        if (r < 0 | n < 0 | n < r) return double.NaN;
        else if (n - r == 1 | r == 1) return n;
        else if (n == r | r == 0) return 1;
        else if (n - r > r) return (P(n, n - r) / Factorial(n - r));
        else return (P(n, r) / Factorial(r));
    }

    public static double P(int n, int r)
    {
        if (r < 0 | n < 0 | n < r) return double.NaN;
        else if (r == 0) return 1;
        else if (n == r) return Factorial(n);
        else
        {
            double Product = 1;
            for (int i = n - r + 1; i <= n; ++i) Product *= i;
            return Product;
        }
    }
}