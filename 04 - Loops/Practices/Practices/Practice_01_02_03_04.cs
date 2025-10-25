public class Practice_01_02_03_04
{
    public static int sum(int n)
    {
        return (n * (n + 1)) / 2;
    }

    public static void Main1(string[] args)
    {
        // 01
        Console.WriteLine("Sum from 0 to 10 is " + sum(10)); 

        // 02
        Console.Write("Enter a positive integer: ");
        int k = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Sum from 1 to " + k + " is: " + sum(k)); 

        // 03
        Console.Write("Enter a positive integer: ");
        int n = Convert.ToInt32(Console.ReadLine());
        for (int i = 1; i <= n; ++i)
        {
            Console.WriteLine(i + " cubed is " + i * i * i);
        }

        // 04
        Console.Write("Enter a positive integgger: ");
        n = Convert.ToInt32(Console.ReadLine());
        n = (n & 1) == 1 ? n / 2 + 1 : n / 2; // count of odd numbers
        Console.WriteLine("Sum of odd numbers from 1 to " + n + " is: ");
        Console.WriteLine( ((2 + 2 * (n - 1)) * n) / 2 );
    }
}
