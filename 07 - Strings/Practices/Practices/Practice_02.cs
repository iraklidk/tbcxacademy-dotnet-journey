using System.Text;
public class Practice_02
{
    public static string revString(string s) // way 1 iteration
    {
        StringBuilder sb = new StringBuilder();
        for (int i = s.Length - 1; i >= 0; --i)
        {
            sb.Append(s[i]);
        }
        return sb.ToString();
    }

    public static string rec(string s) // way 2 recursion (too slow for this case but I LIKE RECURSION)
    {
        string ret = "";
        if (s.Length <= 1)
        {
            return s;
        }
        ret += s[0];
        return rec(s.Substring(1)) + ret;
    }

    public static void Main2(string[] args)
    {
        Console.Write("Enter a string: ");
        string toRev = Console.ReadLine();
        Console.WriteLine(revString(toRev));

        Console.WriteLine(rec(toRev));
    }
}