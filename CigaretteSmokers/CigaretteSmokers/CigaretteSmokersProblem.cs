using System;
using System.Threading;

public class CigaretteSmokersProblem
{
    static Semaphore agentSemaphore = new Semaphore(1, 1);
    static Semaphore tobaccoSemaphore = new Semaphore(0, 1);
    static Semaphore paperSemaphore = new Semaphore(0, 1);
    static Semaphore matchSemaphore = new Semaphore(0, 1);
    static Semaphore[] smokerSemaphores = new Semaphore[3] { tobaccoSemaphore, paperSemaphore, matchSemaphore };
    static string[] ingredients = { "tobacco", "paper", "matches" };

    public static void Agent()
    {
        Random rand = new Random();
        while (true)
        {
            agentSemaphore.WaitOne();

            int missingIngredient = rand.Next(3);
            Console.WriteLine($"Agent puts out {ingredients[(missingIngredient + 1) % 3]} and {ingredients[(missingIngredient + 2) % 3]}.");

            switch (missingIngredient)
            {
                case 0:
                    tobaccoSemaphore.Release();
                    break;
                case 1:
                    paperSemaphore.Release();
                    break;
                case 2:
                    matchSemaphore.Release();
                    break;
            }
        }
    }

    public static void Smoker(int id, Semaphore mySemaphore, string myIngredient)
    {
        while (true)
        {
            mySemaphore.WaitOne();
            Console.WriteLine($"Smoker with {myIngredient} is making and smoking a cigarette.");
            Thread.Sleep(1000);
            Console.WriteLine($"Smoker with {myIngredient} is done smoking.");

            agentSemaphore.Release();
        }
    }

    public static void Main(string[] args)
    {
        Thread agentThread = new Thread(Agent);
        Thread[] smokerThreads = new Thread[3];

        for (int i = 0; i < 3; i++)
        {
            int id = i;
            smokerThreads[i] = new Thread(() => Smoker(id, smokerSemaphores[id], ingredients[id]));
            smokerThreads[i].Start();
        }

        agentThread.Start();

        foreach (Thread smokerThread in smokerThreads)
        {
            smokerThread.Join();
        }

        agentThread.Join();
    }

    public static Semaphore[] SmokerSemaphores => smokerSemaphores;

    public static string[] Ingredients => ingredients;
}
