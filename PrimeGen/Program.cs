﻿// See https://aka.ms/new-console-template for more information

using System.Numerics;
using System.Security.Cryptography;

namespace PrimeGen;

internal static class Program {
    public static int Bits;
    private static int _count = 1;

    private const string ErrorMsg = """
        dotnet run <bits> <count=1>
            - bits - the number of bits of the prime number, this must be a multiple of 8, and at least 32 bits.
            - count - the number of prime numbers to generate, defaults to 1
        """;
    private static void Main(string[] args) {
        switch (args.Length) {
            case < 1:
                Console.WriteLine("Arguments not specified.");
                Console.WriteLine(ErrorMsg);
                return;
            case > 2:
                Console.WriteLine("Too many arguments.");
                Console.WriteLine(ErrorMsg);
                return;
            case 2:
                _count = int.Parse(args[1]);
                goto case 1;
            case 1:
                Bits = int.Parse(args[0]);
                break;
        }
        Console.WriteLine($"BitLength: {Bits}");
        var n = 1;
        var nLock = new object();
        Parallel.For(0, _count, _ => {
            BigInteger prime;
            do {
                prime = new BigInteger(RandomNumberGenerator.GetBytes(Bits / 8), true);
            } while (!prime.IsProbablyPrime());

            lock (nLock) {
                Console.WriteLine($"{n++}: {prime}");
            }
        });
    }

    public static bool IsProbablyPrime(this BigInteger value, int k = 10) {
        if (value > 0 && value < 3) { // Handle base case
            return true;
        }
        
        int[] lowPrimes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31 };
        foreach (var n in lowPrimes) {
            if (BigInteger.Remainder(value, n) == 0) {
                return false;
            }
        }

        var d = BigInteger.Subtract(value, 1);
        var r = 0;
        while (d.IsEven) {
            d = BigInteger.Divide(d, 2);
            r++;
        }
        
        for (var i = 0; i < k; i++) {
            BigInteger a;
            do {
                a = new BigInteger(RandomNumberGenerator.GetBytes(Bits/8), true);
            } while (a < 2 || BigInteger.Compare(a, BigInteger.Subtract(value, 2)) > 0);
            var x = BigInteger.ModPow(a, d, value);
            if (x == 1 || BigInteger.Compare(x, BigInteger.Subtract(value, 1)) == 0) {
                continue;
            }

            var y = BigInteger.ModPow(x, 2, value);
            for (var j = 0; j < r; j++) {
                y = BigInteger.ModPow(x, 2, value);
                if (y == 1 && x != 1 && BigInteger.Compare(x, BigInteger.Subtract(value, 1)) != 0) {
                    return false;
                }
                x = y;
            }
            if (y != 1) {
                return false;
            }
        }
        return true;
    }
}