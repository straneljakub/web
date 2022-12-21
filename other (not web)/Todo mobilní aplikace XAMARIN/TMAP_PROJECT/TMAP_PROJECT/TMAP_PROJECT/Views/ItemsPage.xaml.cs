using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMAP_PROJECT.Models;
using TMAP_PROJECT.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace TMAP_PROJECT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;
        public ItemsPage()
        {
            InitializeComponent();
            _viewModel = new ItemsViewModel();
            BindingContext = _viewModel;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private async void CheckedChanged(object sender, CheckedChangedEventArgs args)
        {
            if(_viewModel.IsBusy == true) {
                return;
            }
            else {
                CheckBox checkbox = sender as CheckBox;
                Item item = checkbox.BindingContext as Item;

                if (item == null)
                {
                    return;
                }
           
                await App.Database.SaveItemAsync(item);
            }

        }


    }
}