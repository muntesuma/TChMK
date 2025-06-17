using System;

public class PolynomialIrreducibilityTest {
    public static void Main() {
        int p = 3; 
        int[] coefficients = { 2, 2, 0, 0, 0, 0, 1 }; 

        bool isReducible = IsPolynomialReducible(coefficients, p);
        Console.WriteLine($"Полином {PolynomialToString(coefficients)} над Z{p} {(isReducible ? "приводим" : "неприводим")}");
    }

    public static bool IsPolynomialReducible(int[] f, int p) {
        int n = f.Length - 1;           // cтепень полинома
        if (n <= 0) return true;        // константные полиномы приводимы

        int[] u = new int[] { 0, 1 }; // 1

        for (int i = 0; i < n / 2; i++) {
            // 2.1
            u = PolynomialModPow(u, p, f, p);

            // 2.2
            int[] uMinusX = PolynomialSubtract(u, new int[] { 0, 1 }, p);
            int[] d = PolynomialGCD(f, uMinusX, p);

            // 2.3
            if (!IsConstantPolynomial(d, 1)) {
                return true;
            }
        }

        // 3
        return false;
    }

    // возведение полинома в степень по модулю другого полинома
    private static int[] PolynomialModPow(int[] poly, int power, int[] mod, int p)
    {
        int[] result = new int[] { 1 };

        for (int i = 0; i < power; i++) {
            result = PolynomialMod(PolynomialMultiply(result, poly, p), mod, p);
        }

        return result;
    }

    // умножение полиномов
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

    // вычитание полиномов
    private static int[] PolynomialSubtract(int[] a, int[] b, int p)
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
    private static int[] PolynomialGCD(int[] a, int[] b, int p)
    {
        // степень а больше
        if (GetDegree(b) > GetDegree(a))
        {
            (a, b) = (b, a);
        }

        while (!IsZeroPolynomial(b))
        {
            int[] r = PolynomialMod(a, b, p);
            a = b;
            b = r;
        }

        // нормализация результата (старший коэффициент равен 1)
        return NormalizePolynomial(a, p);
    }

    // деление полиномов с остатком
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

    private static bool IsZeroPolynomial(int[] poly) {
        foreach (int coeff in poly) {
            if (coeff != 0)
                return false;
        }
        return true;
    }

    private static bool IsConstantPolynomial(int[] poly, int value)
    {
        if (poly.Length == 0) return false;

        for (int i = 1; i < poly.Length; i++) {
            if (poly[i] != 0) return false;
        }

        return poly[0] == value;
    }

    private static string PolynomialToString(int[] poly) {
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