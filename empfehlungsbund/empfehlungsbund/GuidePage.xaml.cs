using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using System.Diagnostics;
using Plugin.Geolocator;

namespace empfehlungsbund
{
    public class GuideViewModel
    {
        public string query { get; set; }
        public string location { get; set; }
    }

    public partial class GuidePage : ContentPage
    {
        private GuideViewModel viewModel { get; set; }

        public GuidePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            viewModel = new GuideViewModel
            {
                query = "Ruby",
                location = "Dresden"
            };
            BindingContext = viewModel;
        }

        private async void Geolocation(object sender, EventArgs e)
        {
            LoadingLayer.IsVisible = true;

            try
            {
                var locator = CrossGeolocator.Current;

                if (!locator.IsGeolocationEnabled)
                {
                    LoadingLayer.IsVisible = false;
                    await DisplayAlert("Standortermittlung", "Bitte aktivieren Sie Ihre Standortermittlung.", "OK");

                    return;
                }

                var position = await locator.GetPositionAsync(timeoutMilliseconds: 20000);

                if (position == null)
                {
                    LoadingLayer.IsVisible = false;
                    await DisplayAlert("Standortermittlung", "Ihr Standort konnte leider nicht ermittelt werden.", "OK");

                    return;
                }

                await Api.ReverseGeocode(position.Latitude, position.Longitude, async (data) => {
                    LocationEntry.Text = data.city;

                    LoadingLayer.IsVisible = false;
                    await DisplayAlert("Standortermittlung", "Ermittelter Standort: " + data.city, "OK");
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                LoadingLayer.IsVisible = false;
                await DisplayAlert("Standortermittlung", "Ihr Standort konnte leider nicht ermittelt werden.", "OK");
            }
        }

        private async void Save(object sender, EventArgs e)
        {
            Settings.query = viewModel.query;
            Settings.location = viewModel.location;

            await Navigation.PopModalAsync();
        }
    }
}
