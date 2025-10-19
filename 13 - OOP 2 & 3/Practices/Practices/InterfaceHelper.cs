using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Practices
{
    internal static class InterfaceHelper
    {
        internal static Triangle GetTriangleFromUser()
        {
            Point p1 = null, p2 = null, p3 = null;
            while (GeometryHelper.AnyThreePointsCollinear(p1, p2, p3))
            {
                Console.WriteLine("Please enter non collinear points");
                Console.WriteLine("Please enter first point of triangle: ");
                double x1 = Convert.ToDouble(Console.ReadLine());
                double y1 = Convert.ToDouble(Console.ReadLine());
                p1 = new Point(x1, y1);
                Console.WriteLine("Please enter second point of triangle: ");
                double x2 = Convert.ToDouble(Console.ReadLine());
                double y2 = Convert.ToDouble(Console.ReadLine());
                p2 = new Point(x2, y2);
                Console.WriteLine("Please enter third point of triangle: ");
                double x3 = Convert.ToDouble(Console.ReadLine());
                double y3 = Convert.ToDouble(Console.ReadLine());
                p3 = new Point(x3, y3);
            }
            Console.WriteLine($"Points of ur triangle are: ({p1.X}, {p1.Y}), " +
                $"({p2.X}, {p2.Y}), ({p3.X}, {p3.Y})\n");
            return new Triangle(p1, p2, p3);
        }
        
        internal static Circle GetCircleFromUser()
        {
            Console.WriteLine("Please enter center point of cfircle: ");
            double x1 = Convert.ToDouble(Console.ReadLine());
            double y1 = Convert.ToDouble(Console.ReadLine());
            Point center = new Point(x1, y1);
            Console.WriteLine("Please enter some point fo cfircle: ");
            double x2 = Convert.ToDouble(Console.ReadLine());
            double y2 = Convert.ToDouble(Console.ReadLine());
            Point pointOnCircle = new Point(x2, y2);
            Console.WriteLine($"Center of ur circle ({center.X}, {center.Y}), " +
                $"some point of ur cirlce ({pointOnCircle.X}, {pointOnCircle.Y})\n");
            return new Circle(center, pointOnCircle);
        }

        internal static Quadrilateral GetQuadrilateralFromUser()
        {
            Point p1 = null, p2 = null, p3 = null, p4 = null;
            while (GeometryHelper.AnyThreePointsCollinear(p1, p2, p3, p4))
            {
                Console.WriteLine("Please enter non collinear points");
                Console.WriteLine("Please enter first point of quadrilateral: ");
                double x1 = Convert.ToDouble(Console.ReadLine());
                double y1 = Convert.ToDouble(Console.ReadLine());
                p1 = new Point(x1, y1);
                Console.WriteLine("Please enter second point of quadrilateral: ");
                double x2 = Convert.ToDouble(Console.ReadLine());
                double y2 = Convert.ToDouble(Console.ReadLine());
                p2 = new Point(x2, y2);
                Console.WriteLine("Please enter third point of quadrilateral: ");
                double x3 = Convert.ToDouble(Console.ReadLine());
                double y3 = Convert.ToDouble(Console.ReadLine());
                p3 = new Point(x3, y3);
                Console.WriteLine("Please enter fourth point of quadrilateral: ");
                double x4 = Convert.ToDouble(Console.ReadLine());
                double y4 = Convert.ToDouble(Console.ReadLine());
                p4 = new Point(x4, y4);
            }
            Console.WriteLine($"Points of your quadrilateral are ({p1.X}, {p1.Y}), " +
                  $"({p2.X}, {p2.Y}), ({p3.X}, {p3.Y}), ({p4.X}, {p4.Y})\n");
            return new Quadrilateral(p1, p2, p3, p4);
        }
    }
}
