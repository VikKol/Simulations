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

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            var wnd = new GravityAttraction.CanvasWnd();
            wnd.ShowDialog();
        }
    }
}
