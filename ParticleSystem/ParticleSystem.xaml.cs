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

        public ParticleSystem()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            width = (int)this.ImgContainer.ActualWidth;
            height = (int)this.ImgContainer.ActualHeight;
            writeableBmp = Graphics3D.BitmapFactory.Create(width, height);
            this.ImgView.Source = writeableBmp;



            CompositionTarget.Rendering += Draw;
        }

        private void Draw(object sender, EventArgs e)
        {
        }

        private void Page_Unloaded(object sender, System.Windows.RoutedEventArgs e) => CompositionTarget.Rendering -= Draw;
    }
}
