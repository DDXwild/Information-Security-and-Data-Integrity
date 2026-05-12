using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MD5Auth
{
    class Program
    {
        static Dictionary<string, string> users = new Dictionary<string, string>();

        static string ComputeMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        static void Register()
        {
            Console.Write("Enter login    : ");
            string login = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrEmpty(login)) { Console.WriteLine("Login cannot be empty."); return; }
            if (users.ContainsKey(login)) { Console.WriteLine("User already exists."); return; }

            Console.Write("Enter password : ");
            string password = Console.ReadLine() ?? "";
            if (string.IsNullOrEmpty(password)) { Console.WriteLine("Password cannot be empty."); return; }

            string hash = ComputeMD5(password);
            users[login] = hash;
            Console.WriteLine($"User '{login}' registered.");
            Console.WriteLine($"Stored hash    : {hash}");
        }

        static void Login()
        {
            Console.Write("Enter login    : ");
            string login = Console.ReadLine()?.Trim() ?? "";
            if (!users.ContainsKey(login)) { Console.WriteLine("User not found."); return; }

            Console.Write("Enter password : ");
            string password = Console.ReadLine() ?? "";
            string hash = ComputeMD5(password);
            Console.WriteLine($"Computed hash  : {hash}");

            if (users[login] == hash)
                Console.WriteLine($"Access granted. Welcome, {login}.");
            else
                Console.WriteLine("Access denied. Incorrect password.");
        }

        static void ListUsers()
        {
            if (users.Count == 0) { Console.WriteLine("No users registered."); return; }
            Console.WriteLine($"{"Login",-20} {"MD5 Hash",-32}");
            Console.WriteLine(new string('-', 54));
            foreach (var kv in users)
                Console.WriteLine($"{kv.Key,-20} {kv.Value,-32}");
        }

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== User Authentication via MD5 ===");

            while (true)
            {
                Console.WriteLine("\n1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. List users");
                Console.WriteLine("0. Exit");
                Console.Write("Choice: ");
                string choice = Console.ReadLine()?.Trim() ?? "";

                switch (choice)
                {
                    case "1": Register(); break;
                    case "2": Login(); break;
                    case "3": ListUsers(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }
    }
}