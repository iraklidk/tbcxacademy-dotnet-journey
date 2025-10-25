using Practice;
public class Program
{
    public static void Main(string[] args)
    {

        Console.WriteLine("\n====================GENERIC METHODS====================");
        int[] arr = { 1, 2, 3, 4, 5 };
        GenericMethods.GenericPrintArray<int>(arr);
        GenericMethods.GenericSwap<int>(arr, 1, 2);
        Console.Write("after swap: ");
        GenericMethods.GenericPrintArray<int>(arr);
        Console.WriteLine();
        string[] names = { "Alice", "Bob", "Charlie", "Diana" };
        GenericMethods.GenericPrintArray<string>(names);
        GenericMethods.GenericSwap<string>(names, 0, 2);
        Console.Write("after swap: ");
        GenericMethods.GenericPrintArray<string>(names);

        QueueTests.RunTests();
        StackTests.RunTests();
        BSTTests.RunTests();


    }
}