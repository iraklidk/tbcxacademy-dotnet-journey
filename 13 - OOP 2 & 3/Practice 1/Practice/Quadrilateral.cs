using Practices;

internal class Quadrilateral : Shape
{
    public Quadrilateral(Point a, Point b, Point c, Point d) => (A, B, C, D) = (a, b, c, d);
    public Point A { get; set; }
    public Point B { get; set; }
    public Point C { get; set; }
    public Point D { get; set; }
    internal override double Area()
    {
        return GeometryHelper.CalculateQuadrilateralArea(A, B, C, D);
    }
    internal override double Perimeter()
    {
        return GeometryHelper.CalculateQuadrilateralPerimeter(A, B, C, D);
    }
}