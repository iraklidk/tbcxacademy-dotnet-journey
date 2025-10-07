public class Practice_01_02_03_04_05
{
    // 01
    static void PrintNumbersTail(int k, int n)
    {
        if (k > n) return;
        Console.Write(k + " ");
        PrintNumbersTail(k + 1, n);
    }

    static void PrintNumbersHead(int n)
    {
        if (n == 0) return;
        PrintNumbersHead(n - 1);
        Console.Write(n + " ");
    }

    // 02
    static void PrintSumTail(int n, int sum = 0)
    {
        if (n == 0)
        {
            Console.WriteLine(sum);
            return;
        }
        sum += n;
        PrintSumTail(n - 1, sum);
    }

    static int PrintSumHead(int n)
    {
        if (n == 0) return 0;
        int sum = n + PrintSumHead(n - 1);
        return sum;
    }

    // 03
    static void CountDigitsTail(int n, int count = 0)
    {
        if (n == 0)
        {
            Console.Write(count);
            return;
        }
        count++;
        CountDigitsTail(n / 10, count);
    }

    static int CountDigitsHead(int n)
    {
        if (n == 0) return 0;
        int count = 1 + CountDigitsHead(n / 10);
        return count;
    }

    // 04
    static void CalculatePower1(int a, int b, int result = 1)
    {
        if (b == 0)
        {
            Console.WriteLine(result);
            return;
        }
        result *= a;
        CalculatePower1(a, b - 1, result);
    }
    static int CalculatePower2(int a, int b, int res = 1)
    {
        if (b == 0) return res;
        return a * CalculatePower2(a, b - 1, res);
    }

    //05
    static bool isPalindrome(string s)
    {
        if(s.Length <= 1) return true;
        return s[0] == s[s.Length - 1] && isPalindrome(s.Substring(1, s.Length - 2));
    }

       public static void Main()
    {
        // 01
        Console.Write("Enter a positive integer: ");
        int n = Convert.ToInt32(Console.ReadLine());
        PrintNumbersTail(1, n);
        Console.WriteLine();
        PrintNumbersHead(n);
        Console.WriteLine("\n");
        // 02
        Console.Write("(tail rec)Sum from 1 to n is : ");
        PrintSumTail(n);
        Console.WriteLine("Sum from 1 to n is : " + PrintSumHead(n));
        // 03
        Console.Write("Enter a positive integer: ");
        n = Convert.ToInt32(Console.ReadLine());
        Console.Write("(tail)The number " + n + " has ");
        CountDigitsTail(n == 0 ? 1 : n);
        Console.WriteLine(" digits.");
        Console.Write("The number " + n + " has " + (n == 0 ? 1 : CountDigitsHead(n)) + " digits.\n");
        // 04
        Console.Write("Enter number a: ");
        int a = Convert.ToInt32(Console.ReadLine());
        Console.Write("Enter number b: ");
        int b = Convert.ToInt32(Console.ReadLine());
        Console.Write("(pwr1)a ^ b = ");
        CalculatePower1(a, b);
        Console.WriteLine("(pwr2)a ^ b = " + CalculatePower2(a, b));
        // 05
        Console.Write("Enter a string: ");
        string s = Console.ReadLine();
        if(isPalindrome(s)) Console.WriteLine("The string \"" + s + "\" is a palindrome.");
        else Console.WriteLine("The string \"" + s + "\" is not a palindrome.");
    }
}