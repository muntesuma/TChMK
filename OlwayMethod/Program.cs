using System;

class OlveyMethod
{
    static int? FindDivisor(int n, int d)
    {
        int s = (int)Math.Sqrt(n);

        // 1
        int r1 = n % d;
        int r2 = n % (d - 2);
        int q1 = n / d;
        int q2 = n / (d - 2);
        int q = 4 * (q2 - q1);

        while (true)
        {
            // 2
            d += 2;
            if (d > s)
                return null;

            // 3
            int r = 2 * r1 - r2 + q;
            r2 = r1;
            r1 = r;

            // 4
            if (r1 < 0)
            {
                r1 += d;
                q += 4;
            }

            // 5
            while (r1 >= d)
            {
                r1 -= d;
                q -= 4;
            }

            // 6
            if (r1 == 0)
                return d;
        }
    }

    static void Main()
    {
        Console.Write("Enter an odd number n: ");
        int n = int.Parse(Console.ReadLine());

        if (n % 2 == 0) {
            Console.WriteLine("The number n must be odd");
            return;
        }

        double cubeRoot = Math.Pow(n, 1.0 / 3);
        int minD = (int)(2 * cubeRoot) + 1;

        if (minD % 2 == 0) minD++;

        Console.Write($"Enter the odd d >= {minD}: ");
        int d = int.Parse(Console.ReadLine());

        if (d < minD || d % 2 == 0) {
            Console.WriteLine($"d must be odd and> = {minD}");
            return;
        }

        int? divisor = FindDivisor(n, d);

        if (divisor.HasValue)
            Console.WriteLine($"Divider is founded: {divisor.Value}");
        else
            Console.WriteLine("There is no divider in the indicated range");
    }
}