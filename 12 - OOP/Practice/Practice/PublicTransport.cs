namespace Practice
{
    abstract class PublicTransport : Vehicle, IRefuelable
    {
        public virtual void GoBackToStartLocation()
        {
            Console.WriteLine($"Public transport is returning to start location");
        }

        public abstract void Refuel();

        public byte Cost { get; set; }
        protected PublicTransport(Manufacturer manufacturer) 
                              : base(manufacturer)
        {
        }
        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.Write($", Cost: {Cost}");
        }
    }

    class Bus : PublicTransport
    {
        
        public override void GoBackToStartLocation()
        {
            Console.WriteLine($"Bus is returning to start location");
        }
        public short RouteNumber { get; set; }
        public Bus(Manufacturer manufacturer, short routeNumber) 
                   : base(manufacturer)
        {
            FuelType = FuelType.Diesel;
            Speed = 60;
            PassengerCapacity = 35;
            this.Cost = 1;
            RouteNumber = routeNumber;
        }
        public override void Refuel() => Console.WriteLine("Bus is refueling..");
        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.Write($", Route Number: {RouteNumber}");
        }
    }

    class Subway : PublicTransport
    {

        public short LineNumber { get; set; }
        public Subway(Manufacturer manufacturer) 
                      : base(manufacturer)
        {
            FuelType = FuelType.Electric;
            Speed = 80;
            PassengerCapacity = 200;
            this.Cost = 2;
            LineNumber = 17;
        }
        public override void Refuel() => Console.WriteLine("Subway is refueling..");
        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.Write($", Line Number: {LineNumber}");
        }
    }
}
