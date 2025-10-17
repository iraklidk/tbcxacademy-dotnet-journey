namespace Practice
{
    public abstract class Vehicle
    {
        Manufacturer Manufacturer { get; set; }
        public byte PassengerCapacity { get; set; }
        public short Speed{ get; set; }
        public FuelType FuelType { get; set; }

        public Vehicle(Manufacturer manufacturer) => (Manufacturer) = (manufacturer);
        public virtual void ShowInfo()
        {
            Console.Write($"Manufacturer: {Manufacturer}, " +
                              $"Passenger Capacity: {PassengerCapacity}, " +
                              $"Speed: {Speed} km/h, " +
                              $"Fuel Type: {FuelType}");
        }
    }
}