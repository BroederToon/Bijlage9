using NUnit.Framework;
using System;
using System.IO;
using System.Threading;

[TestFixture]
public class CigaretteSmokersProblemTests
{
    private StringWriter _consoleOutput;

    [SetUp]
    public void SetUp()
    {
        _consoleOutput = new StringWriter();
        Console.SetOut(_consoleOutput);
    }

    [TearDown]
    public void TearDown()
    {
        _consoleOutput.Dispose();
    }

    [Test]
    public void TestAgentAndSmokers()
    {
        // Arrange
        Thread agentThread = new Thread(CigaretteSmokersProblem.Agent);
        Thread[] smokerThreads = new Thread[3];

        for (int i = 0; i < 3; i++)
        {
            int id = i;
            smokerThreads[i] = new Thread(() => CigaretteSmokersProblem.Smoker(id, CigaretteSmokersProblem.SmokerSemaphores[id], CigaretteSmokersProblem.Ingredients[id]));
            smokerThreads[i].Start();
        }

        // Act
        agentThread.Start();

        foreach (Thread smokerThread in smokerThreads)
        {
            smokerThread.Join(1000);
        }

        agentThread.Join(1000);

        // Assert
        string output = _consoleOutput.ToString();
        Assert.That(output.Contains("Smoker with tobacco is making and smoking a cigarette."));
        Assert.That(output.Contains("Smoker with paper is making and smoking a cigarette."));
        Assert.That(output.Contains("Smoker with matches is making and smoking a cigarette."));
    }
}