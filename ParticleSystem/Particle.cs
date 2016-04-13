using System.Windows;
using SharpDX;

namespace Simulations.ParticleSystem
{
    struct Particle
    {
        public Vector Position;
        public Vector Velocity;
        public Vector Acceleration;

        public void Display(Graphics3D.Drawing drawing)
        {
            drawing.DrawPoint((int)Position.X, (int)Position.Y, Color.Blue);
        }
    }
}
