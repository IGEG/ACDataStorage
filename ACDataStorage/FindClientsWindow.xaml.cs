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
    /// <summary>
    /// Логика взаимодействия для FindClientsWindow.xaml
    /// </summary>
    public partial class FindClientsWindow : Window
    {
 
        public FindClientsWindow( List<Client> client)
        {
            InitializeComponent();
            ListFindClient.ItemsSource = client;

        }
    }
}
