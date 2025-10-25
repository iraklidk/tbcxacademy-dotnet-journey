using Practices;

public class Practice_01_02_03
{
    public static void Main(string[] args)
    {
        Cat cat = new Cat();
        Console.Write("Enter food weight in grams: ");
        int weight = Convert.ToInt32(Console.ReadLine());
        cat.Eat(weight);
        Console.Write("Enter meowing count: ");
        int meowCount = Convert.ToInt32(Console.ReadLine());
        while (meowCount-- > 0) cat.Meow();
        Console.WriteLine();

        Triangle tr = new Triangle();
        Console.WriteLine("Perimeter of the triangle is: " + tr.Perimeter());
        Console.WriteLine("Area of the triangle is: " + tr.Area());
        Console.WriteLine();

        Clock clock = new Clock();
        clock.Hour = 1;
        clock.Minute = 0;
        clock.Second = 1;
        clock.GetCurrentTime();
        clock.decreaseSecond();
        clock.GetCurrentTime();
        clock.decreaseMinute();
        clock.GetCurrentTime();
        clock.decreaseMinute();


        //clock.GetCurrentTime();
        //clock.AddSecond();
        //clock.AddSecond();
        //clock.AddSecond();
        //clock.AddSecond();
        //clock.AddSecond();
        //clock.GetCurrentTime();
        //clock.AddMinute();
        //clock.GetCurrentTime();
        //clock.Hour = 23;
        //clock.GetCurrentTime();
        //clock.AddHour();
        //clock.GetCurrentTime();
    }
}