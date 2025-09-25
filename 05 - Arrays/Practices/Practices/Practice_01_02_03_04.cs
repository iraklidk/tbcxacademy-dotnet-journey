public class Practice_01_02_03_04
{
    public static void Main1(string[] args)
    {
        // 01
        Console.Write("Enter array size: ");
        int size = Convert.ToInt32(Console.ReadLine());
        int[] arr = new int[size];
        for(int i = 0; i < size; ++i)
        {
            Console.Write("Enter number for index " + i + ": ");
            arr[i] = Convert.ToInt32(Console.ReadLine());
        }
        foreach(int k in arr) Console.WriteLine(k);

        // 02
        Console.WriteLine("Here is your array in reverse order!");
        for (int i = size - 1; i >= 0; --i)
        {
            Console.WriteLine(arr[i]);
        }

        // 03
        int sum = 0;
        foreach(int k in arr) sum += k;
        Console.WriteLine("Sum of array elements is: " + sum);

        // 04
        long product = 1;
        foreach(int k in arr) product *= k;
        Console.WriteLine("Product of array elements is: " + product);
    }
}