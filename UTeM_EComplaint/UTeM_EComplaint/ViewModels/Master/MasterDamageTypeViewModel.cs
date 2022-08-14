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
    internal class MasterDamageTypeViewModel : ViewModelBase
    {
        readonly int LOAD_SIZE = 8;

        bool isSelected;
        bool isAdd;

        DamageType selectedDamageType;
        DamageType previousSelectedDamageType;
        DamageType newDamageType;

        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }
        public bool IsAdd { get => isAdd; set => SetProperty(ref isAdd, value); }

        List<DamageType> damageTypes;


        public ObservableRangeCollection<DamageType> DamageTypeList { get; }
        public DamageType SelectedDamageType
        {
            get => selectedDamageType;
            set
            {
                SetProperty(ref selectedDamageType, value);
                OnPropertyChanged();
            }
        }
        public DamageType PreviousSelectedDamageType
        {
            get => previousSelectedDamageType;
            set
            {
                SetProperty(ref previousSelectedDamageType, value);
                OnPropertyChanged();
            }
        }
        public DamageType NewDamageType
        {
            get => newDamageType;
            set
            {
                SetProperty(ref newDamageType, value);
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

        public MasterDamageTypeViewModel()
        {
            Title = "Damage Type";

            DamageTypeList = new ObservableRangeCollection<DamageType>();
            damageTypes = new List<DamageType>();

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
                        int result = await DamageTypeServices.EditDamageType(previousSelectedDamageType);

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
            NewDamageType = new DamageType();
        }

        private async Task Add()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to save the information?", "YES", "NO");
                if (answer)
                {
                    IsBusy = true;
                    int result = await DamageTypeServices.AddDamageType(newDamageType);

                    if (result != 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", $"New damage type with ID {result} has been added", "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Fail to add new damage type", "OK");
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
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to delete this damage type?", "YES", "NO");
                if (answer)
                {
                    IsBusy = true;
                    int result = await DamageTypeServices.DeleteDamageType(previousSelectedDamageType);

                    if (result != 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "This damage type has been deleted", "OK");
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
            if (DamageTypeList.Count == damageTypes.Count)
                return;

            int lastItemIndexed = DamageTypeList.Count;
            int nextItemIndexed = lastItemIndexed + LOAD_SIZE;

            if (nextItemIndexed > damageTypes.Count)
                nextItemIndexed = damageTypes.Count;

            for (int i = lastItemIndexed; i < nextItemIndexed; i++)
            {
                DamageTypeList.Add(damageTypes[i]);
            }
        }

        async void getData()
        {
            try
            {
                int size = LOAD_SIZE;
                IsBusy = true;
                damageTypes = await DamageTypeServices.GetDamageTypes();
                if (damageTypes.Count < LOAD_SIZE)
                    size = damageTypes.Count;
                DamageTypeList.ReplaceRange(damageTypes.GetRange(0, size));
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
            var damageType = arg as DamageType;
            if (damageType == null)
                return;

            PreviousSelectedDamageType = damageType;

            IsSelected = true;

            SelectedDamageType = null;
        }

        async Task Refresh()
        {
            IsBusy = true;
            getData();
            IsBusy = false;
        }
    }
}
