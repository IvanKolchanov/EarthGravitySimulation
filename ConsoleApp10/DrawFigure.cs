using System;
using System.Collections.Generic;
using System.Threading;

namespace EarthGravitySimulation
{
    class DrawFigure
    {
        public static List<Square> objects = new List<Square>();
        public static Body player = Program.player;
        private static int width = Program.width;
        private static int height = Program.height;
        private static DateTime dateTimeWas = DateTime.Now;
        public static char[,] emptyArray = new char[width, height];
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

        private static void collision()
        {
            foreach (var obj in objects)
            {
                obj.intersect(player);
            }
        }

        private static void applyGravity()
        {
            double deltaTime = (DateTime.Now - dateTimeWas).TotalMilliseconds / 40;
            dateTimeWas = DateTime.Now;
            Vector vector = new Vector(0, 0);
            if (player.isInAir)
            {
                vector = g.copy();
            }
            Vector vectorX = new Vector(0.05, 0);
            vectorX.multiply(player.state);
            vector.add(vectorX);
            vector.x = Math.Min(0.05, vector.x);
            vector.multiply(deltaTime);
            player.moveToVector(vector);
            collision();
            player.state = 0;
            player.vector.x = 0;
        }

        public static void putFiguresTogether()
        {
            pointsToDeleteNow = new List<PointOnScreen>(pointsToDelete);
            pointsToDelete.Clear();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (player.bodyScreen[i, j] == '@')
                    {
                        pointsToDelete.Add(new PointOnScreen(i, j));
                        Console.SetCursorPosition(i, j);
                        Console.Write('@');
                    }
                }
            }

            for (int z = 0; z < objects.Count; z++)
            {
                var body = objects[z];
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (body.screen[i, j] == '@')
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
