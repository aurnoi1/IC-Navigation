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
    /// Interaction logic for PictureView.xaml
    /// </summary>
    public partial class PictureView : Page
    {
        public PictureView(string imgPath)
        {
            InitializeComponent();
            SetPicture(imgPath);
        }

        private void SetPicture(string imagePath)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
            bitmap.EndInit();
            this.ImageTested.Source = bitmap;
            var height = this.ImageTested.Source.Height;
            var width = this.ImageTested.Source.Width;
            this.ImageTested.RenderSize = new Size(width, height);
        }
    }
}
