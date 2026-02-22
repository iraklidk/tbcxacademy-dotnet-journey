public class Practice_06
{
    public static void Main3(string[] args)
    {
        Console.Write("Enter a positive integer: ");
        int k = Convert.ToInt32(Console.ReadLine());
        Console.Write("divisors of " + k + " are: ");
        for (int i = 1; i * i <= k; ++i)
        {
            if (k % i == 0)
            {
                Console.Write(i + " ");
                if(i != k / i) Console.Write(k / i + " "); // to avoid printing the square root twice if k is a perfect square
            }
        }
    }
}