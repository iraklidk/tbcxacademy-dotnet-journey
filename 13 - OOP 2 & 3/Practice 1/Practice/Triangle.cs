using Practices;

internal class Triangle : Shape
{
    public Triangle(Point a, Point b, Point c) => (A, B, C) = (a, b, c);
    public Point A { get; set; }
    public Point B { get; set; }
    public Point C { get; set; }
    internal override double Area()
    {
        return GeometryHelper.CalculateTriangleArea(A, B, C);
    }
    internal override double Perimeter()
    {
        return GeometryHelper.CalculateTrianglePerimeter(A, B, C);
    }
}