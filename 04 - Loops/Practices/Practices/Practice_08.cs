public class Practice_08
{
    public static void Main5(string[] args)
    {
        Console.Write("Enter a number: ");
        int k = Convert.ToInt32(Console.ReadLine()), tmp = k;
        string bin = "";
        if(k == 0) bin = "0";
        while (k > 0)
        {
            bin = (k % 2).ToString() + bin;
            k /= 2;
        }
        Console.Write("decimal " + tmp + " in binary is " + bin);
    }
}