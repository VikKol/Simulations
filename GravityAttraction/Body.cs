using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Simulations.Common;

namespace Simulations.GravityAttraction
{
    class Body
    {
        public readonly int Mass;
        public readonly int Size;

        public Vector3D Position { get; private set; }
        private Vector3D velocity;
        private Vector3D acceleration;
        private readonly Color color;

        public Body(int mass, int size, Vector3D pos, Vector3D speed, Color? color = null)
        {
            this.Mass = mass;
            this.Size = size;
            this.Position = pos;
            this.velocity = speed;
            this.acceleration = new Vector3D(0, 0, 0);

            if (color.HasValue)
            {
                this.color = color.Value;
            }
            else
            {
                var rcol = Helpers.GetRandomColor();
                this.color = Color.FromArgb(
                    (byte)((rcol & 0xFF000000) >> 24),
                    (byte)((rcol & 0x00FF0000) >> 16),
                    (byte)((rcol & 0x0000FF00) >> 8),
                    (byte) (rcol & 0x000000FF)
                );
            }
        }

        public void ApplyForce(Vector3D force)
        {
            var f = Vector3D.Divide(force, this.Mass);
            this.acceleration = Vector3D.Add(this.acceleration, f);
        }

        public Vector3D CalcGravityAttraction(Body obj)
        {
            var force = Vector3D.Subtract(this.Position, obj.Position);
            var distance = force.Length;

            distance = distance.Constrain(5.0, 25.0);
            
            force.Normalize();
            var strength = (Constants.G * this.Mass * obj.Mass) / (distance * distance);
            
            return Vector3D.Multiply(force, strength);
        }

        public void Update()
        {
            this.velocity = Vector3D.Add(this.velocity, this.acceleration);
            this.Position = Vector3D.Add(this.Position, this.velocity);
            this.acceleration = Vector3D.Multiply(this.acceleration, 0);
        }

        public void Display(WriteableBitmap g)
        {
            int x1 = (int)Math.Round(this.Position.X);
            int y1 = (int)Math.Round(this.Position.Y);
            int x2 = x1 + Size;
            int y2 = y1 + Size;

            g.DrawEllipse(x1, y1, x2, y2, Colors.DarkGray);
            g.FillEllipse(x1 + 1, y1 + 1, x2 - 1, y2 - 1, color);
        }
    }
}
