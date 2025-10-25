using System.Runtime.Intrinsics.Arm;

public class Practice_07
{
    public static void Main4(string[] args)
    {
        Console.Write("Enter a number: ");
        int n = Convert.ToInt32(Console.ReadLine());

        int[] dp = new int[n];
        if(n >= 1) dp[0] = 0;
        if(n >= 2) dp[1] = 1;

        for(int i = 2; i < n; ++i)
        {
            dp[i] = dp[i - 1] + dp[i - 2];
        }

        foreach(int a in dp)
        {
            Console.Write(a + " ");
        }
    }
}