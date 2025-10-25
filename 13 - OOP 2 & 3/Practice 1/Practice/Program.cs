using Practices;

public class Practice_01
{
    static void Main1()
    {
        Shape[] shapes = new Shape[3];
        shapes[0] = InterfaceHelper.GetTriangleFromUser();
        shapes[1] = InterfaceHelper.GetCircleFromUser();
        shapes[2] = InterfaceHelper.GetQuadrilateralFromUser();

        foreach (Shape shape in shapes)
        {
            Console.WriteLine($"Area of ur {shape.GetType().Name}: {Math.Round(shape.Area(), 2)}");
            Console.WriteLine($"Perimeter of ur {shape.GetType().Name}: {Math.Round(shape.Perimeter(), 2)}");
        }
    }
}