using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace ACDataStorage.Models
{
    class Order : INotifyPropertyChanged 
    {
        
        private int accauntNum;
        private string customer;
        private string products;
        private string dateOfOrder;
       
        public int Id { get; set; }
        public int AccauntNum { get => accauntNum; set { accauntNum = value; OnPropertyChanged(nameof(AccauntNum));}}
        public string Customer { get => customer; set { customer = value; OnPropertyChanged(nameof(Customer));}}
        public string Products { get => products; set { products = value; OnPropertyChanged(nameof(Products));}}
        public string DateOfOrder { get => dateOfOrder; set { dateOfOrder = value; OnPropertyChanged(nameof(DateOfOrder));}}

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
