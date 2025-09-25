public class Practice_05
{
    public static void Main2(string[] args)
    {
        // 05 
        Console.Write("Enter array size: ");
        int size = Convert.ToInt32(Console.ReadLine());
        int[] arr = new int[size];
        for(int i = 0; i < size; ++i)
        {
            Console.Write("Enter number for index " + i + ": ");
            arr[i] = Convert.ToInt32(Console.ReadLine());
        }

        Console.WriteLine("Unique elements of array are");

        for(int i = 0; i < size; i++)
        {
            int c = 0;
            for (int j = 0; j < size; j++)
            {
                if (arr[i] == arr[j]) c++;
                if(c > 1) break;
            }
            if (c == 1) Console.WriteLine(arr[i]);
        }

    }
}