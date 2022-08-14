using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using UTeM_EComplaint.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UTeM_EComplaint.ViewModels
{
    internal class MasterDivisionViewModel : ViewModelBase
    {
        readonly int LOAD_SIZE = 8;

        bool isSelected;
        bool isAdd;

        Division selectedDivision;
        Division previousSelectedDivision;
        Division newDivision;

        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }
        public bool IsAdd { get => isAdd; set => SetProperty(ref isAdd, value); }

        List<Division> divisions;


        public ObservableRangeCollection<Division> DivisionList { get; }
        public Division SelectedDivision
        {
            get => selectedDivision;
            set
            {
                SetProperty(ref selectedDivision, value);
                OnPropertyChanged();
            }
        }
        public Division PreviousSelectedDivision
        {
            get => previousSelectedDivision;
            set
            {
                SetProperty(ref previousSelectedDivision, value);
                OnPropertyChanged();
            }
        }
        public Division NewDivision
        {
            get => newDivision;
            set
            {
                SetProperty(ref newDivision, value);
                OnPropertyChanged();
            }
        }

        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }
        public AsyncCommand LoadMoreCommand { get; }
        public AsyncCommand SaveCommand { get; }
        public AsyncCommand AddButtonCommand { get; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand DeleteCommand { get; }

        public MasterDivisionViewModel()
        {
            Title = "Division complaint";

            DivisionList = new ObservableRangeCollection<Division>();
            divisions = new List<Division>();

            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);
            RefreshCommand = new AsyncCommand(Refresh);
            LoadMoreCommand = new AsyncCommand(LoadMore);
            SaveCommand = new AsyncCommand(Save);
            AddButtonCommand = new AsyncCommand(AddButton);
            AddCommand = new AsyncCommand(Add);
            DeleteCommand = new AsyncCommand(Delete);

            getData();
        }

        private async Task Save()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to save the information?", "YES", "NO");
                if (answer)
                {
                    IsBusy = true;
                    if (answer)
                    {
                        IsBusy = true;
                        int result = await DivisionServices.EditDivision(previousSelectedDivision);

                        if (result != 0)
                        {
                            await Application.Current.MainPage.DisplayAlert("Success", "Your information has been saved", "OK");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Fail to edit", "OK");
                        }
                        IsSelected = false;
                        getData();
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task AddButton()
        {
            IsAdd = true;
            NewDivision = new Division();
        }

        private async Task Add()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to save the information?", "YES", "NO");
                if (answer)
                {
                    IsBusy = true;
                    int result = await DivisionServices.AddDivision(newDivision);

                    if (result != 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", $"New division with ID {result} has been added", "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Fail to add new division", "OK");
                    }
                    IsAdd = false;
                    getData();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task Delete()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to delete this division?", "YES", "NO");
                if (answer)
                {
                    IsBusy = true;
                    int result = await DivisionServices.DeleteDivision(previousSelectedDivision);

                    if (result != 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "This division has been deleted", "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Fail to delete", "OK");
                    }
                    IsSelected = false;
                    getData();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadMore()
        {
            if (DivisionList.Count == divisions.Count)
                return;

            int lastItemIndexed = DivisionList.Count;
            int nextItemIndexed = lastItemIndexed + LOAD_SIZE;

            if (nextItemIndexed > divisions.Count)
                nextItemIndexed = divisions.Count;

            for (int i = lastItemIndexed; i < nextItemIndexed; i++)
            {
                DivisionList.Add(divisions[i]);
            }
        }

        async void getData()
        {
            try
            {
                int size = LOAD_SIZE;
                IsBusy = true;
                divisions = await DivisionServices.GetDivisions();
                if (divisions.Count < LOAD_SIZE)
                    size = divisions.Count;
                DivisionList.ReplaceRange(divisions.GetRange(0, size));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task ItemSelected(object arg)
        {
            var division = arg as Division;
            if (division == null)
                return;

            PreviousSelectedDivision = division;

            IsSelected = true;

            SelectedDivision = null;
        }

        async Task Refresh()
        {
            IsBusy = true;
            getData();
            IsBusy = false;
        }
    }
}
