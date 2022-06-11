using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UTeM_EComplaint
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjUyMDU2QDMyMzAyZTMxMmUzMFJaOXV3ZTJXdy9BVmFPbEJSQWdkMG5HKy94MVUvSU91MjhwV3JERnkzN0U9");
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
