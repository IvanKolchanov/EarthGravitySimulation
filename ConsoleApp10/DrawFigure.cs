using System;
using System.Collections.Generic;
using System.Threading;

namespace EarthGravitySimulation
{
    class DrawFigure
    {
        private static int width = Program.width;
        private static int height = Program.height;
        private static DateTime dateTimeWas = DateTime.Now;
        public static char[,] emptyArray = new char[width, height];
        public static List<Body> bodies = new List<Body>();
        private static Vector g = new Vector(0, -0.02);
        private static List<PointOnScreen> pointsToDelete = new List<PointOnScreen>();
        private static List<PointOnScreen> pointsToDeleteNow = new List<PointOnScreen>();

        public class PointOnScreen
        {
            public int i, j;

            public PointOnScreen(int i, int j)
            {
                this.i = i;
                this.j = j;
            }
        }

        public static void refresh()
        {
            applyGravity();
            putFiguresTogether();
            Thread.Sleep(50);
        }

        private static void applyGravity()
        {
            List<Vector> vectors = new List<Vector>();
            double deltaTime = (DateTime.Now - dateTimeWas).TotalMilliseconds / 40;
            dateTimeWas = DateTime.Now;
            for (int i = 0; i < bodies.Count; i++)
            {
                Body bodyI = bodies[i];
                Vector vector = g.copy();
                if (bodyI.Y0 <= -1 + bodyI.Radius)
                {
                    bodyI.vector.y = -bodyI.vector.y * 0.8;
                    bodyI.vector.x *= 0.95;
                    vector = new Vector(0, 0);
                }
                for (int j = 0; j < bodies.Count; j++)
                {
                    if (j != i)
                    {
                        Body bodyJ = bodies[j];
                        Vector vector1 = new Vector(bodyJ.X0 - bodyI.X0, bodyJ.Y0 - bodyI.Y0);
                        double distance = vector1.getLength();
                        Vector vector2 = new Vector(0, 0);
                        if (distance <= bodyI.Radius + bodyJ.Radius)
                        {
                            double k = 0.1 / (bodyI.mass + bodyJ.mass);
                            vector2 = new Vector((bodyI.mass * bodyI.X0 + bodyJ.mass * (2 * bodyJ.X0 - bodyI.mass)) * k, (bodyI.mass * bodyI.Y0 + bodyJ.mass * (2 * bodyJ.Y0 - bodyI.mass)) * k);
                            vector.add(vector2);
                        }
                    }
                }
                vector.multiply(deltaTime);
                vectors.Add(vector);
            }
            for (int i = 0; i < bodies.Count; i++)
            {
                bodies[i].moveToVector(vectors[i]);
            }
        }

        public static void putFiguresTogether()
        {
            pointsToDeleteNow = new List<PointOnScreen>(pointsToDelete);
            pointsToDelete.Clear();
            for (int z = 0; z < bodies.Count; z++)
            {
                Body body = bodies[z];
                char[,] figure = body.bodyScreen;
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (figure[i, j] == '@')
                        {
                            pointsToDelete.Add(new PointOnScreen(i, j));
                            Console.SetCursorPosition(i, j);
                            Console.Write('@');
                        }
                    }
                }
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    double y = Coordinates.jToY(j);
                    if (y <= -1)
                    {
                        pointsToDelete.Add(new PointOnScreen(i, j));
                        Console.SetCursorPosition(i, j);
                        Console.Write('-');
                    }
                }
            }

                    for (int i = 0; i < pointsToDeleteNow.Count; i++)
            {
                PointOnScreen cursor = pointsToDeleteNow[i];
                if (!pointsToDelete.Exists(point => point.i == cursor.i && point.j == cursor.j))
                {
                    Console.SetCursorPosition(cursor.i, cursor.j);
                    Console.Write(' ');
                }
            }
        }
    }
}
