using System;
using System.Collections.Generic;
using System.Text;
using TMAP_PROJECT.Models;
using TMAP_PROJECT.Views;
using Xamarin.Forms;

namespace TMAP_PROJECT.ViewModels
{
    public class AddItemViewModel : BaseViewModel
    {
        private string title;
        private DateTime deadline;
        private TimeSpan time;

        public Command GoBackCommand { get; }
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public AddItemViewModel()
        {
            GoBackCommand = new Command(OnGoBack);
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public DateTime Deadline
        {
            get => deadline;
            set => SetProperty(ref deadline, value);
        }

        public TimeSpan Time
        {
            get => time;
            set => SetProperty(ref time, value);
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            if (Title == null)
            {
                return;
            }


            DateTime newDate;
            if (Deadline < DateTime.Today)
            {
                newDate = DateTime.Today;
                newDate += Time;
            }
            else
            {
                newDate = Deadline + Time;
            }

            
            Item newItem = new Item()
            {
                Title = Title,
                Deadline = newDate,
                Done = false,
                Timeleft = ""
                
            };

            await App.Database.SaveItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
        }

        private async void OnGoBack()
        {
            await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
        }
    }
}
