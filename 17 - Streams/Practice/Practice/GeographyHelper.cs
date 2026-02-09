
using System.Diagnostics.Metrics;

namespace Practice
{
    public static class GeographyHelper
    {
        internal static void SearchCity(List<City> cities)
        {
            City city = null;
            Console.Write("Please enter the city: ");
            string input = Console.ReadLine();

            foreach (City cityFromList in cities)
            {
                if (cityFromList.Name != input) continue;
                city = cityFromList;
                break;
            }

            if(city is null)
            {
                Console.WriteLine($"no city exist named {input}");
                return;
            }

            ShowCityInfo(city);
        }

        private static void ShowCityInfo(City city)
        {
            Console.WriteLine($"Name: {city.Name}\nArea: {city.Area}\nPopulation: {city.Population}\nCountry: {city.Country}");
            if(city.IsCapital) Console.WriteLine($"Capital of {city.Country}");
            Console.WriteLine();
        }

        internal static void SearchCountry(List<Country> countries)
        {
            Country country = null;
            Console.Write("Please enter the country: ");
            string input = Console.ReadLine();

            foreach (Country countryFromList in countries)
            {
                if (countryFromList.Name != input) continue;
                country = countryFromList;
                break;
            }

            if (country is null)
            {
                Console.WriteLine($"no country exist named {input}");
                return;
            }

            ShowCountryInfo(country);
        }

        private static void ShowCountryInfo(Country country)
        {
            Console.WriteLine($"Country: {country.Name}\nArea: {country.Area}\nPopulation: {country.Population}");
            Console.Write("Cities:");
            foreach(City city in country.Cities)
            {
                Console.Write(city.IsCapital ? $"{city.Name} (IsCapital), " : $"{city.Name}, ");
            }
            Console.WriteLine("\n");
        }
    }
}
