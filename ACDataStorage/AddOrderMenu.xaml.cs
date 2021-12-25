using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ACDataStorage.Models;

namespace ACDataStorage
{

    public partial class AddOrderMenu : Window
    {
        public Order Order { get; private set; }
        public string Textdate { get; private set; }
     

        public AddOrderMenu(Order o)
        {
            Textdate= DateTime.Now.ToString();
            InitializeComponent();
            Order = o;
            this.DataContext = Order;

        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
