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
    internal class MasterModuleViewModel : ViewModelBase
    {
        readonly int LOAD_SIZE = 8;

        bool isSelected;
        bool isAdd;

        SoftwareSystem softwareSystem;

        Module selectedModule;
        Module previousSelectedModule;
        Module newModule;

        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }
        public bool IsAdd { get => isAdd; set => SetProperty(ref isAdd, value); }

        List<Module> modules;


        public ObservableRangeCollection<SoftwareSystem> SoftwareSystemList { get; }
        public ObservableRangeCollection<Module> ModuleList { get; }
        public SoftwareSystem SoftwareSystem
        {
            get => softwareSystem;
            set
            {
                SetProperty(ref softwareSystem, value);
                OnPropertyChanged();
                getData();
            }
        }

        public Module SelectedModule
        {
            get => selectedModule;
            set
            {
                SetProperty(ref selectedModule, value);
                OnPropertyChanged();
            }
        }
        public Module PreviousSelectedModule
        {
            get => previousSelectedModule;
            set
            {
                SetProperty(ref previousSelectedModule, value);
                OnPropertyChanged();
            }
        }
        public Module NewModule
        {
            get => newModule;
            set
            {
                SetProperty(ref newModule, value);
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

        public MasterModuleViewModel()
        {
            Title = "System";

            SoftwareSystemList = new ObservableRangeCollection<SoftwareSystem>();

            ModuleList = new ObservableRangeCollection<Module>();
            modules = new List<Module>();

            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);
            RefreshCommand = new AsyncCommand(Refresh);
            LoadMoreCommand = new AsyncCommand(LoadMore);
            SaveCommand = new AsyncCommand(Save);
            AddButtonCommand = new AsyncCommand(AddButton);
            AddCommand = new AsyncCommand(Add);
            DeleteCommand = new AsyncCommand(Delete);

            getSystem();
        }

        private async void getSystem()
        {
            try
            {
                IsBusy = true;
                SoftwareSystemList.ReplaceRange(await SoftwareSystemServices.GetAllSoftwareSystems());
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

                        await ModuleServices.EditModule(previousSelectedModule);
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
            NewModule = new Module();
        }

        private async Task Add()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to save the information?", "YES", "NO");
                if (answer)
                {
                    IsBusy = true;

                    await ModuleServices.AddModule(NewModule);
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
                    await ModuleServices.DeleteModule(PreviousSelectedModule);
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
            if (ModuleList.Count == modules.Count)
                return;

            int lastItemIndexed = ModuleList.Count;
            int nextItemIndexed = lastItemIndexed + LOAD_SIZE;

            if (nextItemIndexed > modules.Count)
                nextItemIndexed = modules.Count;

            for (int i = lastItemIndexed; i < nextItemIndexed; i++)
            {
                ModuleList.Add(modules[i]);
            }
        }

        async void getData()
        {
            try
            {
                IsBusy = true;
                if (SoftwareSystem == null)
                    return;
                int size = LOAD_SIZE;
                modules = await ModuleServices.GetModulesBySystem(new Module
                {
                    System = SoftwareSystem
                }); 

                if (modules.Count < LOAD_SIZE)
                    size = modules.Count;
                ModuleList.ReplaceRange(modules.GetRange(0, size));
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
            var module = arg as Module;
            if (module == null)
                return;

            PreviousSelectedModule = module;

            IsSelected = true;

            SelectedModule = null;
        }

        async Task Refresh()
        {
            IsBusy = true;
            getData();
            IsBusy = false;
        }
    }
}
