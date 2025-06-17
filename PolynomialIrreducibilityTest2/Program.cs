using System;

public class PolynomialIrreducibilityTest2 {
    public static void Main() {
        int p = 5; 
        int[] coefficients = {1, 3, 0, 0, 0, 1};

        bool isReducible = IsPolynomialReducible(coefficients, p);
        Console.WriteLine($"Полином {PolynomialToString(coefficients)} над Z{p} {(isReducible ? "приводим" : "неприводим")}");
    }

    public static bool IsPolynomialReducible(int[] f, int p) {
        int n = f.Length - 1;       // степень полинома
        if (n <= 0) return true;    // константные полиномы приводимы

        // 1
        int[] u = PolynomialModPow(new int[] { 0, 1 }, Power(p, n), f, p);

        int[] xPoly = new int[] { 0, 1 };
        if (!ArePolynomialsEqual(u, xPoly)) {
            return true; 
        }

        // 2
        int[] primeFactors = Factorize(n);

        foreach (int pi in primeFactors) {
            // 2.1
            int exponent = n / pi;
            u = PolynomialModPow(new int[] { 0, 1 }, Power(p, exponent), f, p);

            // 2.2
            int[] uMinusX = PolynomialSubtract(u, xPoly, p);
            int[] d = PolynomialGCD(f, uMinusX, p);

            // 2.3
            if (!IsConstantPolynomial(d, 1)) {
                return true; 
            }
        }

        return false; 
    }

    private static int Power(int x, int power)
    {
        int result = 1;
        for (int i = 0; i < power; i++)
        {
            result *= x;
        }
        return result;
    }

    // разложение числа
    private static int[] Factorize(int n)
    {
        if (n <= 1) return new int[0];

        System.Collections.Generic.List<int> factors = new System.Collections.Generic.List<int>();

        // проверка делимости на 2
        while (n % 2 == 0) {
            if (!factors.Contains(2))
                factors.Add(2);
            n /= 2;
        }

        // проверка нечетных делителей
        for (int i = 3; i <= Math.Sqrt(n); i += 2) {
            while (n % i == 0) {
                if (!factors.Contains(i))
                    factors.Add(i);
                n /= i;
            }
        }

        // когда остался простой делитель больше 2
        if (n > 2) {
            factors.Add(n);
        }

        return factors.ToArray();
    }

    // возведение полинома в степень по модулю другого полинома
    private static int[] PolynomialModPow(int[] poly, int power, int[] mod, int p)
    {
        int[] result = new int[] { 1 }; 

        for (int i = 0; i < power; i++)
        {
            result = PolynomialMod(PolynomialMultiply(result, poly, p), mod, p);
        }

        return result;
    }

    private static int[] PolynomialMultiply(int[] a, int[] b, int p)
    {
        int[] result = new int[a.Length + b.Length - 1];

        for (int i = 0; i < a.Length; i++) {
            for (int j = 0; j < b.Length; j++) {
                result[i + j] = (result[i + j] + a[i] * b[j]) % p;
            }
        }

        return result;
    }

    private static int[] PolynomialSubtract(int[] a, int[] b, int p)
    {
        int maxLength = Math.Max(a.Length, b.Length);
        int[] result = new int[maxLength];

        for (int i = 0; i < maxLength; i++)
        {
            int aVal = (i < a.Length) ? a[i] : 0;
            int bVal = (i < b.Length) ? b[i] : 0;
            result[i] = (aVal - bVal + p) % p;
        }

        return result;
    }

    // НОД полиномов
    private static int[] PolynomialGCD(int[] a, int[] b, int p)
    {
        if (GetDegree(b) > GetDegree(a)) {
            int[] temp = a;
            a = b;
            b = temp;
        }

        while (!IsZeroPolynomial(b)) {
            int[] r = PolynomialMod(a, b, p);
            a = b;
            b = r;
        }

        return NormalizePolynomial(a, p);
    }

    // деление с остатком
    private static int[] PolynomialMod(int[] a, int[] b, int p)
    {
        int[] remainder = (int[])a.Clone();
        int degB = GetDegree(b);

        while (GetDegree(remainder) >= degB) {
            int degRem = GetDegree(remainder);
            int factor = (remainder[degRem] * ModInverse(b[degB], p)) % p;
            int shift = degRem - degB;

            for (int i = 0; i <= degB; i++) {
                if (i + shift < remainder.Length) {
                    remainder[i + shift] = (remainder[i + shift] - b[i] * factor + p) % p;
                }
            }
        }

        return remainder;
    }

    // нормализация
    private static int[] NormalizePolynomial(int[] poly, int p)
    {
        if (IsZeroPolynomial(poly)) return poly;

        int degree = GetDegree(poly);
        if (poly[degree] == 1) return poly;

        int inv = ModInverse(poly[degree], p);
        int[] result = new int[poly.Length];

        for (int i = 0; i <= degree; i++) {
            result[i] = (poly[i] * inv) % p;
        }

        return result;
    }

    // обратный элемент в поле Zp
    private static int ModInverse(int a, int p)
    {
        a = a % p;
        for (int x = 1; x < p; x++) {
            if ((a * x) % p == 1)
                return x;
        }
        return 1;
    }

    // получение степени полинома
    private static int GetDegree(int[] poly) {
        for (int i = poly.Length - 1; i >= 0; i--) {
            if (poly[i] != 0)
                return i;
        }
        return -1; // нулевой полином
    }

    // проверка на нулевой полином
    private static bool IsZeroPolynomial(int[] poly) {
        foreach (int coeff in poly) {
            if (coeff != 0)
                return false;
        }
        return true;
    }

    // проверка на константность полинома
    private static bool IsConstantPolynomial(int[] poly, int value) {
        if (poly.Length == 0) return false;

        for (int i = 1; i < poly.Length; i++) {
            if (poly[i] != 0) return false;
        }

        return poly[0] == value;
    }

    // проверка равенства полиномов
    private static bool ArePolynomialsEqual(int[] a, int[] b) {
        int maxLength = Math.Max(a.Length, b.Length);

        for (int i = 0; i < maxLength; i++) {
            int aVal = (i < a.Length) ? a[i] : 0;
            int bVal = (i < b.Length) ? b[i] : 0;

            if (aVal != bVal)
                return false;
        }

        return true;
    }

    private static string PolynomialToString(int[] poly)
    {
        string result = "";
        bool firstTerm = true;

        for (int i = poly.Length - 1; i >= 0; i--) {
            if (poly[i] != 0) {
                if (!firstTerm) {
                    result += " + ";
                }

                if (i == 0) {
                    result += poly[i];
                } else {
                    if (poly[i] != 1) {
                        result += poly[i];
                    }
                    result += "x";
                    if (i > 1) {
                        result += "^" + i;
                    }
                }

                firstTerm = false;
            }
        }

        return result == "" ? "0" : result;
    }
}