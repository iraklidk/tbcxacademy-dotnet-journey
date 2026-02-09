namespace Practice
{
    internal class Customer
    {
        public short ID { get; private set; } = 0;
        public string Name { get; set; }
        public Customer(string name, short id) => (Name, ID) = (name, id);
    }
}