using System;
using System.Threading;
using System.Threading.Tasks;

namespace EarthGravitySimulation
{
    class Program
    {
        private const char V = ' ';
        public static int width = 120;
        public static int height = 30;
        public static char[,] screen = new char[width, height];
        public static double aspect = (double)width / height;
        public static double pixelAspect = (double)8 / 16;
        public static Body player = new Body(0.0, 0.0, 0.05, 1.0, new Vector(0, 0), true);

        private static void setup()
        {
            Console.CursorVisible = false;
            Console.Title = "Earth's gravity simulation";
            Console.SetWindowSize(width, height);
            Console.BufferWidth = width;
            try
            {
                for (int j = height - 1; j >= 0; j--)
                {
                    for (int i = 0; i < width; i++)
                    {
                        screen[i, j] = V;
                        DrawFigure.emptyArray[i, j] = V;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine(width + " " + height);
            }
        }


        static void Main(string[] args)
        {
            setup();
            Square square = new Square(-1, -0.625, 0.75);
            DrawFigure.objects.Add(square);
            while (true)
            {
                var tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;
                var task = new Task(() =>
                {
                    token.ThrowIfCancellationRequested();
                    while (!tokenSource.IsCancellationRequested)
                    {
                        if (player.state == 0)
                        {
                            switch (Console.ReadKey(true).Key)
                            {
                                case ConsoleKey.RightArrow:
                                    player.state = 1;
                                    break;
                                case ConsoleKey.LeftArrow:
                                    player.state = -1;
                                    break;
                                case ConsoleKey.Spacebar:
                                    if (!player.isInAir)
                                    {
                                        player.vector.y += 0.4;
                                        player.isInAir = true;
                                    }
                                    break;

                            }
                        }
                        break;
                    }
                }, token);

                task.Start();
                Thread.Sleep(30);
                tokenSource.Cancel();

                DrawFigure.refresh();
            }
        }
    }
}
