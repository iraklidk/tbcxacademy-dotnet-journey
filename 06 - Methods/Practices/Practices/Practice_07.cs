public class Practice_07
{
    static int[,] fillArr()
    {
        Console.Write("Enter number of rows: ");
        int rows = Convert.ToInt32(Console.ReadLine());
        Console.Write("Enter number of columns: ");
        int cols = Convert.ToInt32(Console.ReadLine());
        int[,] arr = new int[rows, cols];
        for(int i = 0; i < rows; i++)
        {
            for(int j = 0; j < cols; j++)
            {
                Console.Write($"Enter integer for index {i},{j}: ");
                arr[i, j] = Convert.ToInt32(Console.ReadLine());
            }
        }
        return arr;
    }

    static int[,] sumMat(int[,] mat1, int[,] mat2)
    {
        int rows1 = mat1.GetLength(0);
        int cols1 = mat1.GetLength(1);
        int rows2 = mat2.GetLength(0);
        int cols2 = mat2.GetLength(1);
        if (rows1 != rows2 || cols1 != cols2)
        {
            Console.WriteLine("Matrices dimensions do not match");
            return new int[0,0];
        }
        int[,] res = new int[rows1, cols1];
        for(int i = 0; i < rows1; i++)
        {
            for(int j = 0; j < cols1; j++)
            {
                res[i, j] = mat1[i, j] + mat2[i, j];
            }
        }
        return res;
    }

    static void printMat(int[,] mat)
    {
        int rows = mat.GetLength(0);
        int cols = mat.GetLength(1);
        for(int i = 0; i < rows; i++)
        {
            for(int j = 0; j < cols; j++)
            {
                Console.Write(mat[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    public static void Main(string[] args)
    {
        int[,] arr1 = fillArr();
        int[,] arr2 = fillArr();
        int[,] sumArr = sumMat(arr1, arr2);
        printMat(sumArr);
    }
}