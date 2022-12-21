using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace TMAP_PROJECT.Models
{
    public class Item : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Deadline { get; set; }
        public string Timeleft { get; set; }
        private bool done;
        public bool Done {
            get { return done; }
            set { done = value; NotifyPropertyChanged("Done"); } 
        }
        public string Color { get; set; }

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    
}
