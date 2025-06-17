using System;

public class PolynomialPrimitivityTest {
    public static bool IsPrimitivePolynomial(int p, int[] coefficients, int n) {
        // 0
        long order = (long)Math.Pow(p, n) - 1;
        long[] primeFactors = GetPrimeFactors(order);

        // 1.1
        for (int i = 0; i < primeFactors.Length; i++) {
            long exponent = order / primeFactors[i];
            int[] remainder = PolynomialModPow(p, new int[] { 0, 1 }, exponent, coefficients);

            // 1.2
            if (IsOne(remainder, p))return false;
        }

        // 2
        return true;
    }

    private static long FindDivisorOlvey(long n, long d)
    {
        if (n % 2 == 0) return 2; // n должно быть нечетным

        long s = (long)Math.Sqrt(n); // верхняя граница

        // 1
        long r1 = n % d;
        long r2 = n % (d - 2);
        long q1 = n / d;
        long q2 = n / (d - 2);
        long q = 4 * (q2 - q1);

        while (true)
        {
            // 2
            d += 2;
            if (d > s) return -1; // делителя нет

            // 3
            long r = 2 * r1 - r2 + q;
            r2 = r1;
            r1 = r;

            // 4
            if (r1 < 0) {
                r1 += d;
                q += 4;
            }

            // 5
            while (r1 >= d) {
                r1 -= d;
                q -= 4;
            }

            // 6
            if (r1 == 0) return d;
        }
    }

    private static long[] GetPrimeFactors(long number) {
        long[] tempStorage = new long[64];
        int count = 0;

        // тривиальные случаи
        if (number % 2 == 0) {
            tempStorage[count++] = 2;
            while (number % 2 == 0) number /= 2;
        }

        if (number == 1) {
            long[] resultFactors = new long[count];
            Array.Copy(tempStorage, resultFactors, count);
            return resultFactors;
        }

        // метод Олвея
        long n = number;
        long d = (long)(2 * Math.Cbrt(n) + 1);
        if (d % 2 == 0) d++; // d должно быть нечетным

        while (n > 1) {
            long divisor = FindDivisorOlvey(n, d);

            if (divisor == -1) {
                tempStorage[count++] = n; // n - простое число
                break;
            }

            tempStorage[count++] = divisor;
            while (n % divisor == 0) n /= divisor;

            // обновление d для следующей раунда
            d = (long)(2 * Math.Cbrt(n) + 1);
            if (d % 2 == 0) d++;
        }

        // конечный массив
        long[] finalFactors = new long[count];
        Array.Copy(tempStorage, finalFactors, count);
        return finalFactors;
    }

    // возведение полинома в степень по модулю другого полинома в поле
    private static int[] PolynomialModPow(int p, int[] poly, long exponent, int[] modulus) {
        int[] result = { 1 };
        int[] basePoly = (int[])poly.Clone();

        while (exponent > 0) {
            if (exponent % 2 == 1) result = PolynomialMod(p, PolynomialMultiply(p, result, basePoly), modulus);

            basePoly = PolynomialMod(p, PolynomialMultiply(p, basePoly, basePoly), modulus);
            exponent /= 2;
        }

        return result;
    }

    // умножение двух полиномов в поле 
    private static int[] PolynomialMultiply(int p, int[] a, int[] b)
    {
        int[] result = new int[a.Length + b.Length - 1];

        for (int i = 0; i < a.Length; i++) {
            for (int j = 0; j < b.Length; j++) {
                result[i + j] = (result[i + j] + a[i] * b[j]) % p;
            }
        }

        return result;
    }

    // Вычисление остатка от деления полиномов в поле 
    private static int[] PolynomialMod(int p, int[] dividend, int[] divisor)
    {
        int[] remainder = (int[])dividend.Clone();

        while (remainder.Length >= divisor.Length) {
            int degreeDiff = remainder.Length - divisor.Length;
            int leadCoeff = remainder[remainder.Length - 1];

            for (int i = 0; i < divisor.Length; i++) {
                int pos = i + degreeDiff;
                remainder[pos] = (remainder[pos] - leadCoeff * divisor[i]) % p;
                if (remainder[pos] < 0) remainder[pos] += p; // Обеспечиваем положительный остаток
            }

            // удаление ведущих нулей
            int newLength = remainder.Length - 1;
            while (newLength > 0 && remainder[newLength - 1] == 0)
                newLength--;

            if (newLength < remainder.Length) {
                int[] newRemainder = new int[newLength];
                Array.Copy(remainder, newRemainder, newLength);
                remainder = newRemainder;
            }
        }

        return remainder;
    }

    // проверка единичности полинома
    private static bool IsOne(int[] poly, int p) {
        return poly.Length == 1 && poly[0] % p == 1;
    }

    public static void Main() {
        int p = 2;
        int[] coefficients = { 1, 1, 0, 1, 1, 0, 0, 0, 1};
        int n = 8;

        bool isReducible = IsPolynomialReducible2(coefficients, p);
        Console.WriteLine($"Полином {PolynomialToString2(coefficients)} над Z{p} {(isReducible ? "приводим" : "неприводим")}");

        if (isReducible == false) {
            bool isPrimitive = IsPrimitivePolynomial(p, coefficients, n);
            Console.WriteLine(isPrimitive ? "Полином примитивный" : "Полином не примитивный");
        } else Console.WriteLine("Нет смысла проверять на примитивность");
    }

    public static bool IsPolynomialReducible2(int[] f, int p) {
        int n = f.Length - 1;           // cтепень полинома
        if (n <= 0) return true;        // константные полиномы приводимы

        int[] u = new int[] { 0, 1 }; // 1

        for (int i = 0; i < n / 2; i++) {
            // 2.1
            u = PolynomialModPow2(u, p, f, p);

            // 2.2
            int[] uMinusX = PolynomialSubtract2(u, new int[] { 0, 1 }, p);
            int[] d = PolynomialGCD2(f, uMinusX, p);

            // 2.3
            if (!IsConstantPolynomial2(d, 1)) return true;
        }

        // 3
        return false;
    }

    // возведение полинома в степень по модулю другого полинома
    private static int[] PolynomialModPow2(int[] poly, int power, int[] mod, int p)
    {
        int[] result = new int[] { 1 };

        for (int i = 0; i < power; i++) {
            result = PolynomialMod2(PolynomialMultiply2(result, poly, p), mod, p);
        }

        return result;
    }

    // умножение полиномов
    private static int[] PolynomialMultiply2(int[] a, int[] b, int p) {
        int[] result = new int[a.Length + b.Length - 1];

        for (int i = 0; i < a.Length; i++) {
            for (int j = 0; j < b.Length; j++) {
                result[i + j] = (result[i + j] + a[i] * b[j]) % p;
            }
        }

        return result;
    }

    // вычитание полиномов
    private static int[] PolynomialSubtract2(int[] a, int[] b, int p)
    {
        int maxLength = Math.Max(a.Length, b.Length);
        int[] result = new int[maxLength];

        for (int i = 0; i < maxLength; i++) {
            int aVal = (i < a.Length) ? a[i] : 0;
            int bVal = (i < b.Length) ? b[i] : 0;
            result[i] = (aVal - bVal + p) % p;
        }

        return result;
    }

    // НОД двух полиномов
    private static int[] PolynomialGCD2(int[] a, int[] b, int p)
    {
        // степень а больше
        if (GetDegree2(b) > GetDegree2(a)) {
            (a, b) = (b, a);
        }

        while (!IsZeroPolynomial2(b)) {
            int[] r = PolynomialMod2(a, b, p);
            a = b;
            b = r;
        }

        // нормализация результата (старший коэффициент равен 1)
        return NormalizePolynomial2(a, p);
    }

    // деление полиномов с остатком
    private static int[] PolynomialMod2(int[] a, int[] b, int p)
    {
        int[] remainder = (int[])a.Clone();
        int degB = GetDegree2(b);

        while (GetDegree2(remainder) >= degB) {
            int degRem = GetDegree2(remainder);
            int factor = (remainder[degRem] * ModInverse2(b[degB], p)) % p;
            int shift = degRem - degB;

            for (int i = 0; i <= degB; i++) {
                if (i + shift < remainder.Length) {
                    remainder[i + shift] = (remainder[i + shift] - b[i] * factor + p) % p;
                }
            }
        }

        return remainder;
    }

    private static int[] NormalizePolynomial2(int[] poly, int p)
    {
        if (IsZeroPolynomial2(poly)) return poly;

        int degree = GetDegree2(poly);
        if (poly[degree] == 1) return poly;

        int inv = ModInverse2(poly[degree], p);
        int[] result = new int[poly.Length];

        for (int i = 0; i <= degree; i++) {
            result[i] = (poly[i] * inv) % p;
        }

        return result;
    }

    // обратный элемент в поле Zp
    private static int ModInverse2(int a, int p) {
        a = a % p;
        for (int x = 1; x < p; x++) {
            if ((a * x) % p == 1)
                return x;
        }
        return 1;
    }

    // получение степени полинома
    private static int GetDegree2(int[] poly) {
        for (int i = poly.Length - 1; i >= 0; i--) {
            if (poly[i] != 0)
                return i;
        }
        return -1; // нулевой полином
    }

    private static bool IsZeroPolynomial2(int[] poly) {
        foreach (int coeff in poly) {
            if (coeff != 0)
                return false;
        }
        return true;
    }

    private static bool IsConstantPolynomial2(int[] poly, int value) {
        if (poly.Length == 0) return false;

        for (int i = 1; i < poly.Length; i++) {
            if (poly[i] != 0) return false;
        }

        return poly[0] == value;
    }

    private static string PolynomialToString2(int[] poly) {
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