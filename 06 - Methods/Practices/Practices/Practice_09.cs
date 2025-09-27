public class Practice_09
{
    static Int32 prod(params int[] arr)
    {
        Int32 res = 1;
        foreach(int n in arr)
        {
            res *= n;
        }
        return res;
    }

    public static void Main7(string[] args)
    {
        int [] arr = new int[] { 1, 3, 4, 15, 13, 23, 98 };
        Console.WriteLine("The product of array elements is: " + prod(arr));
    }
}