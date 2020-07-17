using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinderSwipe.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TinderSwipe.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SwiperControl : ContentView
    {
        public SwiperControl()
        {
            InitializeComponent();
            var picture = new Picture();
            Photo.Source = new UriImageSource() { Uri = picture.Uri };
            DescriptionLabel.Text = picture.Description;
        }
    }
}