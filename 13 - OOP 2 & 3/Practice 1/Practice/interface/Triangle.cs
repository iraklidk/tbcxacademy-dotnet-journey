namespace Practice_01_02
{
    public class Triangle : IShape
    {
        public Triangle(Point a, Point b, Point c) => (A, B, C) = (a, b, c);
        public Point A { get; set; }
        public Point B { get; set; }
        public Point C { get; set; }
        public double CalculateArea()
        {
            return GeometryHelper.CalculateTriangleArea(A, B, C);
        }
        public double CalculatePerimeter()
        {
            return GeometryHelper.CalculateTrianglePerimeter(A, B, C);
        }
    }
}