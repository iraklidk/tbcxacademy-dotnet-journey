using System.Diagnostics;

public class Multithreading1
{
    static List<int> FindPrimesMultiThreading(int start, int end, int threadsCount)
    {
        List<int> primes = new List<int>();
        object lockObj = new object();

        Thread[] threads = new Thread[threadsCount];
        int rangePerThread = (end - start + 1) / threadsCount;

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < threadsCount; ++i)
        {
            int threadStart = start + i * rangePerThread;
            int threadEnd = (i == threadsCount - 1) ? end : threadStart + rangePerThread - 1;

            threads[i] = new Thread(() =>
            {
                for (int n = threadStart; n <= threadEnd; ++n)
                {
                    if (IsPrime(n))
                    {
                        lock (lockObj)
                        {
                            primes.Add(n);
                        }
                    }
                }
            });

            threads[i].Start();
        }

        foreach (var t in threads) t.Join();
        stopwatch.Stop();
        Console.WriteLine("Multi-threaded: " + stopwatch.Elapsed);

        primes.Sort();
        return primes;
    }

    static List<int> FindPrimes(int start, int end)
    {
        List<int> primes = new List<int>();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        for (int i = start; i <= end; ++i) if(IsPrime(i)) primes.Add(i);

        stopwatch.Stop();
        Console.WriteLine("Single-threaded: " + stopwatch.Elapsed);
        return primes;
    }

    static bool IsPrime(int n)
    {
        if (n < 2 | n % 2 == 0) return false;
        if(n == 2) return true;
        for (int i = 3; i * i <= n; i += 2) if (n % i == 0) return false;
        return true;
    }

    static void Main1()
    {
        Console.Write("Enter the start of the range: ");
        int start = int.Parse(Console.ReadLine());

        Console.Write("Enter the end of th erange: ");
        int end = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter the number of threads to use: ");
        int threadsCount = Convert.ToInt32(Console.ReadLine());

        List<int> primesSingleThread = FindPrimes(start, end);
        List<int> primes = FindPrimesMultiThreading(start, end, threadsCount);

        Console.WriteLine($"Prime numbers in the  range {start}-{end}:");
        foreach (int prime in primes) Console.WriteLine(prime);
    }
}