public class Practice_08
{
    public static void Main(string[] args)
    {
        Console.Write("Enter length of side: ");
        int col = Convert.ToInt32(Console.ReadLine());
        int[,] arr = new int[col, col];

        for (int j = col - 1; j >= 0; --j)
            for(int i = j - 1; i >= 0; --i)
                arr[i, j] = 1;

        for(int i = 0; i < col; ++i)
        {
            for(int j = 0; j < col; ++j)
                Console.Write(arr[i, j] + " ");

            Console.WriteLine();
        }

        for (int i = 0; i < col; ++i) // reset all values to zero
            for (int j = 0; j < col; ++j)
                arr[i, j] = 0;

        for (int i = col - 1; i >= 0; --i)
        {
            for (int j = col - i; j < col; ++j)
                arr[i, j] = 1;
        }
        Console.WriteLine();
        for (int i = 0; i < col; ++i)
        {
            for (int j = 0; j < col; ++j)
                Console.Write(arr[i, j] + " ");

            Console.WriteLine();
        }

        Console.WriteLine("\nJagged Array Implementation\n");

        int[][] jarray = new int[col][];
        for(int i = 0; i < col; ++i)
        {
            jarray[i] = new int[col];
        }

        for(int j = col - 1; j >= 0; --j)
            for (int i = j - 1; i >= 0; --i)
                jarray[i][j] = 1;

        for (int i = 0; i < col; ++i)
        {
            for (int j = 0; j < col; ++j)
                Console.Write(jarray[i][j] + " ");
            Console.WriteLine();
        }

        for(int i = 0; i < col; ++i)
            for (int j = 0; j < col; ++j)
                jarray[i][j] = 0;

        for(int i = col - 1; i >= 0; --i)
        {
            for (int j = col - i; j < col; ++j)
                jarray[i][j] = 1;
        }

        Console.WriteLine();

        for (int i = 0; i < col; ++i)
        {
            for (int j = 0; j < col; ++j)
                Console.Write(jarray[i][j] + " ");
            Console.WriteLine();
        }
    }
}