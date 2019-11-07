using IC.Tests.App.Views;
using System.Windows;

namespace IC.Tests.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        public MainWindow(string imgPath)
        {
            InitializeComponent();
            Loaded += (object sender, RoutedEventArgs e) => MainWindow_Loaded(sender, e, imgPath);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new MenuView());
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e, string imagePath)
        {
            this.SizeToContent = SizeToContent.WidthAndHeight;
            MainFrame.NavigationService.Navigate(new PictureView(imagePath));
        }
    }
}