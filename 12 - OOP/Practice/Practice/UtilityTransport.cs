namespace Practice
{
    public abstract class UtilityTransport : Vehicle, IRefuelable
    {
        protected UtilityTransport(Manufacturer manufacturer) 
                                  : base(manufacturer)
        {
        }
        public abstract void Refuel();
    }

    public class Truck : UtilityTransport
    {
        
        public int LoadCapacity { get; set; }
        public Truck(Manufacturer manufacturer) 
                    : base(manufacturer)
        {
            FuelType = FuelType.Diesel;
            Speed = 90;
            PassengerCapacity = 3;
            LoadCapacity = 50;
        }
        public override void Refuel() => Console.WriteLine("Truck is refueling..");
        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($", Load Capacity: {LoadCapacity} tons");
        }
    }

    public class Van : UtilityTransport
    {
        
        public bool slidingDoors { get; set; } 
        public Van(Manufacturer manufacturer) 
                   : base(manufacturer)
        {
            FuelType = FuelType.Gasoline;
            Speed = 100;
            PassengerCapacity = 8;
            this.slidingDoors = true;
        }
        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($", Sliding Doors: " + (slidingDoors ? "Yes" : "No"));
        }
        public override void Refuel() => Console.WriteLine("Van is refueling..");
    }
}
