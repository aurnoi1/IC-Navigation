using System.Windows;
using System.Windows.Controls;

namespace IC.Tests.App.Views
{
    /// <summary>
    /// Interaction logic for AView.xaml
    /// </summary>
    public partial class RedView : Page
    {
        public RedView()
        {
            InitializeComponent();
        }

        private void Back_Clicked(object sender, RoutedEventArgs e)
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