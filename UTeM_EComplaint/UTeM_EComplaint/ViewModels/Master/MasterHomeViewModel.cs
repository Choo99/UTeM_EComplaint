using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UTeM_EComplaint.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UTeM_EComplaint.ViewModels
{
    internal class MasterHomeViewModel : ViewModelBase
    {
        readonly string PATH_TO_DAMAGE_TYPE = $"{nameof(MasterDamageTypePage)}";
        readonly string PATH_TO_DIVISION = $"{nameof(MasterDivisionPage)}";
        readonly string PATH_TO_CATEGORY = $"{nameof(MasterCategoryPage)}";
        readonly string PATH_TO_LOCATION = $"{nameof(MasterLocationPage)}";

        string dateTimeString;

        public AsyncCommand LogoutCommand { get; }
        public AsyncCommand ToDamageTypeCommand { get; }
        public AsyncCommand ToDivisionCommand { get; }
        public AsyncCommand ToCategoryCommand { get; }
        public AsyncCommand ToLocationCommand { get; }

        public string DateTimeString { get => dateTimeString; set => SetProperty(ref dateTimeString, value); }

        public MasterHomeViewModel()
        {
            DateTimeString = DateTime.Now.ToString("dd/MM/yyyy");

            LogoutCommand = new AsyncCommand(Logout);
            ToDamageTypeCommand = new AsyncCommand(ToDamageType);
            ToDivisionCommand = new AsyncCommand(ToDivision);
            ToCategoryCommand = new AsyncCommand(ToCategory);
            ToLocationCommand = new AsyncCommand(ToLocation);
        }

        private async Task Logout()
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Logout", "Are you sure you want to logout?", "YES", "NO");
            
            if (answer)
            {
                Application.Current.MainPage = new AppShell();
                Preferences.Clear();
            }
        }

        private async Task ToDamageType()
        {
            try
            {
                await Shell.Current.GoToAsync(PATH_TO_DAMAGE_TYPE);
            }catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private async Task ToDivision()
        {
            try
            {
                await Shell.Current.GoToAsync(PATH_TO_DIVISION);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private async Task ToCategory()
        {
            try
            {
                await Task.Delay(100);
                await Shell.Current.GoToAsync(PATH_TO_CATEGORY);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private async Task ToLocation()
        {
            try
            {
                await Task.Delay(100);
                await Shell.Current.GoToAsync(PATH_TO_LOCATION);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }
    }
}
