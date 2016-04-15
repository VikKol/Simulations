using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Simulations.Common;
using Graphics3D;
using SharpDX;
using System.Threading.Tasks;

namespace Simulations.ParticleSystem
{
    /// <summary>
    /// Interaction logic for ParticleSystem.xaml
    /// </summary>
    public partial class ParticleSystem : Page
    {
        int width;
        int height;
        private WriteableBitmap writeableBmp;
        private BitmapDrawing drawing;

        static Random rand = new Random();

        static bool isLeftBtnDown = false;
        static bool isRightBtnDown = false;
        static Vector2 mouse = Vector2.Zero;

        const int ParticlesCount = 100;
        private Particle[] particles = new Particle[ParticlesCount];
        private SharpDX.Color[] particleColors = new[]
        {
            SharpDX.Color.Red,
            SharpDX.Color.Green,
            SharpDX.Color.Blue,
            SharpDX.Color.Yellow,
            SharpDX.Color.Violet,
            SharpDX.Color.Orange,
            SharpDX.Color.LightBlue
        };

        public ParticleSystem()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            width = (int)this.ImgContainer.ActualWidth;
            height = (int)this.ImgContainer.ActualHeight;
            writeableBmp = Graphics3D.BitmapFactory.Create(width, height);
            drawing = new BitmapDrawing(writeableBmp);
            this.ImgView.Source = writeableBmp;

            for(int i = 0; i < particles.Length; i++)
            {
                particles[i] = new Particle(
                    rand.Next(20, width - 20),
                    rand.Next(20, height - 20),
                    new Vector2(rand.Next(-4, 4), rand.Next(-4, 4)),
                    particleColors[rand.Next(0, particleColors.Length)]);
            }

            CompositionTarget.Rendering += Draw;
        }

        private void Draw(object sender, EventArgs e)
        {
            using (var cxt = drawing.UsingDrawingContext())
            {
                drawing.Clear(SharpDX.Color.Black);

                if (isLeftBtnDown) DrawLeftBtnShape(drawing);
                else if (isRightBtnDown) DrawRightBtnShape(drawing);

                Parallel.ForEach(particles, (particle) =>
                {
                    var force = Vector2.Zero;
                    if (isLeftBtnDown)
                    {
                        force = CalcAttractingForce(particle);
                    }
                    else if (isRightBtnDown)
                    {
                        force = CalcRepelForce(particle);
                    }
                    particle.ApplyForce(force);
                    particle.Update();
                    CheckEdges(particle);
                    particle.Display(drawing);
                });
            }
        }

        private Vector2 CalcAttractingForce(Particle p)
        {
            var direction = Vector2.Subtract(mouse, p.Position);
            direction.Normalize();
            return Vector2.Multiply(direction, 0.35f);
        }

        private Vector2 CalcRepelForce(Particle p) 
        {
            var direction = Vector2.Subtract(mouse, p.Position);
            var distance = direction.Length();
            direction.Normalize();
            distance = distance.Constrain(1, 100);
            var force = -1 * Constants.RepelForce / (distance * distance);

            return Vector2.Multiply(direction, force);
        }

        private void CheckEdges(Particle p)
        {
            float x = p.Velocity.X, y = p.Velocity.Y;

            if (p.Position.X <= 0 || p.Position.X >= width)
            {
                x = -p.Velocity.X;
            }
            if (p.Position.Y <= 0 || p.Position.Y >= height)
            {
                y = -p.Velocity.Y;
            }

            p.Velocity = new Vector2(x, y);
        }

        private void DrawLeftBtnShape(Graphics3D.Drawing drawing)
        {
            drawing.RadialGradient((int)mouse.X, (int)mouse.Y, 40, SharpDX.Color.LightGreen);
        }

        private void DrawRightBtnShape(Graphics3D.Drawing drawing)
        {
            drawing.RadialGradient((int)mouse.X, (int)mouse.Y, 40, SharpDX.Color.Aqua);
        }

        private void Page_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var position = e.GetPosition(this);
            mouse = new Vector2((float)position.X, (float)position.Y);
        }

        private void Page_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
            => isLeftBtnDown = true;
        private void Page_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
            => isRightBtnDown = true;
        private void Page_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
            => isLeftBtnDown = false;
        private void Page_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
            => isRightBtnDown = false;

        private void Page_Unloaded(object sender, System.Windows.RoutedEventArgs e)
            => CompositionTarget.Rendering -= Draw;
    }
}
