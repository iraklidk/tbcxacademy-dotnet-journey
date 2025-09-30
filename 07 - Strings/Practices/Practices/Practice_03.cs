using System.Text;
public class Practice_03
{
    public static void Main3(string[] args)
    {
        Console.Write("Enter a string: ");
        string str = Console.ReadLine();
        string s = string.Join(" ", str.ToCharArray());
        Console.WriteLine(s);
    }
}