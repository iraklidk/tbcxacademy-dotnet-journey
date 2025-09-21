public class Practice_04
{
    public static void Main4(string[] args)
    {
        Console.WriteLine("Enter your birth year :");
        int y = Convert.ToInt32(Console.ReadLine());  
        switch(y % 12)
        {
            case 0: Console.WriteLine(y + " was Monkey year"); break;
            case 1: Console.WriteLine(y + " was Rooster year"); break;
            case 2: Console.WriteLine(y + " was Dog year"); break;
            case 3: Console.WriteLine(y + " was Pig year"); break;
            case 4: Console.WriteLine(y + " was Rat year"); break;
            case 5: Console.WriteLine(y + " was Ox year"); break;
            case 6: Console.WriteLine(y + " was Tiger year"); break;
            case 7: Console.WriteLine(y + " was Rabbit year"); break;
            case 8: Console.WriteLine(y + " was Dragon year"); break;
            case 9: Console.WriteLine(y + " was Snake year"); break;
            case 10: Console.WriteLine(y + " was Horse year"); break;
            case 11: Console.WriteLine(y + " was Sheep year"); break;
        }
    }
}