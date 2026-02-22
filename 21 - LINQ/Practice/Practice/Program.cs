using Practice;

public class LINQPractice
{
    static async Task Main()
    {
        string[] names = { "Giorgi", "Mariam", "Beka", "Dima", "Nutsa", "Nikoloz", "Nuka", "Lasha", "Anano" };
        Dictionary<int , (string, decimal)> products = new Dictionary<int, (string, decimal)>
        {
            { 1, ("Krombacher", 55.0m) },
            { 2, ("Coca-Cola", 2.5m) },
            { 3, ("Fanta", 2.5m) },
            { 4, ("Sprite", 2.5m) },
            { 5, ("Borjomi", 1.5m) },
            { 6, ("Nabeghlavi", 1.2m) }
        };

        await HelperClass.FillFiles(products, names);

        // read customers
        var customers = File.ReadAllLines("Customers.txt")
            .Select(line =>
            {
                var parts = line.Split('|');
                return new Customer(parts[1], short.Parse(parts[0]));
            })
            .ToList();

        // read orders
        var orders = File.ReadAllLines("Orders.txt")
            .Select(line =>
            {
                var parts = line.Split('|');
                return new Order(DateTime.ParseExact(parts[1], "yyyyMMdd", null), parts[2],
                    decimal.Parse(parts[3]), short.Parse(parts[4]), short.Parse(parts[0]));
            })
            .ToList();

        // generate the reports with LINQ
        HelperClass.GenerateReports(customers, orders);
    }
}