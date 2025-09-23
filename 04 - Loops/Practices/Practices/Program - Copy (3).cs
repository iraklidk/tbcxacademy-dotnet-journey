public class Practice_01_026
{
    public static int sum(int n)
    {
        return n * (n + 1) / 2;
    }

    public static void Main3(string[] args)
    {
        Console.WriteLine(10); // 01

        int k = Convert.ToInt32(Console.ReadLine());


        Console.WriteLine("Sum from 1 to " + k + " is: ");
        Console.WriteLine(sum(k)); // 02
    }
}