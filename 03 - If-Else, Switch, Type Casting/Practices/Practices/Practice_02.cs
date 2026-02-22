public class Practice_02
{
    public static void Main2(string[] args)
    {
        Console.WriteLine("Enter first number :");
        int a = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter second number :");
        int b = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter third number :");
        int c = Convert.ToInt32(Console.ReadLine());
        if (a + b > c && a + c > b && b + c > a) Console.WriteLine("This should be a triangle !");
        else Console.WriteLine("This shouldn't be a triangle !");
    }
}