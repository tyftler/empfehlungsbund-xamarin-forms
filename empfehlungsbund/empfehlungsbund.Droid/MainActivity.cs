using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using ScnViewGestures.Plugin.Forms.Droid.Renderers;

namespace empfehlungsbund.Droid
{
    [Activity(Label = "empfehlungsbund", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, Theme = "@android:style/Theme.Holo.Light")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            ActionBar.SetIcon(Android.Resource.Color.Transparent);
            ViewGesturesRenderer.Init();

            LoadApplication(new App());
        }
    }
}

