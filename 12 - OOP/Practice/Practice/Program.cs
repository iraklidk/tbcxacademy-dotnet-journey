using Practice;
public class Practice_05
{ 
    public static void Main()
    {
        while (true)
        {
            VehicleMenu.ShowMainTypes();
            TransportType type = (TransportType)Convert.ToByte(Console.ReadLine());
            Console.WriteLine($"\nYou picked {type}! Now choose the specific type of vehicle:");

            switch (type)
            {
                case TransportType.Military:
                    VehicleMenu.ShowMilitaryOptions();
                    break;
                case TransportType.Public:
                    VehicleMenu.ShowPublicOptions();
                    break;
                case TransportType.Sport:
                    VehicleMenu.ShowSportOptions();
                    break;
                case TransportType.Utility:
                    VehicleMenu.ShowUtilityOptions();
                    break;
                default:
                    Console.WriteLine("Invalid option selected.");
                    break;
            }

            int specificChoice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            switch (type, specificChoice)
            {
                case (TransportType.Military, 1):
                    Console.WriteLine("You selected Tank.");
                    break;
                case (TransportType.Military, 2):
                    Console.WriteLine("You selected Submarine.");
                    break;
                case (TransportType.Public, 1):
                    Console.WriteLine("You selected Bus.");
                    break;
                case (TransportType.Public, 2):
                    Console.WriteLine("You selected Subway.");
                    break;
                case (TransportType.Sport, 1):
                    Console.WriteLine("You selected Sportcar.");
                    break;
                case (TransportType.Sport, 2):
                    Console.WriteLine("You selected Bike.");
                    break;
                case (TransportType.Utility, 1):
                    Console.WriteLine("You selected Truck.");
                    break;
                case (TransportType.Utility, 2):
                    Console.WriteLine("You selected Van.");
                    break;
                default:
                    Console.WriteLine("Invalid specific option selected.");
                    break;
            }

            VehicleMenu.GetVehicleFromUserInput();
            Console.WriteLine($"Well Done. Here is your transport specifications: ");
            VehicleMenu.ShowInfo(type, specificChoice);
            Console.WriteLine("\n---------------------------------------\n");
        }
    }
}