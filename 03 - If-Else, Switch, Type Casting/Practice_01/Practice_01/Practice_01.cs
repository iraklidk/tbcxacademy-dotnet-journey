public class Practice_01
{
    public static void Main1(string[] args) // Just remove the number X from MainX to run this code.
                                            // Same applies to all cs files.
    {
        Console.WriteLine("Enter integer number :");
        int k = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine((k & 1) == 1 ? "Entered number " + k + " is odd" : "Entered number " + k + " is even");
    }
}