public class Program
{
    public delegate void LogMessage(string message);

    static void Main1()
    {
        LogMessage printMethodsDelegate = ConsoleWriteLine;
        printMethodsDelegate += PrintInFile;

        printMethodsDelegate("test"); // use multicast delegate
    }

    private static void PrintInFile(string msg)
    {
        string path = "output.txt";
        File.WriteAllText(path, msg);
    }

    private static void ConsoleWriteLine(string msg)
    {
        Console.WriteLine(msg);
    }
}