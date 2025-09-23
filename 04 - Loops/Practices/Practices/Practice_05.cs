public class Practice_05
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter a number of rows of Floyd's triangle to be printed: ");
        int k = Convert.ToInt32(Console.ReadLine());
        for(int i = 1; i <= k; ++i)
        {
            for(int j = 1; j <= i; ++j)
            {
                Console.Write((i + j - 1) % 2 + " ");
            }
            Console.WriteLine();
        }
    }
}