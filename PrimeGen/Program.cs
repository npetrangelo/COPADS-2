// See https://aka.ms/new-console-template for more information

using System.Numerics;
using System.Security.Cryptography;

namespace PrimeGen;

static class Program {
    public static int bits;
    static void Main(string[] args) {
        Console.WriteLine("Hello, World!");
        bits = Int32.Parse(args[0]);
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
            var bytes = RandomNumberGenerator.GetBytes(bits/8);
            var a = new BigInteger(bytes);
            // Try again until random number is in valid range
            while (a < 2 || BigInteger.Compare(a, BigInteger.Subtract(value, 2)) > 0) {
                bytes = RandomNumberGenerator.GetBytes(bits/8);
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