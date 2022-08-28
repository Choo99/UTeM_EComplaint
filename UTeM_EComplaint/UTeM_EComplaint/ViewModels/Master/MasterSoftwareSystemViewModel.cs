using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using Xamarin.Forms;

namespace UTeM_EComplaint.ViewModels
{
    internal class MasterSoftwareSystemViewModel : ViewModelBase
    {
        readonly int LOAD_SIZE = 8;

        bool isSelected;
        bool isAdd;

        SoftwareSystem selectedSoftwareSystem;
        SoftwareSystem previousSelectedSoftwareSystem;
        SoftwareSystem newSoftwareSystem;

        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }
        public bool IsAdd { get => isAdd; set => SetProperty(ref isAdd, value); }

        List<SoftwareSystem> softwareSystems;


        public ObservableRangeCollection<SoftwareSystem> SoftwareSystemList { get; }
        public SoftwareSystem SelectedSoftwareSystem
        {
            get => selectedSoftwareSystem;
            set
            {
                SetProperty(ref selectedSoftwareSystem, value);
                OnPropertyChanged();
            }
        }
        public SoftwareSystem PreviousSelectedSoftwareSystem
        {
            get => previousSelectedSoftwareSystem;
            set
            {
                SetProperty(ref previousSelectedSoftwareSystem, value);
                OnPropertyChanged();
            }
        }
        public SoftwareSystem NewSoftwareSystem
        {
            get => newSoftwareSystem;
            set
            {
                SetProperty(ref newSoftwareSystem, value);
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

        public MasterSoftwareSystemViewModel()
        {
            Title = "System";

            SoftwareSystemList = new ObservableRangeCollection<SoftwareSystem>();
            softwareSystems = new List<SoftwareSystem>();

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

                        await SoftwareSystemServices.EditSoftwareSystem(previousSelectedSoftwareSystem);
                        await Application.Current.MainPage.DisplayAlert("Success", "Your information has been saved", "OK");

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
            NewSoftwareSystem = new SoftwareSystem();
        }

        private async Task Add()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to save the information?", "YES", "NO");
                if (answer)
                {
                    IsBusy = true;

                    await SoftwareSystemServices.AddSoftwareSystem(NewSoftwareSystem);
                    await Application.Current.MainPage.DisplayAlert("Success", $"New category has been added", "OK");

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
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to delete this category?", "YES", "NO");
                if (answer)
                {
                    IsBusy = true;
                    await SoftwareSystemServices.DeleteSoftwareSystem(previousSelectedSoftwareSystem);
                    await Application.Current.MainPage.DisplayAlert("Success", "This category has been deleted", "OK");

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
            if (SoftwareSystemList.Count == softwareSystems.Count)
                return;

            int lastItemIndexed = SoftwareSystemList.Count;
            int nextItemIndexed = lastItemIndexed + LOAD_SIZE;

            if (nextItemIndexed > softwareSystems.Count)
                nextItemIndexed = softwareSystems.Count;

            for (int i = lastItemIndexed; i < nextItemIndexed; i++)
            {
                SoftwareSystemList.Add(softwareSystems[i]);
            }
        }

        async void getData()
        {
            try
            {
                int size = LOAD_SIZE;
                IsBusy = true;
                softwareSystems = await SoftwareSystemServices.GetAllSoftwareSystems();
                if (softwareSystems.Count < LOAD_SIZE)
                    size = softwareSystems.Count;
                SoftwareSystemList.ReplaceRange(softwareSystems.GetRange(0, size));
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
            var softwareSystem = arg as SoftwareSystem;
            if (softwareSystem == null)
                return;

            PreviousSelectedSoftwareSystem = softwareSystem;

            IsSelected = true;

            SelectedSoftwareSystem = null;
        }

        async Task Refresh()
        {
            IsBusy = true;
            getData();
            IsBusy = false;
        }
    }
}
