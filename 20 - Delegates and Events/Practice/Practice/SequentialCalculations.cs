public class Sequential
{

    public delegate decimal MathOperation(decimal a, decimal b);

    static void Main2()
    {
        MathOperation mathDelegate = Add;
        mathDelegate += Subtract;
        mathDelegate += Multiply;
        mathDelegate += Divide;

        mathDelegate(105, 5);
    }

    private static decimal Add(decimal a, decimal b) 
    {
        Console.WriteLine($"{a} + {b} = {a + b}");
        return a + b;
    }
    private static decimal Subtract(decimal a, decimal b)
    {
        Console.WriteLine($"{a} - {b} = {a - b}");
        return a - b;
    }
    private static decimal Multiply(decimal a, decimal b)
    {
        Console.WriteLine($"{a} * {b} = {a * b}");
        return a * b;
    }
    private static decimal Divide(decimal a, decimal b)
    {
        if (b == 0) throw new DivideByZeroException("Cannot divide by zero.");
        Console.WriteLine($"{a} / {b} = {a / b}");
        return a / b;
    }
}