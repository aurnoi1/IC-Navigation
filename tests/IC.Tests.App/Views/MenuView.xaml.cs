using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
