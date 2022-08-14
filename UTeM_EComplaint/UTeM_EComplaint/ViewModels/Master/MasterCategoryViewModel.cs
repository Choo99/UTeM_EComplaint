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
    internal class MasterCategoryViewModel : ViewModelBase
    {
        readonly int LOAD_SIZE = 8;

        bool isSelected;
        bool isAdd;

        Category selectedCategory;
        Category previousSelectedCategory;
        Category newCategory;

        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }
        public bool IsAdd { get => isAdd; set => SetProperty(ref isAdd, value); }

        List<Category> categories;


        public ObservableRangeCollection<Category> CategoryList { get; }
        public Category SelectedCategory
        {
            get => selectedCategory;
            set
            {
                SetProperty(ref selectedCategory, value);
                OnPropertyChanged();
            }
        }
        public Category PreviousSelectedCategory
        {
            get => previousSelectedCategory;
            set
            {
                SetProperty(ref previousSelectedCategory, value);
                OnPropertyChanged();
            }
        }
        public Category NewCategory
        {
            get => newCategory;
            set
            {
                SetProperty(ref newCategory, value);
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

        public MasterCategoryViewModel()
        {
            Title = "Category complaint";

            CategoryList = new ObservableRangeCollection<Category>();
            categories = new List<Category>();

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
                        int result = await CategoryServices.EditCategory(previousSelectedCategory);

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
            NewCategory = new Category();
        }

        private async Task Add()
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to save the information?", "YES", "NO");
                if (answer)
                {
                    IsBusy = true;
                    int result = await CategoryServices.AddCategory(newCategory);

                    if (result != 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", $"New category with ID {result} has been added", "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Fail to add new category", "OK");
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
                var answer = await Application.Current.MainPage.DisplayAlert("Save", "Are you sure you want to delete this category?", "YES", "NO");
                if (answer)
                {
                    IsBusy = true;
                    int result = await CategoryServices.DeleteCategory(previousSelectedCategory);

                    if(result != 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "This category has been deleted", "OK");
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
            if (CategoryList.Count == categories.Count)
                return;

            int lastItemIndexed = CategoryList.Count;
            int nextItemIndexed = lastItemIndexed + LOAD_SIZE;

            if (nextItemIndexed > categories.Count)
                nextItemIndexed = categories.Count;

            for (int i = lastItemIndexed; i < nextItemIndexed; i++)
            {
                CategoryList.Add(categories[i]);
            }
        }

        async void getData()
        {
            try
            {
                int size = LOAD_SIZE;
                IsBusy = true;
                categories = await CategoryServices.GetCategories();
                if (categories.Count < LOAD_SIZE)
                    size = categories.Count;
                CategoryList.ReplaceRange(categories.GetRange(0, size));
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
            var category = arg as Category;
            if (category == null)
                return;

            PreviousSelectedCategory = category;

            IsSelected = true;

            SelectedCategory = null;
        }

        async Task Refresh()
        {
            IsBusy = true;
            getData();
            IsBusy = false;
        }
    }
}
