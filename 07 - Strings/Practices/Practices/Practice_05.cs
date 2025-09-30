using System.Text;
public class Practice_05
{
    public static int countLetters(string s)
    {
        int count = 0;
        foreach(char c in s) if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')) count++;
        return count;
    }

    public static int countNumbers(string s)
    {
        int count = 0;
        foreach (char c in s) if (c >= '1' && c <= '9') count++;
        return count;
    }

    public static void printInfo(string s)
    {
        Console.WriteLine("\"" + s + "\" -> Letters: " + countLetters(s) + ", Numbers: " + countNumbers(s) + ", Others: " + (s.Length - countLetters(s) - countNumbers(s)));

    }
    public static void Main5(string[] args)
    {
        Console.Write("Enter a string: ");
        string str = Console.ReadLine();
        printInfo(str);
    }
}