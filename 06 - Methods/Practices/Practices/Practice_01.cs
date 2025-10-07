public class Practice_01
{

    static int ValueAt(int[] arr, int idx)
    {
        if (idx < arr.Length && idx > -1) return arr[idx];
        return -1;
    }

    public static void Main1(string[] args)
    {
        int[] arr = new int[] { 9, 7, 5, 4, 3 };
        Console.WriteLine("Number at index 4 is: " + valueAt(arr, 4));
    }
}