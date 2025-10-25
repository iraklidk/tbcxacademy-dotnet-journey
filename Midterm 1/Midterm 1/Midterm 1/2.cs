using System.Numerics;

public class Midterm_01_02
{

    public static void Sort(int[] arr) 
    {
        for (int i = 0; i < arr.Length - 1; ++i)
        {
            for (int j = i + 1; j < arr.Length; ++j)
            {
                if (arr[i] > arr[j])
                {
                    arr[i] = arr[j] ^ arr[i];
                    arr[j] = arr[j] ^ arr[i];
                    arr[i] = arr[j] ^ arr[i];
                }
            }
        }
    }

    public static bool CanBeProgression1(int[] arr)
    {
        if(arr.Length < 2) return true;
        Sort(arr);
        int d = arr[1] - arr[0];
        for (int i = 2; i < arr.Length; ++i)
        {
            if (arr[i] - arr[i - 1] != d) return false;
        }
        return true;
    }

    public static int SumOfArray(int[] arr)
    {
        int total = 0;

        foreach (int i in arr) total += i;

        return total;
    }

    public static bool CanBeProgression2(int[] arr)
    {
        double total = SumOfArray(arr);
        int n = arr.Length;
        int[] mnmx = GetMinMax(arr);
        int mn = mnmx[1];
        int mx = mnmx[0];

        double possibleSn = ((mn + mx) * n) / (double)2;
        return possibleSn == total;
    }


    public static int[] GetMinMax(int[] arr)
    {
        int mx = int.MinValue;
        int mn = int.MaxValue;

        foreach(int i in arr)
        {
            mx = Math.Max(mx, i);
            mn = Math.Min(mn, i);
        }
        int[] res = { mx, mn };
        return res;
    }


    public static void Main2()
    {
        int[] some = {7, 5, 3, 11, 9, 13, 15, 17, 19, 21};
        if(CanBeProgression1(some)) Console.WriteLine("Yes");
        else Console.WriteLine("No");

        if (CanBeProgression2(some)) Console.WriteLine("Yes");
        else Console.WriteLine("No");
    }
}