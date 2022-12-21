using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TMAP_PROJECT.Models;
using TMAP_PROJECT.Views;
using Xamarin.Forms;

namespace TMAP_PROJECT.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class EditItemViewModel : BaseViewModel
    {
        public Command GoBackCommand { get; }
        public Command SaveItemCommand { get; }
        private int itemId;
        private string title;
        private DateTime deadline;
        private TimeSpan time;
        public int Id { get; set; }

        public EditItemViewModel()
        {
            GoBackCommand = new Command(OnGoBack);
            SaveItemCommand = new Command(OnSaveItem);
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

        public string ItemId
        {
            get
            {
                return itemId.ToString();
            }
            set
            {
                int id = int.Parse(value);
                itemId = id;
                LoadItemId(id);
            }
        }
        public async void LoadItemId(int itemId)
        {
            try
            {
                var item = await App.Database.GetItemsAsync(itemId);
                Id = item.Id;
                Title = item.Title;
                Deadline = item.Deadline;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
        private async void OnSaveItem()
        {
            DateTime newDate;

            if (Deadline < DateTime.Today)
            {
                newDate = DateTime.Today;
                newDate += Time;
            }
            else
            {
                TimeSpan difference = new TimeSpan(Deadline.Hour, Deadline.Minute, 0);
                newDate = Deadline - difference;
                newDate += Time;
            }


            Item item = new Item()
            {
                Id = int.Parse(ItemId),
                Title = Title,
                Deadline = newDate,
            };

            await App.Database.SaveItemAsync(item);


            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
        }
        private async void OnGoBack()
        {
            await Shell.Current.GoToAsync($"//{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={ItemId}");
        }
    }
}
