using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ACDataStorage.Models
{
   public class Reserve : INotifyPropertyChanged
    {
        private string accauntName;
        private string client;
        private string product;
        private string dateOfReserve;

        public int ReserveId { get; set; }
        public string AccauntName { get => accauntName; set { accauntName = value; OnPropertyChanged(nameof(AccauntName)); } }
        public string Client { get => client; set { client = value; OnPropertyChanged(nameof(Client)); } }
        public string Product { get => product; set { product = value; OnPropertyChanged(nameof(Product)); } }
        public string DateOfReserve { get => dateOfReserve; set { dateOfReserve = value; OnPropertyChanged(nameof(DateOfReserve)); } }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}