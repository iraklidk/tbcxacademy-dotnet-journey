using System.Numerics;

public class Midterm_01_01
{
    public static void solve(int[] arr, int target)
    {
        int left = 0, right = arr.Length - 1;
        while(left < right)
        {
            if (arr[left] + arr[right] == target) Console.WriteLine(arr[left] + ", " + arr[right]);
            if(arr[left] + arr[right] > target) right--;
            else left++;
        }
    }
    public static void test()
    {
        Console.WriteLine("test");
    }

    public static void Main1()
    {
        int[] some = { 1, 2, 3, 4, 5, 6, 7, 8 };
        Console.Write("input the targeT: ");
        int target = Convert.ToInt32(Console.ReadLine());

        solve(some, target);
    }
}