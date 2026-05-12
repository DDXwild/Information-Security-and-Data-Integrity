using System;
using System.Text;
using System.Numerics;

namespace RSACipher
{
    class Program
    {
        static bool IsPrime(long n)
        {
            if (n < 2) return false;
            if (n == 2) return true;
            if (n % 2 == 0) return false;
            for (long i = 3; i * i <= n; i += 2)
                if (n % i == 0) return false;
            return true;
        }

        static long GCD(long a, long b)
        {
            while (b != 0) { long t = b; b = a % b; a = t; }
            return a;
        }

        static long ModInverse(long e, long phi)
        {
            long old_r = phi, r = e;
            long old_s = 0, s = 1;
            while (r != 0)
            {
                long q = old_r / r;
                (old_r, r) = (r, old_r - q * r);
                (old_s, s) = (s, old_s - q * s);
            }
            return (old_s % phi + phi) % phi;
        }

        static BigInteger ModPow(BigInteger b, BigInteger exp, BigInteger mod)
        {
            BigInteger result = 1;
            b %= mod;
            while (exp > 0)
            {
                if (exp % 2 == 1) result = result * b % mod;
                exp >>= 1;
                b = b * b % mod;
            }
            return result;
        }

        static long[] Encrypt(string text, long e, long n)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            long[] cipher = new long[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
                cipher[i] = (long)ModPow(bytes[i], e, n);
            return cipher;
        }

        static string Decrypt(long[] cipher, long d, long n)
        {
            byte[] bytes = new byte[cipher.Length];
            for (int i = 0; i < cipher.Length; i++)
                bytes[i] = (byte)ModPow(cipher[i], d, n);
            return Encoding.UTF8.GetString(bytes);
        }

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== RSA Cipher ===\n");

            Console.Write("Enter prime p : ");
            long p = long.Parse(Console.ReadLine()?.Trim() ?? "61");
            Console.Write("Enter prime q : ");
            long q = long.Parse(Console.ReadLine()?.Trim() ?? "53");

            if (!IsPrime(p) || !IsPrime(q))
            { Console.WriteLine("Both numbers must be prime."); return; }
            if (p == q)
            { Console.WriteLine("p and q must be different."); return; }

            long n = p * q;
            long phi = (p - 1) * (q - 1);

            long e = 2;
            while (e < phi && GCD(e, phi) != 1) e++;

            long d = ModInverse(e, phi);

            Console.WriteLine($"\nPublic key  : (e={e}, n={n})");
            Console.WriteLine($"Private key : (d={d}, n={n})");
            Console.WriteLine($"phi(n)      : {phi}");

            Console.Write("\nEnter plaintext : ");
            string text = Console.ReadLine() ?? "Hello";

            foreach (byte b in Encoding.UTF8.GetBytes(text))
                if (b >= n)
                { Console.WriteLine($"n={n} is too small for byte value {b}. Use larger primes."); return; }

            long[] encrypted = Encrypt(text, e, n);
            Console.Write("Encrypted       : ");
            Console.WriteLine(string.Join(" ", encrypted));

            string decrypted = Decrypt(encrypted, d, n);
            Console.WriteLine($"Decrypted       : {decrypted}");
            Console.WriteLine($"\nVerification    : {(decrypted == text ? "OK - texts match" : "FAIL - texts differ")}");
        }
    }
}