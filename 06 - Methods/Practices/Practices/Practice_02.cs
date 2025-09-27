public class Practice_02
{

    static int digitSum(int[] arr, int idx)
    {
        if(idx < 0 || idx >= arr.Length) return -1;
        int n = arr[idx], sum = 0;
        while (n > 0)
        {
            sum += n % 10;
            n /= 10;
        }
        return sum;
    }

    public static void Main2(string[] args)
    {
        int[] arr = new int[] { 1, 2, 123, 739, 13, 23, 98 };
        Console.WriteLine("The sum of the digits of a number at index: 2 is " + digitSum(arr, 2));
    }
}