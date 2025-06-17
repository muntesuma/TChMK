using System;

class PollardsRhoMethod
{
    //f(x) = (x^2 + 1) mod n
    static long F(long x, long n)
    {
        return ((x * x) + 1) % n;
    }

    // НОД (алгоритм Евклида)
    static long GCD(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    static long PollardsRho(long n)
    {
        // 0
        if (n == 1) return n;
        if (n % 2 == 0) return 2; 

        long a = 2, b = 2, d = 1; // 1

        while (d == 1)
        {
            // 2
            a = F(a, n);
            b = F(F(b, n), n);

            if (a == b) return -1; // 3

            d = GCD(Math.Abs(a - b), n); // 4
        }

        return d;
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Enter the composite number n: ");
        long n;
        while (!long.TryParse(Console.ReadLine(), out n) || n <= 0)
            Console.WriteLine("Error! Enter a positive integer: ");

        long divisor = PollardsRho(n);

        if (divisor == -1)
            Console.WriteLine("It was not possible to find a non-trivial divider");
        else if (divisor == n)
            Console.WriteLine("Only a trivial divider (the number itself) was found");
        else
            Console.WriteLine($"Non-trivial divisor was found: {divisor}");
    }
}