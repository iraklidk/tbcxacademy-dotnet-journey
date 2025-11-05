namespace Practice
{
    internal class Country : GeographicEntity
    {
        public List<City> Cities { get; set; }
        public Country(string name, List<City> cities) : base(name)
        {
            foreach(City city in cities)
            {
                Area += city.Area;
                Population += city.Population;
            }
            Cities = cities;
        }
    }
}
