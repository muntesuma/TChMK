using System;

class TrialDivision
{
    static long[] Factorize(long n)
    {
        long[] Factors = new long[64]; // множителей <= 64
        int count = 0;

        while (n % 2 == 0) {
            Factors[count++] = 2;
            n /= 2;
        }

        if (n == 1)
            return CopyArray(Factors, count);

        long d = 3;
        long sqrtN = (long)Math.Sqrt(n) + 1;

        while (d <= sqrtN && n > 1) {
            if (n % d == 0) {
                Factors[count++] = d;
                n /= d;
                sqrtN = (long)Math.Sqrt(n) + 1; 
            } else
                d += (d % 6 == 1) ? 4 : 2; 
        }

        if (n > 1)
            Factors[count++] = n;

        return CopyArray(Factors, count);
    }

    static long[] CopyArray(long[] source, int length)
    {
        long[] result = new long[length];
        Array.Copy(source, result, length);
        return result;
    }

    static void Main()
    {
        Console.Write("Enter the number n > 1: "); // 27644437
        long n = long.Parse(Console.ReadLine());

        long[] factors = Factorize(n);

        Console.Write("Factorization: " + n + " = ");
        for (int i = 0; i < factors.Length; i++)
        {
            if (i > 0) Console.Write(" * ");
            Console.Write(factors[i]);
        }
    }
}