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
    public class ItemDetailViewModel : BaseViewModel
    {
        public Command GoBackCommand { get; }
        public Command EditItemCommand { get; }
        public Command DeleteItemCommand { get; }
        public ItemDetailViewModel()
        {
            EditItemCommand = new Command(OnEditItem);
            DeleteItemCommand = new Command(OnDeleteItem);
            GoBackCommand = new Command(OnGoBack);
        }
        private int itemId;
        private string title;
        private DateTime deadline;
        public int Id { get; set; }

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

        private async void OnEditItem(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(EditItemPage)}?{nameof(EditItemViewModel.ItemId)}={ItemId}");
        }

        private async void OnDeleteItem(object obj)
        {
            Item deletedItem= new Item() { Id = int.Parse(ItemId) };
            await App.Database.DeleteItemAsync(deletedItem);
            await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
        }
        private async void OnGoBack()
        {
            await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
        }
    }
}
