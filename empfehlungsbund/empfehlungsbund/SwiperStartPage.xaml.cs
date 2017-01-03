using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using System.Diagnostics;

namespace empfehlungsbund
{
    public partial class SwiperStartPage : ContentPage
    {
        public SwiperStartPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            OpenGuide();
        }

        private async void OpenGuide()
        {
            GuidePage guidePage = new GuidePage();
            guidePage.Disappearing += (sender, e) =>
            {
                OpenSwiper(null, null);
            };

            await Navigation.PushModalAsync(guidePage, false);
        }

        private async void OpenDashboard(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DashboardPage());
        }

        private async void OpenSwiper(object sender, EventArgs e)
        {
            LoadingLayer.IsVisible = true;

            try
            {
                await Api.SearchJobs(Settings.query, Settings.location, async (data) => {
                    LoadingLayer.IsVisible = false;

                    List<Job> jobs = new List<Job>();

                    foreach (Job job in data.jobs)
                    {
                        if (Settings.ignoredJobs.IndexOf(job.id) == -1)
                        {
                            jobs.Add(job);
                        }
                    }

                    if (jobs.Count > 0)
                    {
                        await Navigation.PushAsync(new SwiperPage(jobs), false);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                LoadingLayer.IsVisible = false;
                await DisplayAlert("Jobsuche", "Die aktuellen Stellenangebote konnten leider nicht abgerufen werden.", "OK");
            }
        }
    }
}
