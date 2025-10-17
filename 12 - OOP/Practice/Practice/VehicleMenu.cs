namespace Practice
{
    public static class VehicleMenu
    {
        public static void ShowMainTypes()
        {
            Console.Write("Store Loading...\n\nPick what kind of transport you wanna buy: \n");
            Console.WriteLine("1.Military\n2.Public\n3.Sport\n4.Utility");
        }

        public static void ShowMilitaryOptions() => Console.WriteLine("1.Tank\n2.Submarine");

        public static void ShowPublicOptions() => Console.WriteLine("1.Bus\n2.Subway");

        public static void ShowUtilityOptions() => Console.WriteLine("1.Truck\n2.Van");

        public static void ShowSportOptions() => Console.WriteLine("1.Sportcar\n2.Bike");

        static Manufacturer manufacturerName;
        public static void GetVehicleFromUserInput()
        {
            Console.WriteLine("\nEnter manufacturer:");
            foreach (var value in Enum.GetValues(typeof(Manufacturer)))
            {
                Console.WriteLine($"{(int)value} - {value}");
            }

            int choice = int.Parse(Console.ReadLine() ?? "0");
            manufacturerName = (Manufacturer)choice;

            Console.WriteLine(); //
        }

        public static void ShowInfo(TransportType type, int specificChoice)
        {
            switch (type, specificChoice)
            {
                case (TransportType.Military, 1):
                    Tank tank = new Tank((manufacturerName));
                    tank.ShowInfo();
                    break;
                case (TransportType.Military, 2):
                    Submarine submarine = new Submarine(manufacturerName);
                    submarine.ShowInfo();
                    break;
                case (TransportType.Public, 1):
                    Bus bus = new Bus(manufacturerName, 42);
                    bus.ShowInfo();
                    break;
                case (TransportType.Public, 2):
                    Subway subway = new Subway(manufacturerName);
                    subway.ShowInfo();
                    break;
                case (TransportType.Sport, 1):
                    SportsCar sportcar = new SportsCar(manufacturerName);
                    sportcar.ShowInfo();
                    break;
                case (TransportType.Sport, 2):
                    Bike bike = new Bike(manufacturerName);
                    bike.ShowInfo();
                    break;
                case (TransportType.Utility, 1):
                    Truck truck = new Truck(manufacturerName);
                    truck.ShowInfo();
                    break;
                case (TransportType.Utility, 2):
                    Van van = new Van(manufacturerName);
                    van.ShowInfo();
                    break;
                default:
                    Console.WriteLine("Invalid option selected.");
                    break;
            }
        }
    }
}