namespace Practice
{
    internal static class HelperClass
    {
        private static readonly Random rnd = new Random();

        public static async Task FillFiles(Dictionary<int, (string, decimal)> products, string[] names)
        {
            await FillCustomersFile(names);
            await FillOrdersFIle(products, names);
        }

        // fills Customers.txt file
        public static async Task FillCustomersFile(string[] names)
        {
            string filePath = "Customers.txt";

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // write each customer to the file in the format: CustomerID|CustomerName
                for (int i = 0; i < names.Length; ++i) 
                    await writer.WriteLineAsync($"{i + 1}|{names[i]}");  
            }
        }

        // fills Orders.txt file
        public static async Task FillOrdersFIle(Dictionary<int, (string, decimal)> products, string[] names)
        {
            string filePath = "Orders.txt";

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                int orderCount = rnd.Next(20, 40);
                for(int i = 0; i < orderCount; ++i)
                {
                    // pick random customer, product and date
                    int customer = rnd.Next(0, names.Length);
                    int randomKey = rnd.Next(1, products.Count + 1);
                    (string, decimal) product = products[randomKey];
                    DateTime start = new DateTime(2023, 1, 1);
                    DateTime end = DateTime.Today;
                    string date = GetRandomDate(start, end).ToString("yyyyMMdd");
                    await writer.WriteLineAsync($"{i + 1}|{date}|{product.Item1}|{product.Item2}|{customer + 1}");
                }
            }
        }

        // generate random date
        public static DateTime GetRandomDate(DateTime start, DateTime end)
        {
            int range = (end - start).Days;
            return start.AddDays(rnd.Next(range + 1));
        }

        public static void GenerateReports(List<Customer> customers, List<Order> orders)
        {
            // 1. each customer's order count
            var orderCounts = orders
                .GroupBy(o => o.CustomerID)
                .Select(g => new { CustomerID = g.Key, OrderCount = g.Count() });

            Console.WriteLine("1. Customer order counts:");
            foreach (var c in orderCounts)
                Console.WriteLine($"CustomerID: {c.CustomerID}, OrderCount: {c.OrderCount}");
            Console.WriteLine();

            // 2. each customer's total order amount
            var sumAmounts = orders
                .GroupBy(o => o.CustomerID)
                .Select(g => new { CustomerID = g.Key, SumAmount = g.Sum(o => o.Price) });

            Console.WriteLine("2. Customer total order amount:");
            foreach (var c in sumAmounts)
                Console.WriteLine($"CustomerID: {c.CustomerID}, SumAmount: {c.SumAmount}");
            Console.WriteLine();

            // 3. each customer's minimum order price
            var minAmounts = orders
                .GroupBy(o => o.CustomerID)
                .Select(g => new { CustomerID = g.Key, MinAmount = g.Min(o => o.Price) });

            Console.WriteLine("3. Customer minimum order price:");
            foreach (var c in minAmounts)
                Console.WriteLine($"CustomerID: {c.CustomerID}, MinAmount: {c.MinAmount}");
            Console.WriteLine();

            // 4. only customers with more than 1 order
            var customersWithMultipleOrders = orders
                .GroupBy(o => o.CustomerID)
                .Where(g => g.Count() > 1)
                .Select(g => new { CustomerID = g.Key, OrderCount = g.Count() });

            Console.WriteLine("4. Customers with more than 1 order:");
            foreach (var c in customersWithMultipleOrders)
                Console.WriteLine($"CustomerID: {c.CustomerID}, OrderCount: {c.OrderCount}");
            Console.WriteLine();

            // 5. only customers whose average order > 10
            var customersWithHighAvg = orders
                .GroupBy(o => o.CustomerID)
                .Where(g => g.Average(o => o.Price) > 10)
                .Select(g => new { CustomerID = g.Key, AvgAmount = g.Average(o => o.Price) });

            Console.WriteLine("5. Customers with average order > 10:");
            foreach (var c in customersWithHighAvg)
                Console.WriteLine($"CustomerID: {c.CustomerID}, AvgAmount: {c.AvgAmount}");
            Console.WriteLine();
        }

    }
}