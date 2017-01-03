using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using System.Collections.ObjectModel;
using System.Diagnostics;
using ScnViewGestures.Plugin.Forms;

namespace empfehlungsbund
{
    public class SwiperViewModel
    {
        public List<Job> jobs { get; set; }
        public int jobIndex { get; set; }
        public ObservableCollection<CarouselItem> carouselItems { get; set; }
    }

    public class CarouselItem
    {
        public string title { get; set; }
        public string preview { get; set; }
    }

    public partial class SwiperPage : ContentPage
    {
        private SwiperViewModel viewModel;
        private bool animationRunning = false;

        public SwiperPage(List<Job> jobs)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            viewModel = new SwiperViewModel
            {
                jobs = jobs,
                jobIndex = -1,
                carouselItems = new ObservableCollection<CarouselItem>(){
                    new CarouselItem {},
                    new CarouselItem { title = "Slide 2", preview = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet." },
                    new CarouselItem { title = "Slide 3", preview = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet." }
                }
            };

            NextJob();

            BindingContext = viewModel;

            CarouselView carouselView = new CarouselView
            {
                ItemTemplate = new DataTemplate(() => {
                    var titleLabel = new Label
                    {
                        Margin = new Thickness(0, 40, 0, 10),
                        HorizontalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        FontAttributes = FontAttributes.Bold
                    };
                    var previewLabel = new Label
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center
                    };

                    titleLabel.SetBinding(Label.TextProperty, "title");
                    previewLabel.SetBinding(Label.TextProperty, "preview");

                    StackLayout stackLayout = new StackLayout
                    {
                        Children =
                        {
                            titleLabel,
                            previewLabel
                        },
                    };

                    return stackLayout;
                })
            };

            carouselView.SetBinding(CarouselView.ItemsSourceProperty, "carouselItems");

            var dragViewGestures = new ViewGestures
            {
                Content = carouselView,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 40,
                BackgroundColor = Color.White
            };

            dragViewGestures.Drag += (s, e) =>
            {
                dragViewGestures.TranslationY += e.DistanceY;

                if (dragViewGestures.TranslationY > 0)
                {
                    UninterestingLabel.IsVisible = true;
                    InterestingLabel.IsVisible = false;
                    BackgroundLayer.Padding = new Thickness(0, dragViewGestures.TranslationY / 2, 0, 0);
                }
                else if (dragViewGestures.TranslationY < 0)
                {
                    UninterestingLabel.IsVisible = false;
                    InterestingLabel.IsVisible = true;
                    BackgroundLayer.Padding = new Thickness(0, 0, 0, -dragViewGestures.TranslationY / 2);
                }
            };

            dragViewGestures.TouchEnded += (s, e) =>
            {
                if (animationRunning)
                {
                    return;
                }

                if (dragViewGestures.TranslationY > 200)
                {
                    Settings.ignoredJobs.Add(viewModel.jobs[viewModel.jobIndex].id);

                    animationRunning = true;

                    dragViewGestures.TranslateTo(0, dragViewGestures.Height, 500, null);

                    var animation = new Animation(callback: d => BackgroundLayer.Padding = new Thickness(0, d, 0, 0),
                         start: BackgroundLayer.Padding.Top,
                         end: dragViewGestures.Height / 2,
                         easing: null);
                    animation.Commit(BackgroundLayer, "FinishSwiper", rate: 10, length: 500);

                    Device.StartTimer(TimeSpan.FromMilliseconds(1000), () => {
                        NextJob();
                        dragViewGestures.TranslationY = 0;
                        animationRunning = false;

                        return false;
                    });
                }
                else if (dragViewGestures.TranslationY < -200)
                {
                    animationRunning = true;

                    dragViewGestures.TranslateTo(0, -dragViewGestures.Height, 500, null);

                    var animation = new Animation(callback: d => BackgroundLayer.Padding = new Thickness(0, 0, 0, d),
                        start: BackgroundLayer.Padding.Bottom,
                        end: dragViewGestures.Height / 2,
                        easing: null);
                    animation.Commit(BackgroundLayer, "FinishSwiper", rate: 10, length: 500);

                    Device.StartTimer(TimeSpan.FromMilliseconds(1000), () => {
                        NextJob();
                        dragViewGestures.TranslationY = 0;
                        animationRunning = false;

                        return false;
                    });
                }
                else if (dragViewGestures.TranslationY > 0)
                {
                    var animation = new Animation(callback: d => BackgroundLayer.Padding = new Thickness(0, d, 0, 0),
                        start: BackgroundLayer.Padding.Top,
                        end: 0,
                        easing: null);
                    animation.Commit(BackgroundLayer, "ResetSwiper", rate: 10, length: 300);

                    dragViewGestures.TranslateTo(0, 0, 300, null);
                }
                else if (dragViewGestures.TranslationY < 0)
                {
                    var animation = new Animation(callback: d => BackgroundLayer.Padding = new Thickness(0, 0, 0, d),
                        start: BackgroundLayer.Padding.Bottom,
                        end: 0,
                        easing: null);
                    animation.Commit(BackgroundLayer, "ResetSwiper", rate: 10, length: 300);

                    dragViewGestures.TranslateTo(0, 0, 300, null);
                }
            };

            SwiperLayer.Children.Add(dragViewGestures);
        }

        private void NextJob()
        {
            if (viewModel.jobIndex + 1 > viewModel.jobs.Count - 1) {
                Navigation.PopAsync(false);
                return;
            }

            viewModel.jobIndex++;
            viewModel.carouselItems[0] = new CarouselItem { title = viewModel.jobs[viewModel.jobIndex].title, preview = viewModel.jobs[viewModel.jobIndex].preview };

            PaginationLabel.Text = (viewModel.jobIndex + 1) + " / " + viewModel.jobs.Count;
        }

        private async void OpenDashboard(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DashboardPage());
        }
    }
}
