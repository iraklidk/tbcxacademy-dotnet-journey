namespace Practice_01_02
{
    internal class Circle : IShape
    {
        private Point pointOnCircle;

        public Circle(Point center, Point pointOnCircle) => (Center, CirclePoint)
            = (center, pointOnCircle);

        public Point Center { get; set; }
        public Point CirclePoint { get; set; }
        public double CalculateArea()
        {
            return GeometryHelper.CalculateCircleArea(Center, CirclePoint);
        }
        public double CalculatePerimeter()
        {
            return GeometryHelper.CalculateCirclePerimeter(Center, CirclePoint);
        }
    }
}