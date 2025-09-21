public class Practice_05
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter your day of birth: ");
        int d = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter your month of birth: ");
        string month = Console.ReadLine();

        if(d > 31 || d < 1) Console.WriteLine("Invalid input");
        else if ((month == "January" && d >= 20) || (month == "February" && d <= 18))
        {
            Console.WriteLine(d + " " + month + " is Aquarius");
        }
        else if ((month == "February" && d >= 19) || (month == "March" && d <= 20))
        {
            Console.WriteLine(d + " " + month + " is Pisces");
        }
        else if ((month == "March" && d >= 21) || (month == "April" && d <= 19))
        {
            Console.WriteLine(d + " " + month + " is Aries");
        }
        else if ((month == "April" && d >= 20) || (month == "May" && d <= 20))
        {
            Console.WriteLine(d + " " + month + " is Taurus");
        }
        else if ((month == "May" && d >= 21) || (month == "June" && d <= 20))
        {
            Console.WriteLine(d + " " + month + " is Gemini");
        }
        else if ((month == "June" && d >= 21) || (month == "July" && d <= 22))
        {
            Console.WriteLine(d + " " + month + " is Cancer");
        }
        else if ((month == "July" && d >= 23) || (month == "August" && d <= 22))
        {
            Console.WriteLine(d + " " + month + " is Leo");
        }
        else if ((month == "August" && d >= 23) || (month == "September" && d <= 22))
        {
            Console.WriteLine(d + " " + month + " is Virgo");
        }
        else if ((month == "September" && d >= 23) || (month == "October" && d <= 22))
        {
            Console.WriteLine(d + " " + month + " is Libra");
        }
        else if ((month == "October" && d >= 23) || (month == "November" && d <= 21))
        {
            Console.WriteLine(d + " " + month + " is Scorpio");
        }
        else if ((month == "November" && d >= 22) || (month == "December" && d <= 21))
        {
            Console.WriteLine(d + " " + month + " is Sagittarius");
        }
        else if ((month == "December" && d >= 22) || (month == "January" && d <= 19))
        {
            Console.WriteLine(d + " " + month + " is Capricorn");
        }
        else Console.WriteLine("Invalid input");
        }
}