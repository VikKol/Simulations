using Simulations.Common;
using Graphics3D;
using SharpDX;

namespace Simulations.GravityAttraction
{
    class Body
    {
        public readonly int Mass;

        public SharpDX.Color Color
        {
            get { return mesh.Color; }
            set { mesh.Color = value; }
        }

        public string Name { get { return mesh.Name; } }

        private Vector3 velocity;
        private Vector3 acceleration;

        private readonly Mesh mesh;

        public Body(int mass, Vector3 speed, Mesh mesh)
        {
            this.Mass = mass;
            this.velocity = speed;
            this.acceleration = Vector3.Zero;
            this.mesh = mesh;
        }

        public void ApplyForce(Vector3 force)
        {
            var f = Vector3.Divide(force, this.Mass);
            this.acceleration = Vector3.Add(this.acceleration, f);
        }

        public Vector3 CalcGravityAttraction(Body obj)
        {
            var force = Vector3.Subtract(this.mesh.Position, obj.mesh.Position);
            var distance = force.Length();

            distance = distance.Constrain(20.0f, 50.0f);
            
            force.Normalize();
            var strength = (Constants.Gravity * this.Mass * obj.Mass) / (distance * distance);
            
            return Vector3.Multiply(force, strength);
        }

        public void Update()
        {
            this.velocity = Vector3.Add(this.velocity, this.acceleration);
            this.mesh.Position = Vector3.Add(this.mesh.Position, this.velocity);
            this.acceleration = Vector3.Multiply(this.acceleration, 0);
        }
    }
}
