using System;

namespace Simulations.Common
{
    public static class DrawingExtensions
    {
        public static void RadialGradient(this Graphics3D.Drawing drawing, int x, int y, int radius, SharpDX.Color c)
        {
            int x1 = -radius, x2 = radius;
            int y1 = -radius, y2 = radius;

            for (int i = x1; i <= x2; i++)
            {
                for (int j = y1; j <= y2; j++)
                {
                    var currRadius = Math.Sqrt((i * i) + (j * j));
                    if (currRadius <= radius)
                    {
                        var color = c;
                        var gradient = 1 - (currRadius / radius);
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
