using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMAP_PROJECT.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TMAP_PROJECT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditItemPage : ContentPage
    {
        EditItemViewModel _viewModel;
        public EditItemPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new EditItemViewModel();
        }
    }
}