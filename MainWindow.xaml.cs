using System.Windows;

namespace Simulations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SolarSystemSwitch_Checked(object sender, RoutedEventArgs e)
        {
            if (frameContent?.Content is GravityAttraction.SolarSystem)
            {
                return;
            }
            frameContent.Content = new GravityAttraction.SolarSystem();
        }

        private void ParticleSystemSwitch_Checked(object sender, RoutedEventArgs e)
        {
            if (frameContent?.Content is ParticleSystem.ParticleSystem)
            {
                return;
            }
            frameContent.Content = new ParticleSystem.ParticleSystem();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            frameContent.Content = new GravityAttraction.SolarSystem();
            SolarSystemSwitch.IsChecked = true;
        }
    }
}
