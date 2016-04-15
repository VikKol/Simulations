using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
    public partial class SolarSystem : Page
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

        private static Dictionary<string, KeyValuePair<int, SharpDX.Color>> colors = 
            new Dictionary<string, KeyValuePair<int, SharpDX.Color>>
            {
                { "Mercury", new KeyValuePair<int, SharpDX.Color>(4, SharpDX.Color.DarkGray) },
                { "Venus", new KeyValuePair<int, SharpDX.Color>(6, SharpDX.Color.DarkOrange) },
                { "Earth", new KeyValuePair<int, SharpDX.Color>(8, new SharpDX.Color(0x0A, 0xFA, 0xC2)) },
                { "Mars", new KeyValuePair<int, SharpDX.Color>(7, new SharpDX.Color(0xD6, 0x56, 0x2F)) },
                { "Jupiter", new KeyValuePair<int, SharpDX.Color>(10, new SharpDX.Color(0xFF, 0xAD, 0x5C)) },
                { "Saturn", new KeyValuePair<int, SharpDX.Color>(9, new SharpDX.Color(0xFF, 0xC2, 0x85)) },
                { "Torus", new KeyValuePair<int, SharpDX.Color>(9, SharpDX.Color.Silver) },
                { "Uranus", new KeyValuePair<int, SharpDX.Color>(7, new SharpDX.Color(0, 0xBF, 0xFF)) },
                { "Neptune", new KeyValuePair<int, SharpDX.Color>(6, new SharpDX.Color(0, 0x80, 0xFF)) },
                { "Pluto", new KeyValuePair<int, SharpDX.Color>(2, SharpDX.Color.Silver) },
            };

        public SolarSystem()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
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
                var mass = colors[meshes[i].Name].Key;
                var velocity = new Vector3(-0.03f, 0, 0.08f);
                Planets[i] = new Body(mass, velocity, planetMeshes[i]);
                Planets[i].Color = colors[meshes[i].Name].Value;
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

        private void Page_Unloaded(object sender, RoutedEventArgs e) => CompositionTarget.Rendering -= Draw;
    }
}
