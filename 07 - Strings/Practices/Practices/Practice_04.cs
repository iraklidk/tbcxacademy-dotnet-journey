using System.Text;
public class Practice_04
{
    public static int countWords(string s)
    {
        return s.Split(' ').Length;
    }
    public static void print(int n)
    {
        Console.WriteLine(n);
    }
    public static void Main4(string[] args)
    {
        Console.Write("Enter a string: ");
        string sentence = Console.ReadLine();
        int wordCount = countWords(sentence);
        print(wordCount);
    }
}