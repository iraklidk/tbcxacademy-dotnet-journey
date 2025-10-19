namespace Practice_01_02
{
    internal class Quadrilateral : IShape
    {
        public Quadrilateral(Point a, Point b, Point c, Point d) => (A, B, C, D) = (a, b, c, d);
        public Point A { get; set; }
        public Point B { get; set; }
        public Point C { get; set; }
        public Point D { get; set; }
        public double CalculateArea()
        {
            return GeometryHelper.CalculateQuadrilateralArea(A, B, C, D);
        }
        public double CalculatePerimeter()
        {
            return GeometryHelper.CalculateQuadrilateralPerimeter(A, B, C, D);
        }
    }
}