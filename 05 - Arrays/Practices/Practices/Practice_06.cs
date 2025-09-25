public class Practice_06
{
    public static void Main3(string[] args)
    {
        Console.Write("Enter array row size: ");
        int row = Convert.ToInt32(Console.ReadLine());
        Console.Write("Enter array column size: ");
        int col = Convert.ToInt32(Console.ReadLine());
        int[,] mat = new int[row, col];

        for (int i = 0; i < row; ++i)
        {
            for (int j = 0; j < col; ++j)
            {
                Console.Write("Enter number for index [" + i + "," + j + "]: ");
                mat[i, j] = Convert.ToInt32(Console.ReadLine());
            }
        }

        Console.WriteLine("Here is matrix view of multidimensional array");
        int c = 0;
        foreach(int k in mat) {
            Console.Write(k + ", ");
            c++;
            if(c % col == 0) Console.WriteLine();
        }

    }
}