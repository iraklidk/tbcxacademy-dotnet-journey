using Practices;

public class Program
{
    static void Main()
    {
        Person[] persons = new Person[3];

        for(int i = 1; i <= persons.Length; ++i)
        {
            Home home = new Home("Melikishvili St", "Tbilisi");
            persons[i - 1] = new Person(20 + i, "Irakli", i);
            persons[i - 1].Home = home;
        }

        Console.WriteLine(persons[0].Home.Adress + " " + persons[0].Home.City);
    }
}