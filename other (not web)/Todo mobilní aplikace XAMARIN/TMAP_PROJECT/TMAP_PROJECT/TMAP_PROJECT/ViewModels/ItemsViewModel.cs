using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using TMAP_PROJECT.Models;
using TMAP_PROJECT.Views;
using Xamarin.Forms;
using System.Linq;
using System.ComponentModel;
using System.Collections.Specialized;

namespace TMAP_PROJECT.ViewModels
{
    class ItemsViewModel : BaseViewModel
    {
        private Item _selectedItem;
        private ObservableCollection<Item> _items;
        public ObservableCollection<Item> Items
        {
            get { return _items; }
            set
            {
                if (_items == value) return;
                _items = value;
                OnPropertyChanged();
            }
        }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Item> ItemTapped { get; }



        public ItemsViewModel()
        {
            Items = new ObservableCollection<Item>();
            Items.CollectionChanged += ContentCollectionChanged;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddItemCommand = new Command(OnAddItem);
            ItemTapped = new Command<Item>(OnItemSelected);

        }

        public void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Item item in e.OldItems)
                {
                    item.PropertyChanged -= ItemPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Item item in e.NewItems)
                {
                    item.PropertyChanged += ItemPropertyChanged;
                }
            }
        }

        public async void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        public async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var list = await App.Database.GetItemsAsync();
                var items = list.OrderBy(i => i.Deadline).OrderBy(i => i.Done == true);


                foreach (var item in items)
                {

                    TimeSpan timeSpan = item.Deadline - DateTime.Now;

                    int timeSpanMinutes = timeSpan.Minutes;
                    int timeSpanHours = timeSpan.Hours;
                    int timeSpanDays = timeSpan.Days;

                    if (timeSpanHours >= 2)
                        timeSpanHours -= 2;
                    else
                    {
                        timeSpanHours = timeSpanHours + 24 - 2;
                        timeSpanDays -= 1;
                    }

                    if (timeSpanHours < 0 || timeSpanDays < 0)
                    {
                        timeSpanHours = 0;
                        timeSpanDays = 0;
                        timeSpanMinutes = 0;
                    }

                    if (timeSpanDays > 0)
                    {
                        item.Timeleft = $"{timeSpanDays} Days {timeSpanHours} Hours {timeSpanMinutes} Minutes";
                    }
                    else if (timeSpanHours > 0)
                        item.Timeleft = $"{timeSpanHours} Hours {timeSpanMinutes} Minutes";
                    else
                        item.Timeleft = $"{timeSpanMinutes} Minutes";

                    if (item.Done == true)
                    {
                        item.Timeleft = "Done!";
                        item.Color = "#13eb57";
                    }
                    else if (timeSpanDays <= 0 && timeSpanHours <= 0 && timeSpanMinutes <= 0)
                    {
                        item.Color = "#ff0000";
                    }
                    else if (timeSpanDays < 1)
                    {
                        item.Color = "#e8aa00";
                    }
                    else
                    {
                        item.Color = "#000000";
                    }


                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;

            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(AddItemPage));
        }

        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;


            await Shell.Current.GoToAsync($"//{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }


    }


}
