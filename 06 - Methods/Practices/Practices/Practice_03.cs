public class Practice_03
{

    static int[] fillArr()
    {
        Console.Write("Enter size of array: ");
        int size = Convert.ToInt32(Console.ReadLine());
        int[] arr = new int[size];
        for(int i = 0; i < size; i++)
        {
            Console.Write("Enter integer for index " + i + ": ");
            arr[i] = Convert.ToInt32(Console.ReadLine());
        }
        return arr;
    }

    static int[] minMax(int[] arr)
    {
        int mn = arr[0];
        int  mx = arr[0];
        foreach(int num in arr)
        {
            mn = Math.Min(mn, num);
            mx = Math.Max(mx, num);
        }
        return new int[] { mn, mx };
    }

    public static void Main(string[] args)
    {
        int[] arr = fillArr();
        int[] mnmx = minMax(arr);
        Console.WriteLine("The minimum number in the array is " + mnmx[0] + "\n" + "The maximum number in the array is " + mnmx[1]);
    }
}