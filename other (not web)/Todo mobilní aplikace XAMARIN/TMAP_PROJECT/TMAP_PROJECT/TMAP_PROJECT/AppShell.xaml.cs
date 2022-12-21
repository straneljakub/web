using System;
using System.Collections.Generic;
using TMAP_PROJECT.ViewModels;
using TMAP_PROJECT.Views;
using Xamarin.Forms;

namespace TMAP_PROJECT
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(AddItemPage), typeof(AddItemPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
