// See https://aka.ms/new-console-template for more information

using System.Numerics;
using System.Security.Cryptography;

namespace PrimeGen;

static class Program {
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
        }
        _count = int.Parse(args[1]);
        Bits = int.Parse(args[0]);
        Console.WriteLine($"Bits={Bits} count={_count}");
    }

    public static Boolean IsProbablyPrime(this BigInteger value, int k = 10) {
        if (value > 0 && value < 3) { // Handle base case
            return true;
        }

        if (value.IsEven) {
            return false;
        }

        var d = BigInteger.Subtract(value, 1);
        var r = 0;
        while (d.IsEven) {
            d = BigInteger.Divide(d, 2);
            r++;
        }
        
        for (int i = 0; i < k; i++) {
            var bytes = RandomNumberGenerator.GetBytes(Bits/8);
            var a = new BigInteger(bytes);
            // Try again until random number is in valid range
            while (a < 2 || BigInteger.Compare(a, BigInteger.Subtract(value, 2)) > 0) {
                bytes = RandomNumberGenerator.GetBytes(Bits/8);
                a = new BigInteger(bytes);
            }
            var x = BigInteger.ModPow(a, d, value);
            if (x == 1 || BigInteger.Compare(x, BigInteger.Subtract(value, 1)) == 0) {
                continue;
            }

            var y = BigInteger.ModPow(x, 2, value);
            for (int j = 0; j < r; j++) {
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
    
    public static Boolean IsIntProbablyPrime(int value, int k = 10) {
        if (value > 0 && value < 3) { // Handle base case
            return true;
        }

        if (value % 2 == 0) {
            return false;
        }

        var d = value - 1;
        var r = 0;
        while (d % 2 == 0) {
            d /= 2;
            r++;
        }
        
        for (var i = 0; i < k; i++) {
            var a = new Random().Next(2, value - 1);
            var x = (int) Math.Pow(a, d) % value;
            if (x == 1 || x == value - 1) {
                continue;
            }

            var y = x*x % value;
            for (var j = 0; j < r; j++) {
                y = x*x % value;
                if (y == 1 && x != 1 && x != value - 1) {
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