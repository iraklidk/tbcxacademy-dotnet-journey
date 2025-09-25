public class Practice_07
{
    public static void Main(string[] args)
    {
        Console.Write("Enter array row size: ");
        int row = Convert.ToInt32(Console.ReadLine());
        Console.Write("Enter array column size: ");
        int col = Convert.ToInt32(Console.ReadLine());
        int[,] mat1 = new int[row, col];
        Console.WriteLine("Fill first matrix");

        for (int i = 0; i < row; ++i)
        {
            for (int j = 0; j < col; ++j)
            {
                Console.Write("Enter number for index [" + i + "," + j + "]: ");
                mat1[i, j] = Convert.ToInt32(Console.ReadLine());
            }
        }

        int[,] mat2 = new int[row, col];
        Console.WriteLine("Fill second matrix");
        for (int i = 0; i < row; ++i)
        {
            for (int j = 0; j < col; ++j)
            {
                Console.Write("Enter number for index [" + i + "," + j + "]: ");
                mat2[i, j] = Convert.ToInt32(Console.ReadLine());
            }
        }

        Console.WriteLine("Here is sum of two matrices");
        for (int i = 0; i < row; ++i)
        {
            for(int j = 0; j < col; ++j)
            {
                Console.Write(mat1[i, j] + mat2[i, j] + ", ");
            }
            Console.WriteLine();
        }
    }
}