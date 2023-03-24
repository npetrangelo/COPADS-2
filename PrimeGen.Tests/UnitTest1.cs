using System.Numerics;

namespace PrimeGen.Tests;

public class Tests {
    [SetUp]
    public void Setup() {
        Program.Bits = 8;
    }

    [Test]
    public void Test1() {
        Assert.Pass();
    }

    [Test]
    public void TestIsProbablyPrime() {
        Assert.True(new BigInteger(97).IsProbablyPrime());
        Assert.False(new BigInteger(100).IsProbablyPrime());
    }
}