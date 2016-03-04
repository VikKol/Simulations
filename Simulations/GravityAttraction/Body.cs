using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Simulations.GravityAttraction
{
    class Body
    {
        const int G = 1;

        private readonly int mass;
        private Vector position;
        private Vector velocity;
        private Vector acceleration;

        public Body(int mass, int x, int y)
        {
            this.mass = mass;
            this.position = new Vector(x, y);
            this.velocity = new Vector(0, 0);
            this.acceleration = new Vector(0, 0);
        }

        public void ApplyForce(Vector force)
        {
            var f = Vector.Divide(force, this.mass);
            this.acceleration = Vector.Add(this.acceleration, f);
        }

        public Vector CalcGravityAttraction(Body m)
        {
            var force = Vector.Subtract(this.position, m.position);
            var distance = force.Length;

            distance = Constrain(distance, 5.0, 25.0);
            
            force.Normalize();
            var strength = (G * this.mass * m.mass) / (distance * distance);
            
            return Vector.Multiply(force, strength);
        }

        public void Update()
        {
            this.velocity = Vector.Add(this.velocity, this.acceleration);
            this.position = Vector.Add(this.position, this.velocity);
            this.acceleration = Vector.Multiply(this.acceleration, 0);
        }

        public void Display(WriteableBitmap g)
        {
            var c = System.Drawing.Color.Blue;
            var color = Color.FromArgb(c.A, c.R, c.G, c.B);
            g.DrawEllipse((int)this.position.X, (int)this.position.Y, this.mass * 10, this.mass * 10, color);
        }

        private double Constrain(double distance, double l, double h)
        {
            if (distance < l) return l;
            if (distance > h) return h;
            return distance;
        }
    }
}
