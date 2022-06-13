using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using UTeM_EComplaint.Views;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace UTeM_EComplaint.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {

        string username = "";
        string password = "";
        bool isShow = false;
        bool isPassword = true;

        readonly int userID;
        readonly string role;

        public LoginViewModel()
        {
            
            loginCommand = new Command(login);
            userID = Preferences.Get("userID", 0);
            role = Preferences.Get("role", null);

            try
            {
                string token = DependencyService.Get<INotificationHelper>().GetToken();
                Console.WriteLine("Token: " + token);

            } catch (Exception ex)
            {
                Console.WriteLine("Errorr: " + ex.Message);
            }
            checkPreferences();
        }

        async Task<bool> validate()
        {
            if(username == null || password == null)
            {
                if (username == null)
                    await Application.Current.MainPage.DisplayAlert("Username","Username field cannot be empty","OK");
                else if(password == null)
                    await Application.Current.MainPage.DisplayAlert("Password","Password field cannot be empty","OK");
                return false;
            }
            else
                return true;
        }
        void checkPreferences()
        {
            if (userID != 0)
            {
                if (role == "staff")
                {
                    Shell.Current.GoToAsync($"//{nameof(StaffHomePage)}");
                }
                else if (role == "technician")
                {
                    Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
                }
                else if (role == "admin")
                {
                    Shell.Current.GoToAsync($"//{nameof(AdminHomePage)}");
                }
            }
        }

        public ICommand loginCommand { get; }
        async void login()
        {
            try
            {
                if (! await validate())
                    return;
                IsBusy = true;
                User user = new User
                {
                    Username = username,
                    Password = password,
                };
                IsBusy = true;
                user = await UserServices.Login(user);
                
                Preferences.Set("role",user.Role);
                Preferences.Set("userID", user.UserID);

                if(user.Role == "technician")
                {
                    await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
                }
                else if(user.Role == "staff")
                {
                    await Shell.Current.GoToAsync($"//{nameof(StaffHomePage)}");
                }
                else if(user.Role == "admin")
                {
                    await Shell.Current.GoToAsync($"//{nameof(AdminHomePage)}");
                }
                DependencyService.Get<INotificationHelper>().UpdateInstanceID(user);
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert("Something went wrong", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                clearText();
            }
        }
        void clearText()
        {
            Username = null;
            Password = null;
        }

        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        public bool IsShow
        {
            get => isShow;
            set 
            {
                SetProperty(ref isShow, value);
                IsPassword = !value;
            }
        }
        public bool IsPassword
        {

            get => isPassword;
            set
            {
                SetProperty(ref isPassword, value);
            }
        }
    }
}
