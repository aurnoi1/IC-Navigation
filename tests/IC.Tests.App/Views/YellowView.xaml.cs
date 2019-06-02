using System.Windows;
using System.Windows.Controls;

namespace IC.Tests.App.Views
{
    /// <summary>
    /// Interaction logic for AView.xaml
    /// </summary>
    public partial class YellowView : Page
    {
        public YellowView()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void BtnOpenMenuView_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MenuView());
        }
    }
}