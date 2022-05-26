﻿using System;

namespace EarthGravitySimulation
{
    public class Body
    {
        private static int width = Program.width;
        private static int height = Program.height;

        

        public int state = 0;
        public bool isInAir = true;

        public Vector vector = new Vector(0, 0);
        public double x0, y0, radius;
        public double mass;
        public double X0 { get { return x0; } private set { x0 = Coordinates.normalizeX(value); } }
        public double Y0 { get { return y0; } private set { y0 = Coordinates.normalizeY(value); } }
        public double Radius { get { return radius; } set { radius = Coordinates.normalizeX(value); } }

        public char[,] bodyScreen = new char[width, height];
        public Body(double x0, double y0, double radius, double mass, Vector vector, bool check)
        {
            this.vector.x = vector.x;
            this.vector.y = vector.y;
            this.mass = mass;
            X0 = x0;
            Y0 = y0;
            Radius = radius;
            updateScreen();
        }
        public void moveToVector(Vector vector)
        {
            this.vector.add(vector);
            if (y0 + this.vector.y <= -1)
            {
                y0 = -1 + 6 * Radius;
                this.vector.y = 0;
                isInAir = false;
            }
            else
            {
                y0 += this.vector.y;
            }
            x0 = this.vector.x + x0;
            updateScreen();
        }

        public void moveTo(double x, double y)
        {
            x0 = x;
            y0 = y;
            updateScreen();
        }

        public void updateScreen()
        {
            for (int j = 0; j <= height - 1; j++)
            {
                double y = Coordinates.jToY(j);
                for (int i = 0; i < width; i++)
                {
                    double x = Coordinates.iToX(i);
                    if (Math.Pow(x - X0, 2) + Math.Pow(y - Y0, 2) <= radius) bodyScreen[i, j] = '@';
                    else bodyScreen[i, j] = ' ';
                }
            }
        }
    }
}
