namespace Practice_01_02
{
    public class Practice_01
    {
        static void Main()
        {
            Console.WriteLine("USING AN INTERFACE\n-----------------------");
            IShape[] shapes = new IShape[3];
            shapes[0] = InterfaceHelper.GetTriangleFromUser();
            shapes[1] = InterfaceHelper.GetCircleFromUser();
            shapes[2] = InterfaceHelper.GetQuadrilateralFromUser();

            foreach (IShape shape in shapes)
            {
                Console.WriteLine($"Area of ur {shape.GetType().Name}: {Math.Round(shape.CalculateArea(), 2)}");
                Console.WriteLine($"Perimeter of ur {shape.GetType().Name}: {Math.Round(shape.CalculatePerimeter(), 2)}");
            }
        }
    }
}