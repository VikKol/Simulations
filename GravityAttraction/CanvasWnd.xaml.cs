using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Simulations.Common;

namespace Simulations.GravityAttraction
{
    /// <summary>
    /// Interaction logic for CanvasWnd.xaml
    /// </summary>
    public partial class CanvasWnd : Window
    {
        int width;
        int height;
        private WriteableBitmap writeableBmp;
        private static readonly Random rand = new Random();

        private static Body Attractor;
        private static Body[] Objects = new Body[9];

        public CanvasWnd()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            width = (int)this.ImgContainer.ActualWidth;
            height = (int)this.ImgContainer.ActualHeight;
            writeableBmp = BitmapFactory.New(width, height);
            this.ImgView.Source = writeableBmp;

            const int AttractorMass = 8;
            const int AttractorSize = 100;
            var AttractorPosition = new Vector3D(
                (width / 2) - (AttractorSize / 2),
                (height / 2) - (AttractorSize / 2),
                0);

            Attractor = new Body(AttractorMass, AttractorSize, AttractorPosition, new Vector3D(0, 0, 0), Colors.Yellow);
            Objects[Objects.Length - 1] = Attractor;

            for (var i = 0; i < Objects.Length - 1; i++)
            {
                var size = rand.Next(10, 60);
                Objects[i] = new Body(
                    rand.Next(1, 5),
                    size,
                    new Vector3D(
                        ((i+1) * 50) - (size / 2),
                        (height / 2) - (size / 2),
                        0),
                    new Vector3D(1, 0, 1));
            }

            // Rendering loop
            CompositionTarget.Rendering += (s, eargs) => Draw();
        }

        private void Draw()
        {
            using (writeableBmp.GetBitmapContext())
            {
                writeableBmp.Clear(Colors.Black);

                for (var i = 0; i < Objects.Length - 1; i++)
                {
                    var force = Attractor.CalcGravityAttraction(Objects[i]);
                    Objects[i].ApplyForce(force);
                    Objects[i].Update();
                }

                foreach (var obj in Objects.OrderBy(it => it.Position.Z))
                {
                    obj.Display(writeableBmp);
                }
            }
        }
    }
}
