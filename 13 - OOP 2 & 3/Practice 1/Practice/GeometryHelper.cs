using System.Runtime.Serialization;

namespace Practices
{
    internal static class GeometryHelper
    {
        internal static double CalculateDistance(Point A, Point B)
        {
            return Math.Sqrt(System.Math.Pow(B.X - A.X, 2) +
                Math.Pow(B.Y - A.Y, 2));
        }

        // Triangle
        public static double CalculateTriangleArea(Point A, Point B, Point C)
        {
            double FirstSide = CalculateDistance(A, B);
            double SecondSide = CalculateDistance(B, C);
            double ThirdSide = CalculateDistance(C, A);
            double HalfPerimeter = (FirstSide + SecondSide + ThirdSide) / 2;
            return Math.Sqrt(HalfPerimeter * (HalfPerimeter - FirstSide) *
                (HalfPerimeter - SecondSide) * (HalfPerimeter - ThirdSide));
        }
        public static double CalculateTrianglePerimeter(Point A, Point B, Point C)
        {
            double FirstSide = CalculateDistance(A, B);
            double SecondSide = CalculateDistance(B, C);
            double ThirdSide = CalculateDistance(C, A);
            return FirstSide + SecondSide + ThirdSide;
        }

        // Circle
        public static double CalculateCircleRadius(Point Center, Point CirclePoint)
        {
            return CalculateDistance(Center, CirclePoint);
        }
        public static double CalculateCircleArea(Point Center, Point CirclePoint)
        {
            double Radius = CalculateCircleRadius(Center, CirclePoint);
            return Math.PI * Radius * Radius;
        }
        public static double CalculateCirclePerimeter(Point Center, Point CirclePoint)
        {
            double Radius = CalculateCircleRadius(Center, CirclePoint);
            return 2 * Math.PI * Radius;
        }

        // Quadrilateral
        public static double CalculateQuadrilateralArea(Point A, Point B, Point C, Point D)
        {
            double sum1 = A.X * B.Y + B.X * C.Y + C.X * D.Y + D.X * A.Y;
            double sum2 = A.Y * B.X + B.Y * C.X + C.Y * D.X + D.Y * A.X;
            return Math.Abs(sum1 - sum2) / 2.0;
        }
        public static double CalculateQuadrilateralPerimeter(Point A, Point B, Point C, Point D)
        {
            double FirstSide = CalculateDistance(A, B);
            double SecondSide = CalculateDistance(B, C);
            double ThirdSide = CalculateDistance(C, D);
            double FourthSide = CalculateDistance(D, A);
            return FirstSide + SecondSide + ThirdSide + FourthSide;
        }

        internal static bool AnyThreePointsCollinear(params Point[] points)
        {
            for (int i = 0; i < points.Length - 2; i++)
            {
                if (points[i] == null) return true;
                for (int j = i + 1; j < points.Length - 1; j++)
                {
                    for (int k = j + 1; k < points.Length; k++)
                    {
                        Point A = points[i];
                        Point B = points[j];
                        Point C = points[k];

                        double Area = A.X * (B.Y - C.Y) + B.X * (C.Y - A.Y) + C.X * (A.Y - B.Y);
                        if (Math.Abs(Area) < 1e-10)
                            return true;
                    }
                }
            }

            return false;
        }
    }
}