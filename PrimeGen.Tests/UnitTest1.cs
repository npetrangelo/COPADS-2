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
    public void TestIsIntProbablyPrime() {
        Assert.True(Program.IsIntProbablyPrime(97));
        Assert.False(Program.IsIntProbablyPrime(100));
    }
    
    [Test]
    public void TestIsProbablyPrime() {
        Assert.True(new BigInteger(97).IsProbablyPrime());
    }
}