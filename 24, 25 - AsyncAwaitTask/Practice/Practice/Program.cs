public class Program
{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; }
        public Order(int orderId, string orderStatus) => (OrderId, OrderStatus) = (orderId, orderStatus);
        Random random = new Random();

        public bool ValidatePayment(int orderId)
        {
            int sleepTime = random.Next(1000, 2001);
            Thread.Sleep(sleepTime);
            Console.WriteLine($"Order {orderId}: Payment validated.");
            return true;
        }

        public bool CheckInventory(int orderId)
        {
            int sleepTime = random.Next(1500, 2501);
            Thread.Sleep(sleepTime);
            Console.WriteLine($"Order {orderId}: Inventory Checked.");
            return true;
        }

        public void PrepareShipment(int orderId)
        {
            int sleepTime = random.Next(2000, 3001);
            Thread.Sleep(sleepTime);
            Console.WriteLine($"Order {orderId}: Shipment prepared.");
        }

        public async Task ProcessOrder(int orderId)
        {
            var paymentTask = Task.Run(() => ValidatePayment(OrderId));
            var inventoryTask = Task.Run(() => CheckInventory(OrderId));

            bool[] results = await Task.WhenAll(paymentTask, inventoryTask);

            if (results[0] && results[1]) PrepareShipment(OrderId);
            else Console.WriteLine($"Order {OrderId}: Failed.");
        }
    }

    static async Task Main()
    {
        List<Order> orders = new List<Order>();
        for (int i = 1; i <= 5; ++i) orders.Add(new Order(i, "Processing"));

        List<Task> tasks = new List<Task>();
        foreach (var order in orders) tasks.Add(order.ProcessOrder(order.OrderId));

        await Task.WhenAll(tasks);
    }
}