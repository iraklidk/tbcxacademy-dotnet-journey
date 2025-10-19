using Practices;

internal class Circle : Shape
{
    private Point pointOnCircle;

    public Circle(Point center, Point pointOnCircle) => (Center, CirclePoint) 
        = (center, pointOnCircle);

    public Point Center { get; set; }
    public Point CirclePoint { get; set; }
    internal override double Area()
    {
        return GeometryHelper.CalculateCircleArea(Center, CirclePoint);
    }
    internal override double Perimeter()
    {
        return GeometryHelper.CalculateCirclePerimeter(Center, CirclePoint);
    }
}