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
        private const double _deadZone = 0.4d;
        private const double _decisionZone = 0.4d;
        private double _screenWidth = -1;
        public event EventHandler OnLike;
        public event EventHandler OnDislike;

        public SwiperControl()
        {
            InitializeComponent();
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            this.GestureRecognizers.Add(panGesture);
            var picture = new Picture();
            image.Source = new UriImageSource() { Uri = picture.Uri };
            DescriptionLabel.Text = picture.Description;

            LoadingLabel.SetBinding(IsVisibleProperty, "IsLoading");
            LoadingLabel.BindingContext = image;
            
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    PanStart();
                    break;
                case GestureStatus.Running:
                    PanRunning(e);
                    break;
                case GestureStatus.Completed:
                    PanCompleted();
                    break;
            }
        }

        private void CalculatePanState(double panX)
        {
            var halfScreenWidth = _screenWidth / 2;
            var deadZoneEnd = _deadZone * halfScreenWidth;
            if(Math.Abs(panX) < deadZoneEnd)
            {
                return;
            }
            var passedDeadZone = panX < 0 ? panX + deadZoneEnd : panX - deadZoneEnd;
            var decisionZoneEnd = _decisionZone * halfScreenWidth;
            var opacity = passedDeadZone / decisionZoneEnd;
            opacity = Clamp(opacity, -1, 1);
            LikeLabel.Opacity = opacity;
            DislikeLabel.Opacity = -opacity;
        }
        private bool ShouldImageExit()
        {
            var halfScreenWidth = _screenWidth / 2;
            var deadZoneEnd = _deadZone * halfScreenWidth;
            return Math.Abs(photo.TranslationX) > deadZoneEnd;

        }
        private void ExitImage()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var direction = photo.TranslationX < 0 ? -1 : 1;
                if(direction > 0)
                {
                    OnLike.Invoke(this, new EventArgs());
                }
                else
                {
                    OnDislike.Invoke(this, new EventArgs());
                }
                await photo.TranslateTo(photo.TranslationX +
                (_screenWidth * direction),
                photo.TranslationY, 200, Easing.CubicIn);
                var parent = Parent as Layout<View>;
                parent?.Children.Remove(this);
            });
        }
        private double Clamp (double value, double min, double max)
        {
            return value < min ? min : value > max ? max : value;
        }

        private void PanStart()
        {
            photo.ScaleTo(1.1, 100);
        }
        private void PanRunning(PanUpdatedEventArgs e)
        {

            
            photo.TranslationX = e.TotalX;
            photo.TranslationY = e.TotalY;
            photo.Rotation = photo.TranslationX / 25;

            CalculatePanState(e.TotalX);
        }

        private void PanCompleted()
        {
            if (ShouldImageExit())
            {
                ExitImage();
            }
            LikeLabel.Opacity = 0;
            DislikeLabel.Opacity = 0;
            photo.TranslateTo(0, 0, 250, Easing.SpringOut);
            photo.RotateTo(0, 250, Easing.SpringOut);
            photo.ScaleTo(1, 100);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if(Application.Current.MainPage == null)
            {
                return; 
            }
            _screenWidth = Application.Current.MainPage.Width;
        }
    }
}