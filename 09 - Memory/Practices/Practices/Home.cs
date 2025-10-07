namespace Practices
{
    public class Home
    {
        public string Adress { get; set; }
        public string City { get; set; }

        public Home(string adress, string city)
        {
            Adress = adress;
            City = city;
        }
    }
}