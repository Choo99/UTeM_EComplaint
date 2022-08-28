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
    internal class MasterSubModuleViewModel : ViewModelBase
    {
        readonly int LOAD_SIZE = 8;

        bool isSelected;
        bool isAdd;

        int selectedSystemIndex;
        int selectedModuleIndex;

        SoftwareSystem softwareSystem;
        Module module;

        Submodule selectedSubmodule;
        Submodule previousSelectedSubmodule;
        Submodule newSubmodule;

        public int SelectedSystemIndex { get => selectedSystemIndex; set => SetProperty(ref selectedSystemIndex, value); }
        public int SelectedModuleIndex { get => selectedModuleIndex; set => SetProperty(ref selectedModuleIndex, value); }

        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }
        public bool IsAdd { get => isAdd; set => SetProperty(ref isAdd, value); }

        List<Submodule> submodules;


        public ObservableRangeCollection<SoftwareSystem> SoftwareSystemList { get; }
        public ObservableRangeCollection<Module> ModuleList { get; }
        public ObservableRangeCollection<Submodule> SubmoduleList { get; }
        public SoftwareSystem SoftwareSystem
        {
            get => softwareSystem;
            set
            {
                SetProperty(ref softwareSystem, value);
                getModule();
            }
        }
        public Module Module
        {
            get => module;
            set
            {
                SetProperty(ref module, value);
                getData();
            }
        }

        public Submodule SelectedSubmodule
        {
            get => selectedSubmodule;
            set
            {
                SetProperty(ref selectedSubmodule, value);
            }
        }
        public Submodule PreviousSelectedSubmodule
        {
            get => previousSelectedSubmodule;
            set
            {
                SetProperty(ref previousSelectedSubmodule, value);
            }
        }
        public Submodule NewSubmodule
        {
            get => newSubmodule;
            set
            {
                SetProperty(ref newSubmodule, value);
            }
        }

        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }
        public AsyncCommand LoadMoreCommand { get; }
        public AsyncCommand SaveCommand { get; }
        public AsyncCommand AddButtonCommand { get; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand DeleteCommand { get; }

        public MasterSubModuleViewModel()
        {
            Title = "System";

            SoftwareSystemList = new ObservableRangeCollection<SoftwareSystem>();
            ModuleList = new ObservableRangeCollection<Module>();
            SubmoduleList = new ObservableRangeCollection<Submodule>();

            submodules = new List<Submodule>();

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

        private async void getModule()
        {
            try
            {
                IsBusy = true;
                ModuleList.ReplaceRange(await ModuleServices.GetModulesBySystem(new Module
                {
                    System = softwareSystem
                }));
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

                        await SubmoduleServices.EditSubmodule(previousSelectedSubmodule);
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
            NewSubmodule = new Submodule();
        }

        private async Task Add()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to save the information?", "YES", "NO");
                if (answer)
                {
                    IsBusy = true;

                    NewSubmodule.Module = module;
                    await SubmoduleServices.AddSubmodule(NewSubmodule);
                    await Application.Current.MainPage.DisplayAlert("Success", $"New submodule has been added", "OK");

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
                    await SubmoduleServices.DeleteSubmodule(previousSelectedSubmodule);
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
            if (SubmoduleList.Count == submodules.Count)
                return;

            int lastItemIndexed = SubmoduleList.Count;
            int nextItemIndexed = lastItemIndexed + LOAD_SIZE;

            if (nextItemIndexed > submodules.Count)
                nextItemIndexed = submodules.Count;

            for (int i = lastItemIndexed; i < nextItemIndexed; i++)
            {
                SubmoduleList.Add(submodules[i]);
            }
        }

        async void getData()
        {
            try
            {
                IsBusy = true;

                if (SoftwareSystem == null || Module == null)
                    return;

                int size = LOAD_SIZE;
                submodules = await SubmoduleServices.GetSubmodulesByModule(new Submodule
                {
                    Module = Module
                });

                if (submodules.Count < LOAD_SIZE)
                    size = submodules.Count;
                SubmoduleList.ReplaceRange(submodules.GetRange(0, size));
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
            var submodule = arg as Submodule;
            if (submodule == null)
                return;

            PreviousSelectedSubmodule = submodule;

            IsSelected = true;

            SelectedSubmodule = null;
        }

        async Task Refresh()
        {
            IsBusy = true;
            getData();
            IsBusy = false;
        }
    }
}
