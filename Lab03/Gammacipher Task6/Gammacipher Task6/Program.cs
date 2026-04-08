using System;
using System.Text;

namespace GammaCipher
{
    class Program
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

        static string Canonicalize(string text)
        {
            var sb = new StringBuilder();
            foreach (char c in text)
                if (char.IsLetter(c) && c < 128)
                    sb.Append(char.ToLower(c));
            return sb.ToString();
        }

        static string BuildGamma(string key, int length)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < length; i++)
                sb.Append(key[i % key.Length]);
            return sb.ToString();
        }

        static string Encrypt(string plaintext, string gamma)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < plaintext.Length; i++)
            {
                int o = Alphabet.IndexOf(plaintext[i]);
                int g = Alphabet.IndexOf(gamma[i]);
                int s = ((o - g) % 26 + 26) % 26;
                sb.Append(Alphabet[s]);
            }
            return sb.ToString();
        }

        static string Decrypt(string ciphertext, string gamma)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < ciphertext.Length; i++)
            {
                int s = Alphabet.IndexOf(ciphertext[i]);
                int g = Alphabet.IndexOf(gamma[i]);
                int o = (s + g) % 26;
                sb.Append(Alphabet[o]);
            }
            return sb.ToString();
        }

        static void PrintTable(string source, string gamma, string result, string formula, int maxRows = 15)
        {
            Console.WriteLine($"\n{"#",-4} {"Char",-6} {"Key",-6} {"Code C",-8} {"Code K",-8} {"Expr",-10} {"mod26",-7} {"Result",-6}");
            Console.WriteLine(new string('-', 60));
            int count = Math.Min(maxRows, source.Length);
            for (int i = 0; i < count; i++)
            {
                int src = Alphabet.IndexOf(source[i]);
                int g = Alphabet.IndexOf(gamma[i]);
                int expr = formula == "encrypt" ? src - g : src + g;
                int mod = ((expr % 26) + 26) % 26;
                Console.WriteLine($"{i + 1,-4} {source[i],-6} {gamma[i],-6} {src,-8} {g,-8} {expr,-10} {mod,-7} {result[i],-6}");
            }
            if (source.Length > maxRows)
                Console.WriteLine($"... ({source.Length - maxRows} more characters omitted)");
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Gamma Cipher ===\n");

            string inputText;
            string key;

            if (args.Length >= 2)
            {
                inputText = args[0];
                key = args[1].ToLower();
                Console.WriteLine($"Text  : {inputText}");
                Console.WriteLine($"Key   : {key}");
            }
            else
            {
                Console.Write("Enter plaintext : ");
                inputText = Console.ReadLine() ?? "";
                Console.Write("Enter key       : ");
                key = (Console.ReadLine() ?? "").ToLower();
            }

            string canonicalKey = Canonicalize(key);
            if (canonicalKey.Length == 0)
            {
                Console.WriteLine("Error: key contains no Latin letters.");
                return;
            }

            string plaintext = Canonicalize(inputText);
            Console.WriteLine($"\n[1] Canonical plaintext ({plaintext.Length} chars):");
            Console.WriteLine($"    {plaintext}");

            string gamma = BuildGamma(canonicalKey, plaintext.Length);
            Console.WriteLine($"\n[2] Gamma (key \"{canonicalKey}\" repeated to {plaintext.Length} chars):");
            Console.WriteLine($"    {gamma}");

            string ciphertext = Encrypt(plaintext, gamma);
            Console.WriteLine("\n[3] Encryption  S = (O - K) mod 26  (first 15 steps):");
            PrintTable(plaintext, gamma, ciphertext, "encrypt");
            Console.WriteLine($"\n    Ciphertext:");
            Console.WriteLine($"    {ciphertext}");

            string decrypted = Decrypt(ciphertext, gamma);
            Console.WriteLine("\n[4] Decryption  O = (S + K) mod 26  (first 15 steps):");
            PrintTable(ciphertext, gamma, decrypted, "decrypt");
            Console.WriteLine($"\n    Decrypted text:");
            Console.WriteLine($"    {decrypted}");

            bool ok = decrypted == plaintext;
            Console.WriteLine($"\n[5] Verification: decrypted text {(ok ? "matches plaintext" : "does NOT match")}");
        }
    }
}