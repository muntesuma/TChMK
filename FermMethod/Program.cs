using System;

class FermatFactorizationMethod
{
    static bool FermatFactorization(int n, out int a, out int b)
    {
        a = b = 0;

        // 1
        int x = (int)Math.Sqrt(n);
        if (x * x == n)
        {
            a = b = x;
            return true;
        }

        // z = x² - n
        int z = x * x - n;

        while (true)
        {
            x++; // 2

            // 3
            if (x == (n + 1) / 2)
            {
                a = 1;
                b = n;
                return true;
            }

            // иначе
            z += 2 * x - 1;

            // 4
            int y = (int)Math.Round(Math.Sqrt(z));
            if (y * y == z)
            {
                a = x + y;
                b = x - y;
                return true;
            }
        }
    }

    static void Main()
    {
        Console.WriteLine("Enter an odd number n: ");
        int n = int.Parse(Console.ReadLine());

        if (n % 2 == 0)
        {
            Console.WriteLine("The number should be odd");
            return;
        }

        int a, b;
        if (FermatFactorization(n, out a, out b))
        {
            if (a == 1 && b == n)
                Console.WriteLine($"{n} is a prime number");
            else
                Console.WriteLine($"Divisors: a = {a}, b = {b}");
        }
        else
            Console.WriteLine($"It was not possible to decompose {n} into multipliers");
    }
}