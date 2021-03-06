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

    public partial class AddReserveMenu : Window
    {
       public Reserve Reserve { get; private set; }
       public string Textdate { get; private set; }
       public AddReserveMenu(Reserve res)
        {
            Textdate = DateTime.Now.ToString();
            InitializeComponent();
            Reserve = res;
            this.DataContext = Reserve;
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
