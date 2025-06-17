using System;

public class PollardRhoOneMethod
{
    // НОД (алгоритм Евклида)
    public static int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    // быстрое возведение в степень по модулю (a^b mod n)
    public static int ModPow(int a, int exponent, int modulus)
    {
        int result = 1;
        a = a % modulus;
        while (exponent > 0)
        {
            if (exponent % 2 == 1)
                result = (result * a) % modulus;
            exponent >>= 1;
            a = (a * a) % modulus;
        }
        return result;
    }

    public static bool IsPrime(int n)
    {
        if (n <= 1) return false;
        if (n == 2) return true;
        if (n % 2 == 0) return false;

        for (int i = 3; i * i <= n; i += 2)
        {
            if (n % i == 0)
                return false;
        }
        return true;
    }

    // массив простых чисел до B
    public static int[] GetPrimesUpTo(int B)
    {
        int count = 0;
        for (int i = 2; i <= B; i++)
            if (IsPrime(i)) count++;

        int[] primes = new int[count];
        int index = 0;
        for (int i = 2; i <= B; i++)
            if (IsPrime(i)) primes[index++] = i;

        return primes;
    }

    // Метод Полларда для поиска делителя
    public static int PollardMethodForDivisor(int n, int B)
    {
        if (n % 2 == 0) return 2; // тривиальный случай

        Random rand = new Random(); // 2
        int a = rand.Next(2, n - 1);
        int d = GCD(a, n);

        if (d > 1)
            return d;

        int[] primes = GetPrimesUpTo(B);

        foreach (int q in primes) // 3
        {
            // e = [ln(n) / ln(q)]
            double logN = Math.Log(n);
            double logQ = Math.Log(q);
            int e = (int)(logN / logQ);

            // a = (a^q)^e mod n
            int qPowE = (int)Math.Pow(q, e);
            a = ModPow(a, qPowE, n);
        }

        if (a == 1) // 4
            return -1; 

        d = GCD(a - 1, n);

        if (d == 1 || d == n) // 5
            return -1;
        else
            return d;
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("Enter the number n (composite): ");
        int n = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter the border of the smoothness b:"); // 1
        int B = int.Parse(Console.ReadLine());

        int divisor = PollardMethodForDivisor(n, B);

        if (divisor == -1)
            Console.WriteLine("Non-trivial divisor was not found.");
        else
            Console.WriteLine($"Non-trivial divisor was found: {divisor}");
    }
}