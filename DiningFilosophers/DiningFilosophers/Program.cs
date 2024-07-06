using System;
using System.Threading;

class DiningPhilosophers
{
    static Semaphore[] chopsticks = new Semaphore[5];
    static Semaphore maxPhilosophers = new Semaphore(4, 4); // Allow at most 4 philosophers

    static void Philosopher(int id)
    {
        while (true)
        {
            Think(id);

            maxPhilosophers.WaitOne();

            chopsticks[id].WaitOne();
            chopsticks[(id + 1) % 5].WaitOne();

            Eat(id);

            chopsticks[(id + 1) % 5].Release();
            chopsticks[id].Release();

            maxPhilosophers.Release();
        }
    }

    static void Think(int id)
    {
        Console.WriteLine("Philosopher " + id + " is thinking.");
        Thread.Sleep(new Random().Next(1000, 3000));
    }

    static void Eat(int id)
    {
        Console.WriteLine("Philosopher " + id + " is eating.");
        Thread.Sleep(new Random().Next(1000, 3000));
    }

    static void Main(string[] args)
    {
        Thread[] philosophers = new Thread[5];

        for (int i = 0; i < 5; i++)
        {
            chopsticks[i] = new Semaphore(1, 1);
        }

        for (int i = 0; i < 5; i++)
        {
            int id = i;
            philosophers[i] = new Thread(() => Philosopher(id));
            philosophers[i].Start();
        }

        foreach (Thread philosopher in philosophers)
        {
            philosopher.Join();
        }
    }
}