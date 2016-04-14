using System;
using SharpDX;

namespace Simulations.ParticleSystem
{
    class Particle
    {
        public int Radius;
        public Color Color;

        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Acceleration;

        public void Update()
        {
        }

        public void Display(Graphics3D.Drawing drawing)
        {
            int x1 = -Radius, x2 = Radius;
            int y1 = -Radius, y2 = Radius;
            int x = (int)Position.X, y = (int)Position.Y;

            for (int i = x1; i <= x2; i++)
            {
                for (int j = y1; j <= y2; j++)
                {
                    var currRadius = Math.Sqrt((i * i) + (j * j));
                    if (currRadius <= Radius)
                    {
                        var color = Color;
                        var gradient = 1 - (currRadius / Radius);
                        color.B = (byte)(color.B * gradient);
                        color.R = (byte)(color.R * gradient);
                        color.G = (byte)(color.G * gradient);

                        drawing.DrawPoint(x + i, y + j, color);
                    }
                }
            }
        }
    }
}
