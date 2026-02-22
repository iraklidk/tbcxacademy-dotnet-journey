public class Practice_08
{
    static int getInt()
    {
        Console.Write("Enter a positive number: ");
        return Convert.ToInt32(Console.ReadLine());
    }

    static void some(int n)
    {
        int tmp1 = n, tmp2 = n, count = 0;
        while(tmp1 > 0)
        {
            tmp1 /= 10;
            count++;
        }
        int[] digits = new int[count];
        int tmpCount = count;
        while(tmp2 > 0)
        {
            digits[--tmpCount] = tmp2 % 10;
            tmp2 /= 10;
        }
        Console.Write(n + " = ");
        for(int i = 0; i < digits.Length; i++)
        {
            if(i == 0) Console.Write(digits[i] + " * " + "10^" + (count - i - 1) );
            else Console.Write(" + " + digits[i] + " * " + "10^" + (count - i - 1) );
        }
    }

    // 1230 1 2 3 0

    public static void Main6(string[] args)
    {
        int k = getInt();
        some(k);
    }
}