namespace Practices
{
    public class Cat
    {
        string _name, _breed, _sex;
        int _age;
        int _dose = 10;
        public int age 
        {
            get
            {
                return _age;
            }
            set
            {
                if (value < 0) Console.WriteLine("Invalid value");
                else _age = value;
            }
        }
        public string name { get; set; }
        public string breed { get; }
        public Cat()
        {
            Console.WriteLine("Creating cat object...");
            Console.Write("Enter name: ");
            string name = Console.ReadLine();
            _name = name;
            Console.Write("Enter breed: ");
            string breed = Console.ReadLine();
            _breed = breed;
            Console.Write("Enter age: ");
            int age = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter sex: ");
            string sex = Console.ReadLine();
            _sex = sex;
            Console.WriteLine("Cat object created.");

        }
        public void Meow()
        {
            Console.WriteLine("Meowing ...");
        }

        public void Eat(int weight)
        {
            int bite = ((weight + _dose - 1) / _dose);
            Console.WriteLine(_name + " start eating.");
            while(bite-- > 0) Console.WriteLine("Eating ...");
            Console.WriteLine(_name + " finished eating.");
        }
    }
}
