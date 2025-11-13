public class Temp
{
    public class TemperatureMonitor
    {
        // delegate for temperature change handlers
        public delegate void TemperatureChangedHandler(double temperature);

        // event triggered when temeprature crosses critical limits
        public event TemperatureChangedHandler TemperatureCritical;

        private double _temperature;

        public double Temperature
        {
            get => _temperature;
            set
            {
                _temperature = value;
                CheckTemperature(_temperature);
            }
        }

        public void CheckTemperature(double temp)
        {
            // trigger event if temp < 0 or temp > 40
            if (temp < 0 || temp > 40)
            {
                TemperatureCritical?.Invoke(temp); // call subscribers
            }
        }
    }

    public class TemperatureAlert
    {
        public void OnTemperatureCritical(double temperature)
        {
            Console.WriteLine($"Critical temperature alert! Current temp: {temperature}°C");
        }
    }

    static void Main()
    {
        var monitor = new TemperatureMonitor();
        var alert = new TemperatureAlert();

        // subrscibe alert to the event
        monitor.TemperatureCritical += alert.OnTemperatureCritical;

        double[] temperatures = { 25, 42, 38, -5, 15 };
        foreach (var temp in temperatures) monitor.Temperature = temp;

        Console.WriteLine("Enter temperature values (type 'exit' to quit):");

        while (true)
        {
            Console.Write("Temperature: ");
            string input = Console.ReadLine();

            if (input.Trim().ToLower() == "exit") break;

            if (double.TryParse(input, out double temp)) monitor.Temperature = temp;
            else Console.WriteLine("Invalid input, please enter a number or 'exit'.");
        }

        Console.WriteLine("Program terminated.");
    }
}