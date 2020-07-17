using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinderSwipe.Controls;
using Xamarin.Forms;

namespace TinderSwipe
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            AddInitialPhotos();
        }
        private void AddInitialPhotos()
        {
            for (int i = 0; i < 10; i++)
            {
                InsertPhoto();
            }
        }
        private void InsertPhoto()
        {
            var photo = new SwiperControl();
            photo.OnDislike += Handle_OnDislike;
            photo.OnLike += Handle_OnLike;
            this.MainGrid.Children.Insert(0, photo);
        }
        private int _likeCount;
        private int _denyCount;
        private void UpdateGui()
        {
            likeLabel.Text = _likeCount.ToString();
            denyLabel.Text = _denyCount.ToString();
        }
        private void Handle_OnLike(object sender, EventArgs e)
        {
            _likeCount++;
            InsertPhoto();
            UpdateGui();
        }
        private void Handle_OnDislike(object sender, EventArgs e)
        {
            _denyCount++;
            InsertPhoto();
            UpdateGui();
        }
    }
}
