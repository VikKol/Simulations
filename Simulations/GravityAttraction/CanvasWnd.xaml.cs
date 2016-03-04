using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Simulations.GravityAttraction
{
    /// <summary>
    /// Interaction logic for CanvasWnd.xaml
    /// </summary>
    public partial class CanvasWnd : Window
    {
        private volatile bool shown = true;
        private readonly Random rand = new Random();

        public CanvasWnd()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            Draw();
        }

        private void Draw()
        {
            var width = (int) this.Width;
            var height = (int) this.Height;
            var movers = new Body[10];
            for (var i = 0; i < movers.Length; i++)
            {
                movers[i] = new Body(rand.Next(1, 10), rand.Next(width), rand.Next(height));
            }

            var writeableBmp = BitmapFactory.New(width, height);
            this.image.Source = writeableBmp;
            using (writeableBmp.GetBitmapContext())
            {
                while (shown)
                {
                    for (var i = 0; i < movers.Length; i++)
                    {
                        for (var j = 0; j < movers.Length; j++)
                        {
                            if (i != j)
                            {
                                var force = movers[j].CalcGravityAttraction(movers[i]);
                                movers[i].ApplyForce(force);
                            }
                        }
                        movers[i].Update();
                        movers[i].Display(writeableBmp);
                    }
                }
            } 
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            shown = false;
            base.OnClosing(e);
        }
    }
}
