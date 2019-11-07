using System.Windows;
using System.Windows.Controls;

namespace IC.Tests.App.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : Page
    {
        public MenuView()
        {
            InitializeComponent();
        }

        private void BtnOpenRedView_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RedView());
        }

        private void BtnOpenBlueView_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new BlueView());
        }

        private void BtnOpenYellowView_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new YellowView());
        }
    }
}