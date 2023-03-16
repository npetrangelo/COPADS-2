// See https://aka.ms/new-console-template for more information

using System.Numerics;
using System.Security.Cryptography;

namespace PrimeGen;

static class Program {
    static void Main(string[] args) {
        Console.WriteLine("Hello, World!");
    }

    static Boolean IsProbablyPrime(this BigInteger value, int k = 10) {
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
            var a = RandomNumberGenerator.GetInt32(2, BigInteger.Subtract(value, 2));
            var bytes = RandomNumberGenerator.GetBytes(3); // How many bytes?
            var rand = new BigInteger(bytes);
        }
        return true;
    }
}