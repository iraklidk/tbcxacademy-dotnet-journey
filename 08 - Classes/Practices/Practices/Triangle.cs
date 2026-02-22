namespace Practices
{
    internal class Triangle
    {
        double _a, _b, _c;
        public double a
        {
            get
            {
                return _a;
            }
            set
            {
                if (value <= 0 || value >= _b + _c) Console.WriteLine("Invalid value");
                else _a = value;
            }
        }

        public double b { 
            get
            {
                return _b;
            }
            set
            {
                if (value <= 0 || value >= _c + _a) Console.WriteLine("Invalid value");
                else _b = value;
            }
        }

        public double c
        {
            get
            {
                return _c;
            }
            set
            {
                if (value <= 0 || value >= _b + _a) Console.WriteLine("Invalid value");
                else _c = value;
            }
        }

        public Triangle()
        {
            while (true)
            {
                Console.Write("Enter side 1: ");
                int a = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter side 2: ");
                int b = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter side 3: ");
                int c = Convert.ToInt32(Console.ReadLine());
                if (a + b > c && a + c > b && b + c > a)
                {
                    _a = a;
                    _b = b;
                    _c = c;
                    break;
                }
                Console.WriteLine("It is not valid triangle");
            }
        }

        public double Perimeter()
        {
            return _a + _b + _c;
        }

        public double Area()
        {
            double halfPerimeter = (_a + _b + _c) / 2.0;
            return Math.Sqrt(halfPerimeter * (halfPerimeter - a) * (halfPerimeter - b) * (halfPerimeter - c));
        }
    }
}
