using System.Diagnostics.Metrics;

namespace Practice
{
    internal class City : GeographicEntity
    {
        public City(string name, double area, int population, bool isCapital, string country) : base(name)
        {
            IsCapital = isCapital;
            Country = country;
            Area = area;
            Population = population;    
        }

        public bool IsCapital { get; }
        public string Country { get; }
    }
}
