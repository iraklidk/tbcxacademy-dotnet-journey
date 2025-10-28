internal class PointCalculations
{
    static double GetDistance((double x, double y) a, (double x, double y) b)
           => Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));

    static void Main2()
    {
        var tests = new ((double, double), (double, double))[]
        {
            ((0, 0), (3, 4)),
            ((-1, -1), (2, 3)),
            ((2.5, 4.5), (5.5, 8.5)),
            ((1000, 1000), (1003, 1004))
        };

        foreach (var (a, b) in tests) Console.WriteLine($"From {a} to {b} -> {GetDistance(a, b)}\n");
    }
}