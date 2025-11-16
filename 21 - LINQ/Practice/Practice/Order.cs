namespace Practice
{
    internal class Order
    {
        public short ID { get; private set; }
        public DateTime Date { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
        public short CustomerID { get; set; }

        public Order(DateTime date, string product, decimal price, short customerId, short id)
                    => (Date, Product, Price, CustomerID) = (date, product, price, customerId);

    }
}