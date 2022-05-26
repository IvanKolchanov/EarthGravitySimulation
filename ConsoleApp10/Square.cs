using System;
using System.Collections.Generic;
using System.Linq;

namespace EarthGravitySimulation
{
    class Square
    {
        private static int width = Program.width, height = Program.height;

        private int id = 0;
        private double x0, y0, side;
        private int angelInDegrees = 45;
        private Coordinates.Point A, B, C, D;

        public char[,] screen = new char[width, height];

        public double X0 { get { return x0; } private set { x0 = Coordinates.normalizeX(value); } }
        public double Y0 { get { return y0; } private set { y0 = Coordinates.normalizeY(value); } }
        public double Side { get { return side; } set { side = Coordinates.normalizeX(value); updateInfo(); } }
        public Square(double x0, double y0, double side)
        {
            X0 = x0;
            Y0 = y0;
            side = Coordinates.normalizeX(side);
            for (int j = 0; j <= height - 1; j++)
            {
                double y = Coordinates.jToY(j);
                for (int i = 0; i < width; i++)
                {
                    double x = Coordinates.iToX(i);
                    if ((y <= Y0 + side/2) && (y >= Y0 - side/2) && (x <= X0 + side/2) && (x >= X0 - side/2)) screen[i, j] = '@';
                    else screen[i, j] = ' ';
                }
            }
            A = new Coordinates.Point(X0 - side / 2, Y0 + side / 2);
            B = new Coordinates.Point(X0 + side / 2, Y0 + side / 2);
            C = new Coordinates.Point(X0 + side / 2, Y0 - side / 2);
            D = new Coordinates.Point(X0 - side / 2, Y0 - side / 2);
        }

        public void intersect(Body body)
        {
            if (body.y0 <= A.y && body.y0 >= C.y && body.x0 >= A.x && body.x0 <= B.x)
            {
                    
            }
            
            }
        }

        public void getIntersectionPoints(Body body)
        {
            List<Coordinates.Line> lines = new List<Coordinates.Line>();
            lines.Add(new Coordinates.Line(A, B));
            lines.Add(new Coordinates.Line(B, C));
            lines.Add(new Coordinates.Line(C, D));
            lines.Add(new Coordinates.Line(A, D));
            foreach (Coordinates.Line line in lines)
            {
                Coordinates.Direct direct = line.direct;
                double a = 1 + direct.k * direct.k;
                double b = 2 * direct.k * direct.b - 2 * body.x0 - 2 * direct.k * body.y0;
                double c = body.x0 * body.x0 + body.y0 * body.y0 + direct.b * direct.b - body.Radius * body.Radius - 2 * direct.b * body.y0;

                if (direct.k == int.MaxValue)
                {
                    a = 1;
                    b = -2 * body.y0;
                    c = body.y0 * body.y0 + Math.Pow((direct.b - body.y0), 2) - body.radius * body.radius;
                }
                double D = b * b - 4 * a * c;

                Console.SetCursorPosition(0, 0);
                if (D > 0)
                {
                    double t1 = -b + Math.Sqrt(D);
                    t1 /= 2 * a;
                    double t2 = -b - Math.Sqrt(D);
                    t2 /= 2 * a;
                    if (direct.k == 0 && body.X0 >= Math.Min(line.A.x, line.B.x) && body.X0 <= Math.Max(line.A.x, line.B.x))
                    {

                        body.isInAir = false;
                        body.vector.y = 0;
                        if (line.A.y >= body.y0)
                        {
                            body.y0 = line.A.y + body.Radius * 6;
                        }
                        else
                        {
                            body.y0 = line.A.y - body.Radius * 6;
                        }
                        
                    }else if (direct.k == int.MaxValue && body.Y0 <= Math.Max(line.A.y, line.B.y) && body.Y0 >= Math.Min(line.A.y, line.B.y))
                    {
                        body.vector.x = 0;
                    }
                    else if (D == 0)
                    {
                        double t = -b / (2 * a);
                    }
            }
        }
        
        public void updateScreen()
        {
            
        }

        public void updateInfo()
        {

        }
    }
}
