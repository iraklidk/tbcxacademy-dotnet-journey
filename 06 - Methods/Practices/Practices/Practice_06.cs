public class Practice_06
{
    static char[] fillArr()
    {
        Console.Write("Enter size of array: ");
        int size = Convert.ToInt32(Console.ReadLine());
        char[] arr = new char[size];
        for(int i = 0; i < size; i++)
        {
            Console.Write("Enter char for index " + i + ": ");
            arr[i] = Convert.ToChar(Console.ReadLine());
        }
        return arr;
    }

    static int fun(char[] arr, char c)
    {
        int count = 0;
        foreach(char ch in arr)
        {
            if (ch == c) count++;
        }
        return count;
    }

    static void printRes(int count, char c)
    {
        Console.WriteLine($"'" + c + "' shegvxvda " + count + "-jer");
    }

    public static void Main4(string[] args)
    {
        char[] arr = fillArr();
        char ch = 'a';
        int count = fun(arr, ch);
        printRes(count, ch);
    }
}