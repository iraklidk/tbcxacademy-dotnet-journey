namespace Practice
{
    public abstract class SportsTransport : Vehicle
    {
        protected SportsTransport(Manufacturer manufacturer) 
                                 : base(manufacturer)
        {
        }
    }

    public class SportsCar : SportsTransport, IRefuelable
    {
        
        public short HorsePower { get; set; }
        public SportsCar(Manufacturer manufacturer) 
                         : base(manufacturer)
        {
            FuelType = FuelType.Gasoline;
            Speed = 400;
            PassengerCapacity = 1;
            HorsePower = 700;
        }
        public void Refuel() => Console.WriteLine("Sport car is refueling..");
        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($", Horse Power: {HorsePower} hp");
        }
    }

    public class Bike : SportsTransport
    {

        public Bike(Manufacturer manufacturer) 
                       : base(manufacturer)
        {
            FuelType = FuelType.None;
            Speed = 30;
            PassengerCapacity = 1;
        }
        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.Write(". This is a bike, it does not require refueling.");
        }
    }
}
