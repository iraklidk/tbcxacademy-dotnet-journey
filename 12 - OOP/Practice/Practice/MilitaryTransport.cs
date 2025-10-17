namespace Practice
{
    internal abstract class MilitaryTransport : Vehicle, IRestoration, IRefuelable
    {
        public abstract void Attack();
        public byte ArmorPercentage { get; set; } = 100;
        public Weapon weapon { get; set; }
        protected MilitaryTransport(Manufacturer manufacturer) 
                                   : base(manufacturer)
        {
        }
        public abstract void Rearm();
        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.Write($", Armor Percentage: {ArmorPercentage}%, Weapon: {weapon}");
        }

        public virtual void Refuel() => Console.WriteLine("Military transport is refueling.");
        
    }

    class Tank : MilitaryTransport
    {
        private int choice;

        public byte RotationSpeed { get; }
        public override void Attack() =>Console.WriteLine("Tank is attacking with " + weapon);
        
        public Tank(Manufacturer manufacturer) 
                   : base(manufacturer)
        {
            FuelType = FuelType.Diesel;
            Speed = 20;
            PassengerCapacity = 6;
            this.weapon = Weapon.Cannon;
            RotationSpeed = 10;
        }
        public override void Rearm() => Console.WriteLine($"Tank rearmed with {this.weapon}.");
        public override void Refuel() => Console.WriteLine("Tank is refueling.");
        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.Write($", Turret Rotation Speed: {RotationSpeed} degrees/sec");
        }
    }

    class Submarine : MilitaryTransport
    {
        public short MaxDepth { get; set; }
        public override void Attack() => Console.WriteLine("Submarine is attacking with " + weapon);
        
        public Submarine(Manufacturer manufacturer) 
                        : base(manufacturer)
        {
            FuelType = FuelType.Nuclear;
            Speed = 150;
            PassengerCapacity = 25;
            this.weapon = Weapon.Torpedo;
            MaxDepth = 350;
        }
        public override void Rearm() => Console.WriteLine($"Suvmarine rearmed with {this.weapon}.");
        public override void Refuel() => Console.WriteLine("Submarine is refueling.");
        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.Write($", Max Depth: {MaxDepth} meters");
        }
    }
}