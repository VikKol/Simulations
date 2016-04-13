using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Graphics3D;
using SharpDX;
using Simulations.Common;

namespace Simulations.GravityAttraction
{
    /// <summary>
    /// Interaction logic for SolarSystem.xaml
    /// </summary>
    public partial class SolarSystem : Window
    {
        int width;
        int height;
        private WriteableBitmap writeableBmp;
        private static readonly Random rand = new Random();

        private Device device;
        private Mesh[] meshes;
        private readonly Camera camera = new Camera();
        private readonly Light light = new AmbientLight(0.07f);

        private static Body Sun;
        private static Body[] Planets;

        public SolarSystem()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            width = (int)this.ImgContainer.ActualWidth;
            height = (int)this.ImgContainer.ActualHeight;
            writeableBmp = Graphics3D.BitmapFactory.Create(width, height);
            this.ImgView.Source = writeableBmp;

            light.Position = new Vector3(0, 0, 0);
            camera.Position = new Vector3(0, 20, -80);
            camera.Target = Vector3.Zero;
            device = new Device(camera, light, writeableBmp, new VisualizerFactory<TextureVisualizer>());

            meshes = MeshHelper.LoadFromJsonFile("GravityAttraction/meshes.babylon");

            Sun = new Body(Constants.SunMass, Vector3.Zero, meshes.First(m => m.Name == "Sun"));
            Sun.Color = SharpDX.Color.Yellow;

            var planetMeshes = meshes.Where(m => m.Name != "Sun").ToArray();
            Planets = new Body[planetMeshes.Length];
            for (int i = 0; i < Planets.Length; i++)
            {
                //TODO: make mass realistic, add random speed.
                var mass = rand.Next(5, 10);
                Planets[i] = new Body(mass, new Vector3(-0.05f, 0, 0.08f), planetMeshes[i]);
                Planets[i].Color = Helpers.GetRandomColor();
            }

            CompositionTarget.Rendering += Draw;
        }

        private void Draw(object sender, EventArgs e)
        {
            device.Clear(SharpDX.Color.Black);

            for (var i = 0; i < Planets.Length; i++)
            {
                var force = Sun.CalcGravityAttraction(Planets[i]);
                Planets[i].ApplyForce(force);
                Planets[i].Update();
            }

            device.Render(meshes);
            device.Present();
        }
    }
}
