using System;
using System.Threading;
using System.Threading.Tasks;

namespace EarthGravitySimulation
{
    class Program
    {
        public static int width = 120;
        public static int height = 30;
        public static char[,] screen = new char[width, height];
        public static double aspect = (double)width / height;
        public static double pixelAspect = (double)8 / 16;

        private static void setup()
        {
            Console.CursorVisible = false;
            Console.Title = "Earth's gravity simulation";
            Console.SetWindowSize(width, height);
            Console.BufferWidth = width;
            for (int j = height - 1; j >= 0; j--)
            {
                for (int i = 0; i < width; i++)
                {
                    screen[i, j] = ' ';
                    DrawFigure.emptyArray[i, j] = ' ';
                }
            }
        }


        static void Main(string[] args)
        {
            setup();
            Body body = new Body(2, 0.4, 0.04, 1, new Vector(-0.08, 0), true);
            Body body2 = new Body(-2, 0.4, 0.04, 2, new Vector(0.08, 0), true);
            while (true)
            {
                DrawFigure.refresh();
                Task.Factory.StartNew(() =>
                {
                    if (Console.KeyAvailable)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.RightArrow:
                                Coordinates.deltaX += 0.2;
                                break;
                            case ConsoleKey.LeftArrow:
                                Coordinates.deltaX -= 0.2;
                                break;
                            case ConsoleKey.UpArrow:
                                Coordinates.deltaY += 0.2;
                                break;
                            case ConsoleKey.DownArrow:
                                Coordinates.deltaY -= 0.2;
                                break;
                            case ConsoleKey.OemPlus:
                                Coordinates.zoom -= 0.2;
                                break;
                            case ConsoleKey.OemMinus:
                                Coordinates.zoom += 0.2;
                                break;
                        }
                    }
                });
            }
        }
    }
}
