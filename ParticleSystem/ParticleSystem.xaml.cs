using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Graphics3D;
using SharpDX;

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

        const int ParticlesCount = 200;
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
                particles[i] = new Particle
                {
                    Radius = 10,
                    Color = particleColors[rand.Next(0, particleColors.Length)],
                    Position = new Vector2(rand.Next(20, width - 20), rand.Next(20, height - 20))
                };
            }

            CompositionTarget.Rendering += Draw;
        }

        private void Draw(object sender, EventArgs e)
        {
            using (var cxt = drawing.UsingDrawingContext())
            {
                drawing.Clear(SharpDX.Color.Black);

                foreach (var particle in particles)
                {
                    particle.Update();
                    particle.Display(drawing);
                }
            }
        }

        private void Page_Unloaded(object sender, System.Windows.RoutedEventArgs e) 
            => CompositionTarget.Rendering -= Draw;
    }
}
