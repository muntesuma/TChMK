using System;

class HelfondAlgorithm
{
    static void Main()
    {
        Console.WriteLine("Helfond's Algorithm (Giant-step Baby-step)");

        Console.Write("Enter group order (n): ");
        int n = int.Parse(Console.ReadLine());

        Console.Write("Enter the generating element of the group (g): ");
        int g = int.Parse(Console.ReadLine());

        Console.Write("Enter group element (a): ");
        int a = int.Parse(Console.ReadLine());

        try
        {
            int x = BabyStepGiantStep(g, a, n);
            Console.WriteLine($"Discrete logarithm x = log{g}({a}) = {x}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static int BabyStepGiantStep(int g, int a, int n)
    {
        // 1
        int h = (int)Math.Sqrt(n) + 1;
        Console.WriteLine($"h = [√{n}] + 1 = {h}");

        // 2
        int b = ModPow(g, h, n);
        Console.WriteLine($"b = {g}^{h} mod {n} = {b}");

        // 3 (b^u mod n)
        int[] giantValues = new int[h + 1];
        giantValues[1] = b;
        for (int u = 2; u <= h; u++) // 4
        {
            giantValues[u] = (giantValues[u - 1] * b) % n;
        }

        // 3 (a * g^v mod n)
        int[] babyValues = new int[h + 1]; 
        babyValues[0] = a % n;
        for (int v = 1; v <= h; v++)
        {
            babyValues[v] = (babyValues[v - 1] * g) % n;
        }

        // 4
        for (int u = 1; u <= h; u++)
        {
            for (int v = 0; v <= h; v++)
            {
                if (giantValues[u] == babyValues[v])
                {
                    Console.WriteLine($"Match founded: u = {u}, v = {v}");
                    int x = (h * u - v) % n;
                    if (x < 0) x += n; // корректировка отрицательных значений

                    // проверка корректности
                    if (ModPow(g, x, n) == a) {
                        return x;
                    } else {
                        throw new Exception("The solution found is not correct");
                    }
                }
            }
        }

        throw new Exception("Discrete logarithm not found");
    }

    // быстрое возведение в степень по модулю (без рекурсии)
    static int ModPow(int baseValue, int exponent, int modulus)
    {
        if (modulus == 1) return 0;

        int result = 1;
        baseValue = baseValue % modulus;

        while (exponent > 0)
        {
            if ((exponent & 1) == 1)
                result = (result * baseValue) % modulus;

            exponent >>= 1;
            baseValue = (baseValue * baseValue) % modulus;
        }

        return result;
    }
}