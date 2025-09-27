public class Practice_03_04_05
{
    //03

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

    // 04

    static double calculateAvg(int[] arr)
    {
        double sum = 0;
        foreach (int num in arr)
        {
            sum += num;
        }
        return sum / arr.Length;
    }

    // 05

    static void getFact(int[] arr, int n)
    {
        foreach(int num in arr)
        {
            if (num == n)
            {
                Console.WriteLine("Factorial of " + n + " is " + fact(n));
                return;
            }
        }
        Console.WriteLine("Number not found in the array");
    }

    static int fact(int n)
    {
        if (n == 0) return 1;
        return n * fact(n - 1);
    }

    public static void Main3(string[] args)
    {
        int[] arr = fillArr();
        int[] mnmx = minMax(arr);
        Console.WriteLine("The minimum number in the array is " + mnmx[0] + "\n" + "The maximum number in the array is " + mnmx[1]);
        double avg = calculateAvg(arr);
        Console.WriteLine("Arithmetic average of array is " + avg);
        getFact(arr, 12);
    }
}