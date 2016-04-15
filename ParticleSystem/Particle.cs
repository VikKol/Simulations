using System;
using Simulations.Common;
using SharpDX;

namespace Simulations.ParticleSystem
{
    class Particle
    {
        const int maxSpeed = 10;

        private int mass;
        private int radius;
        private Color color;

        private Vector2 acceleration;
        public Vector2 Velocity { get; set; }
        public Vector2 Position { get; private set; }

        public Particle(int x, int y, Vector2 velocity, Color color, int radius = 10, int mass = 1)
        {
            this.Position = new Vector2(x, y);
            this.radius = radius;
            this.Velocity = velocity;
            this.color = color;
            this.mass = mass;
        }

        public void Update()
        {
            this.Velocity = Vector2.Add(this.Velocity, this.acceleration);

            var length = this.Velocity.Length();
            if (length > maxSpeed)
            {
                this.Velocity = Vector2.Divide(this.Velocity, length / maxSpeed);
            }

            this.Position = Vector2.Add(this.Position, this.Velocity);
            this.acceleration = Vector2.Multiply(this.acceleration, 0);
        }

        public void ApplyForce(Vector2 force)
        {
            force = Vector2.Divide(force, this.mass);
            this.acceleration = Vector2.Add(this.acceleration, force);
        }

        public void Display(Graphics3D.Drawing drawing)
        {
            drawing.RadialGradient((int)Position.X, (int)Position.Y, radius, this.color);
        }
    }
}
