using System.Text;
public class Practice_01
{
    public static int count(string s, string str)
    {
        int count = 0;
        StringBuilder sb = new StringBuilder();

        if (str.ToUpper() == "VOWEL")
        {
            foreach (char c in s)
            {
                if ("aeiouAEIOU".Contains(c))
                {
                    ++count;
                    sb.Append(c);
                }
            }
        }

        else if (str.ToUpper() == "CONSONANT")
        {
            foreach (char c in s)
            {
                if (!("aeiouAEIOU".Contains(c)) && (c > 'a' && c <= 'z') || (c > 'A' && c <= 'Z'))
                {
                    ++count;
                    sb.Append(c);
                }
            }
        }

        return count;
    }

    public static void printLetters(string s, string str)
    {
        StringBuilder sb = new StringBuilder();

        if (str.ToUpper() == "VOWEL")
        {
            foreach (char c in s)
            {
                if ("aeiouAEIOU".Contains(c))
                {
                    sb.Append(c + " ");
                }
            }
        }

        else if (str.ToUpper() == "CONSONANT")
        {
            foreach (char c in s)
            {
                if (!("aeiouAEIOU".Contains(c)) && (c > 'a' && c <= 'z') || (c > 'A' && c <= 'Z'))
                {
                    sb.Append(c + " ");
                }
            }
        }

        Console.WriteLine(sb.ToString());
    }

    public static void Main1(string[] args)
    {
        Console.Write("Enter a string: ");
        while (true)
        {
            string k = Console.ReadLine();
            if (k.Length > 0)
            {
                Console.Write("which do u want? ur answer should be \"vowel\" or \"consonant\": ");
                string str = Console.ReadLine(); str = str.ToLower();
                while (true)
                {        
                    if(str != "vowel" && str != "consonant")
                    {
                        Console.Write("Please input valid string! ur answer should be \"vowel\" or \"consonant\": ");
                        str = Console.ReadLine(); str = str.ToLower();
                    }
                    else break;
                }
                Console.WriteLine(str + " count: " + count(k, str));
                Console.Write(str + "s: ");
                printLetters(k, str);
            }
            else
            {
                Console.WriteLine("Please input valid string!");
            }
            if (k.Length > 0) break;
            Console.Write("Enter a nonempty string: ");
        }
    }
}