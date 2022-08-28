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
    internal class MasterSubmenuViewModel : ViewModelBase
    {
        readonly int LOAD_SIZE = 8;

        bool isSelected;
        bool isAdd;

        int selectedSystemIndex;
        int selectedModuleIndex;
        int selectedSubmoduleIndex;

        SoftwareSystem softwareSystem;
        Module module;
        Submodule submodule;

        Submenu selectedSubmenu;
        Submenu previousSelectedSubmenu;
        Submenu newSubmenu;

        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }
        public bool IsAdd { get => isAdd; set => SetProperty(ref isAdd, value); }

        List<Submenu> submenus;

        public int SelectedSystemIndex { get => selectedSystemIndex; set => SetProperty(ref selectedSystemIndex, value); }
        public int SelectedModuleIndex { get => selectedModuleIndex; set => SetProperty(ref selectedModuleIndex, value); }
        public int SelectedSubmoduleIndex { get => selectedSubmoduleIndex; set => SetProperty(ref selectedSubmoduleIndex, value);}

        public ObservableRangeCollection<SoftwareSystem> SoftwareSystemList { get; }
        public ObservableRangeCollection<Module> ModuleList { get; }
        public ObservableRangeCollection<Submodule> SubmoduleList { get; }
        public ObservableRangeCollection<Submenu> SubmenuList { get; }
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
                getSubmodule();
            }
        }

        public Submodule Submodule
        {
            get => submodule;
            set
            {
                SetProperty(ref submodule, value);
                getData();
            }
        }

        public Submenu SelectedSubmenu
        {
            get => selectedSubmenu;
            set
            {
                SetProperty(ref selectedSubmenu, value);
            }
        }
        public Submenu PreviousSelectedSubmenu
        {
            get => previousSelectedSubmenu;
            set
            {
                SetProperty(ref previousSelectedSubmenu, value);
            }
        }
        public Submenu NewSubmenu
        {
            get => newSubmenu;
            set
            {
                SetProperty(ref newSubmenu, value);
            }
        }

        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }
        public AsyncCommand LoadMoreCommand { get; }
        public AsyncCommand SaveCommand { get; }
        public AsyncCommand AddButtonCommand { get; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand DeleteCommand { get; }

        public MasterSubmenuViewModel()
        {
            Title = "System";

            SoftwareSystemList = new ObservableRangeCollection<SoftwareSystem>();
            ModuleList = new ObservableRangeCollection<Module>();
            SubmoduleList = new ObservableRangeCollection<Submodule>();
            SubmenuList = new ObservableRangeCollection<Submenu>();

            submenus = new List<Submenu>();

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

        private async void getSubmodule()
        {
            try
            {
                IsBusy = true;
                SubmoduleList.ReplaceRange(await SubmoduleServices.GetSubmodulesByModule(new Submodule
                {
                    Module = module
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

                        await SubmenuServices.EditSubmenu(previousSelectedSubmenu);
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
            NewSubmenu = new Submenu();
        }

        private async Task Add()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to save the information?", "YES", "NO");
                if (answer)
                {
                    IsBusy = true;

                    NewSubmenu.Submodule = Submodule;
                    await SubmenuServices.AddSubmenu(NewSubmenu);
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
                    await SubmenuServices.DeleteSubmenu(previousSelectedSubmenu);
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
            if (SubmenuList.Count == submenus.Count)
                return;

            int lastItemIndexed = SubmenuList.Count;
            int nextItemIndexed = lastItemIndexed + LOAD_SIZE;

            if (nextItemIndexed > submenus.Count)
                nextItemIndexed = submenus.Count;

            for (int i = lastItemIndexed; i < nextItemIndexed; i++)
            {
                SubmenuList.Add(submenus[i]);
            }
        }

        async void getData()
        {
            try
            {
                IsBusy = true;

                if (SoftwareSystem == null || Module == null || Submodule == null)
                    return;

                int size = LOAD_SIZE;
                submenus = await SubmenuServices.GetSubmenusBySubmodule(new Submenu
                {
                    Submodule = Submodule
                });

                if (submenus.Count < LOAD_SIZE)
                    size = submenus.Count;
                SubmenuList.ReplaceRange(submenus.GetRange(0, size));
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
            var submenu = arg as Submenu;
            if (submenu == null)
                return;

            PreviousSelectedSubmenu = submenu;

            IsSelected = true;

            SelectedSubmenu = null;
        }

        async Task Refresh()
        {
            IsBusy = true;
            getData();
            IsBusy = false;
        }
    }
}
