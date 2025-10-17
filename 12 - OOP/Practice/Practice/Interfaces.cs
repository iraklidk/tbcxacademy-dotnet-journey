namespace Practice
{
    internal interface IRefuelable
    {
        void Refuel();
    }
    internal interface IRestoration
    {
        void Repair()
        {
            Console.WriteLine("Vehicle is being repaired.");
        }
        void Refuel()
        {
            Console.WriteLine("Vehicle is being refueled.");
        }
        void Rearm();
    }
    public enum TransportType
    {
        Military = 1,
        Public,
        Sport,
        Utility
    }
    public enum Manufacturer
    {
        Boeing,
        Airbus,
        NorthropGrumman,
        BAEsystems,
        Toyota,
        Ford,
        BMW,
        Mercedes,
        Honda,
        Tesla,
        Volvo
    }
    public enum Weapon
    {
        Cannon, // Main weapons
        Howitzer,
        Autocannon,
        Torpedo, // Submarine weapon
        ATGM,           // Anti-Tank Guided Missile
        RocketLauncher  // Anti-tank weapon
    }

    public enum FuelType
    {
        Nuclear, // for submarines
        Gasoline,
        Diesel,
        Electric,
        Hybrid,
        PlugInHybrid,
        None // for bikes and other non-fuel vehicles
    }
}
