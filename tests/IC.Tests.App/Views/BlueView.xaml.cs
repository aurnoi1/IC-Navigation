using System.Windows;
using System.Windows.Controls;

namespace IC.Tests.App.Views
{
    /// <summary>
    /// Interaction logic for AView.xaml
    /// </summary>
    public partial class BlueView : Page
    {
        public BlueView()
        {
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void BtnOpenYellowView_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new YellowView());
        }
    }
}