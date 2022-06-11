﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using UTeM_EComplaint.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace UTeM_EComplaint.ViewModels
{
    internal class AdminAssignTechnicianViewModel : ViewModelBase
    {
        readonly int LOAD_SIZE = 5;

        bool isItemSelected;
        public bool IsItemSelected { get => IsItemSelected; set => SetProperty(ref isItemSelected, value); }

        Technician selectedTechnician;
        List<Technician> technicians;
        public ObservableRangeCollection<Technician> TechnicianList { get; }
        public Technician SelectedTechnician
        {
            get => selectedTechnician;
            set
            {
                SetProperty(ref selectedTechnician, value);
                OnPropertyChanged();
                if(value != null)
                    IsItemSelected = true;
                else
                    IsItemSelected = false;
            }
        }

        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand<object> ItemSelectedCommand { get; }
        public AsyncCommand LoadMoreCommand { get; }
        public AsyncCommand SubmitCommand { get; }

        public AdminAssignTechnicianViewModel()
        {
            Title = "Assigned complaint";

            TechnicianList = new ObservableRangeCollection<Technician>();
            technicians = new List<Technician>();

            ItemSelectedCommand = new AsyncCommand<object>(ItemSelected);
            RefreshCommand = new AsyncCommand(Refresh);
            LoadMoreCommand = new AsyncCommand(LoadMore);
            SubmitCommand = new AsyncCommand(Submit);
            getData();
        }

        private async Task Submit()
        {
            await Shell.Current.GoToAsync("..?technicianID=" + SelectedTechnician.TechnicianID);
        }

        private async Task LoadMore()
        {
            if (TechnicianList.Count == technicians.Count)
                return;

            int lastItemIndexed = TechnicianList.Count;
            int nextItemIndexed = lastItemIndexed + LOAD_SIZE;

            if (nextItemIndexed > technicians.Count)
                nextItemIndexed = technicians.Count;

            for (int i = lastItemIndexed; i < nextItemIndexed; i++)
            {
                TechnicianList.Add(technicians[i]);
            }
        }

        async void getData()
        {
            try
            {
                int size = LOAD_SIZE;
                IsBusy = true;
                technicians = await TechnicianServices.GetAllTechnicians();
                if (technicians.Count < LOAD_SIZE)
                    size = technicians.Count;
                TechnicianList.ReplaceRange(technicians.GetRange(0, size));
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
            var technician = arg as Technician;
            if (technician == null)
                return;

        }
        async Task Refresh()
        {
            IsBusy = true;
            getData();
            IsBusy = false;
        }
    }
}