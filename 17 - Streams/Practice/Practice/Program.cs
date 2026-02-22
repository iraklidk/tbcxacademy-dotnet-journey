using Practice;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

public class GeographyNow
{
    static void Main()
    {
        string file = "Cities.txt";

        List<City> cities = new List<City>();
        List<Country> countries = new List<Country>();

        Dictionary<string, List<City>> citiesByCountry = new Dictionary<string, List<City>>();

        string[] citiesFromFile = File.ReadAllLines(file);

        // insert city in List of cities from file
        foreach (string infoLine in citiesFromFile) 
        {
            string[] infos = infoLine.Split('|');
            City city = new City(infos[0], double.Parse(infos[1]), int.Parse(infos[2]), bool.Parse(infos[3]), infos[4]);
            cities.Add(city);
            if (!citiesByCountry.ContainsKey(infos[4])) citiesByCountry[infos[4]] = new List<City>();
            citiesByCountry[infos[4]].Add(city);
        }

        // insert country in List of countries
        foreach(var p in citiesByCountry)
        {
            countries.Add(new Country(p.Key, p.Value));
        }

        // check if each country has only one capital
        foreach(var p in citiesByCountry)
        {
            int c = 1;
            foreach (City city in p.Value) if (city.IsCapital) c--;
            if(c < 0) 
            {
                File.AppendAllText("log.txt", "A country must have exactly one capital city.");
            }
        }

        // main program
        Console.WriteLine("please choose one of them");
        while (true)
        {

            Console.WriteLine("1. Search Country");
            Console.WriteLine("2. Seach City");
            Console.WriteLine("3. Exit");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    GeographyHelper.SearchCountry(countries);
                    break;
                case "2":
                    GeographyHelper.SearchCity(cities);
                    break;
                case "3":
                    Console.WriteLine("Exit program..");
                    return;
                default:
                    Console.WriteLine("please insert 1 or 2");
                    break;
            }
        }
    }
}