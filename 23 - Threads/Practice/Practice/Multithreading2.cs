public class Multithreading2
{
    static int seconds = 0;
    static bool running = true;
    static object lockObj = new object();

    static void Main8()
    {
        Thread timerThread = new Thread(TimerThread);
        Thread inputThread = new Thread(InputThread);

        timerThread.Start();
        inputThread.Start();

        timerThread.Join();
        inputThread.Join();
        Console.Write("\nProgram terminated.");
    }

    static void TimerThread()
    {
        while (running)
        {
            lock (lockObj)
            {
                Console.Write($"\rElapsed time: {seconds} seconds   ");
            }
            Thread.Sleep(1000);

            lock (lockObj)
            {
                seconds++;
            }
        }
    }

    static void InputThread()
    {
        while (running)
        {
            var key = Console.ReadKey(true).Key;

            lock (lockObj)
            {
                if (key == ConsoleKey.R) seconds = 0;
                else if (key == ConsoleKey.Q) running = false;
            }
        }
    }
}