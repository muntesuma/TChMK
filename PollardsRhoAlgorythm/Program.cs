using System;

public class PollardsRhoAlgorithm {
    // тройка (x, u, v)
    private struct Triple {
        public long x;
        public long u;
        public long v;

        public Triple(long x, long u, long v) {
            this.x = x;
            this.u = u;
            this.v = v;
        }
    }

    // функция ρ-метода Полларда
    private static Triple F(long x, long u, long v, long a, long g, long n, long order)
    {
        long mod = x % 3;
        if (mod < 0) mod += 3; 

        if (mod == 1) {
            return new Triple(
                (a * x) % n,
                u % order,
                (v + 1) % order);
        } else if (mod == 2) {
            return new Triple(
                (x * x) % n,
                (2 * u) % order,
                (2 * v) % order);
        } else {
            return new Triple(
                (g * x) % n,
                (u + 1) % order,
                v % order);
        }
    }

    // НОД
    private static long Gcd(long a, long b) {
        while (b != 0) {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    // алгоритм Евклида для обратного элемента
    private static long ModInverse(long a, long m) {
        if (m == 1) return 0;
        long m0 = m;
        long y = 0, x = 1;

        while (a > 1) {
            long q = a / m;
            long t = m;

            m = a % m;
            a = t;
            t = y;

            y = x - q * y;
            x = t;
        }

        if (x < 0)
            x += m0;

        return x;
    }

    // проверка на переполнение
    private static long SafeMultiply(long a, long b, long mod) {
        return (a * b) % mod;
    }

    // степень по модулю
    private static long ModPow(long baseValue, long exponent, long modulus) {
        if (modulus == 1) return 0;
        long result = 1;
        baseValue %= modulus;
        while (exponent > 0) {
            if (exponent % 2 == 1)
                result = (result * baseValue) % modulus;
            exponent >>= 1;
            baseValue = (baseValue * baseValue) % modulus;
        }
        return result;
    }

    // ρ-метод Полларда
    public static long PollardsRho(long g, long a, long n, long order) {
        // 1
        long x1 = 1, u1 = 0, v1 = 0;
        long x2 = 1, u2 = 0, v2 = 0;

        while (true) {
            // 2
            var t1 = F(x1, u1, v1, a, g, n, order);
            x1 = t1.x; u1 = t1.u; v1 = t1.v;

            var t2 = F(x2, u2, v2, a, g, n, order);
            t2 = F(t2.x, t2.u, t2.v, a, g, n, order);
            x2 = t2.x; u2 = t2.u; v2 = t2.v;

            // 3
            if (x1 == x2) break;
        }

        // 4
        long r = (v1 - v2) % order;
        if (r < 0) r += order;

        if (r == 0) {
            throw new Exception("Отказ: r = 0");
        }

        // 5
        long rz = (u2 - u1) % order;
        if (rz < 0) rz += order;

        long d = Gcd(r, order);
        if (rz % d != 0) {
            throw new Exception("Нет решений");
        }

        long m = order / d;
        long rReduced = r / d;
        long rzReduced = rz / d;

        long invR = ModInverse(rReduced, m);
        long z0 = (rzReduced * invR) % m;

        // 6
        for (long i = 0; i < d; i++) {
            long z = (z0 + i * m) % order;
            if (ModPow(g, z, n) == a) {
                return z;
            }
        }

        throw new Exception("Решение не найдено");
    }

    public static void Main(string[] args)
    {
        try {
            long g = 70;            // генератор группы
            long a = 269;           // элемент, логарифм которого ищем
            long n = 599;           // модуль (порядок группы)
            long order = 598;       // порядок группы (n-1 для простого n)

            long result = PollardsRho(g, a, n, order);
            Console.WriteLine($"log{g}({a}) mod {n} = {result}");
        } catch (Exception ex) {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}