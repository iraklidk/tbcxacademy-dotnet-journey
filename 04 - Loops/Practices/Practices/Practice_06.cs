public class Practice_06
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter a positive integer: ");
        int k = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("divisors of " + k + " are: ");
        for (int i = 1; i <= k / 2; ++i)
        {
            if (k % i == 0)
            {
                Console.Write(i + " ");
            }
        }
        Console.WriteLine(k);
    }
}