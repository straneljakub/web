using System;
using System.IO;
using TMAP_PROJECT.Services;
using TMAP_PROJECT.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TMAP_PROJECT
{
    public partial class App : Application
    {
        static ItemDatabase _database;

        // Create the database connection as a singleton.
        public static ItemDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new ItemDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Items.db3"));
                }
                return _database;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
