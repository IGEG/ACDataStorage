using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.Entity;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ACDataStorage.Models;
using System.Runtime.CompilerServices;
using System.Configuration;
using System.IO;


namespace ACDataStorage
{

    public partial class MainMenu : Window
    {
        AppDbContext appDb;
        public MainMenu()
        {

            InitializeComponent();
            appDb = new AppDbContext();
            appDb.Orders.Load();
            appDb.Clients.Load();
            appDb.Reserves.Load();
            DataGridOrders.DataContext = appDb.Orders.Local.ToBindingList();
            DataGridClients.DataContext = appDb.Clients.Local.ToBindingList();
            DataGridReserve.DataContext = appDb.Reserves.Local.ToBindingList();

            this.Closing += MainWindow_Closing;

        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            appDb.Dispose();
        }

        #region ORDERS CRUD
        private void AddOrderButton(object sender, RoutedEventArgs e)
        {
            AddOrderMenu addOrderMenu = new AddOrderMenu(new Order());
            if (addOrderMenu.ShowDialog() == true)
            {
                Order order = addOrderMenu.Order;
                appDb.Orders.Add(order);
                appDb.SaveChanges();
            }
        }

        private void ChangeOrderButton(object sender, RoutedEventArgs e)
        {
            if (DataGridOrders.SelectedItem == null) return;
            Order order = DataGridOrders.SelectedItem as Order;
            AddOrderMenu addOrderMenu = new AddOrderMenu(new Order
            {
                Id = order.Id,
                AccauntNum = order.AccauntNum,
                Customer = order.Customer,
                Products = order.Products,
                DateOfOrder = order.DateOfOrder
            });
            if (addOrderMenu.ShowDialog() == true)
            {
                order = appDb.Orders.Find(addOrderMenu.Order.Id);
                if (order != null)
                {
                    order.AccauntNum = addOrderMenu.Order.AccauntNum;
                    order.Customer = addOrderMenu.Order.Customer;
                    order.Products = addOrderMenu.Order.Products;
                    order.DateOfOrder = addOrderMenu.Order.DateOfOrder;
                    appDb.Entry(order).State = EntityState.Modified;
                    appDb.SaveChanges();
                }
            }
        }

        private void DeleteOrderButton(object sender, RoutedEventArgs e)
        {
            if (DataGridOrders.SelectedItem == null) return;
            Order order = DataGridOrders.SelectedItem as Order;
            appDb.Orders.Remove(order);
            appDb.SaveChanges();

        }
        #endregion ORDERS

        #region CLIENTS FILTERS
        private void FindClientsButton(object sender, RoutedEventArgs e)
        {
            if (TextBoxSearchClient.Text == null) return; //проверяем, что введен текст
            string findTextBoxClient = TextBoxSearchClient.Text;
            AppDbContext db = new AppDbContext();
            // находим в столбцах Name и Contacts совпадения с введенным текстом
            List<Client> ClientToFindList = db.Clients.Where(x => x.Name.Contains(findTextBoxClient) || x.Contacts.Contains(findTextBoxClient)).ToList<Client>();
            if (ClientToFindList.Count() == 0)
            { MessageBox.Show("КЛИЕНТ НЕ НАЙДЕН"); return; }// если нет совпадений-выходим
            FindClientsWindow findClient = new FindClientsWindow(ClientToFindList);// передаем в конструктор класса коллекцию нужных нам объектов CLients
            findClient.ShowDialog(); // визуализиурем в listbox

        }
        #endregion

        #region CLIENTS CRUD
        private void AddReservButton(object sender, RoutedEventArgs e)
        {
            AddReserveMenu addReserveMenu = new AddReserveMenu(new Reserve());
            if (addReserveMenu.ShowDialog() == true)
            {
                Reserve reserve = addReserveMenu.Reserve;
                appDb.Reserves.Add(reserve);
                appDb.SaveChanges();
            }
        }

        private void ChangeReserveButton(object sender, RoutedEventArgs e)
        {
            {
                if (DataGridReserve.SelectedItem == null) return;
                Reserve reserve = DataGridReserve.SelectedItem as Reserve;
                AddReserveMenu addReserveMenu = new AddReserveMenu(new Reserve
                {
                    ReserveId = reserve.ReserveId,
                    AccauntName = reserve.AccauntName,
                    Client = reserve.Client,
                    Product = reserve.Product,
                    DateOfReserve = reserve.DateOfReserve
                });
                if (addReserveMenu.ShowDialog() == true)
                {
                    reserve = appDb.Reserves.Find(addReserveMenu.Reserve.ReserveId);
                    if (reserve != null)
                    {
                        reserve.AccauntName = addReserveMenu.Reserve.AccauntName;
                        reserve.Client = addReserveMenu.Reserve.Client;
                        reserve.Product = addReserveMenu.Reserve.Product;
                        reserve.DateOfReserve = addReserveMenu.Reserve.DateOfReserve;
                        appDb.Entry(reserve).State = EntityState.Modified;
                        appDb.SaveChanges();
                    }
                }
            }

        }

        private void DeleteReserveButton(object sender, RoutedEventArgs e)
        {
            if (DataGridReserve.SelectedItem == null) return;
            Reserve reserve = DataGridReserve.SelectedItem as Reserve;
            appDb.Reserves.Remove(reserve);
            appDb.SaveChanges();

        }

        #endregion

        #region DownLoad data from folders on PC to Clients table in ACDataStorsgeDB

        private async void DownLoadButton(object sender, RoutedEventArgs e)
        {

            await Task.Run(() => { Parallel.Invoke(() => Client.DownLoadCollection()); });

        }

        private async void DropTableButton(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => { Parallel.Invoke(() => Client.DropTableClients()); });

        }
        #endregion

   

        private async void OpenDocumetnsButton(object sender, RoutedEventArgs e)
        {
            string stringDocuments = ConfigurationManager.AppSettings["stringDocumemts"];
            await Task.Run(() => { Parallel.Invoke(() => MainMenu.OpenFolder(stringDocuments)); });
        }

        private async void OpenCargoButton_Click(object sender, RoutedEventArgs e)
        {
            string stringCargo = ConfigurationManager.AppSettings["stringCargo"];
            await Task.Run(() => { Parallel.Invoke(() => MainMenu.OpenFolder(stringCargo)); });
        }

        private async void OpenManufacturingButton_Click(object sender, RoutedEventArgs e)
        {
            string stringManufacturing = ConfigurationManager.AppSettings["stringManufacturing"];
            await Task.Run(() => { Parallel.Invoke(() => MainMenu.OpenFolder(stringManufacturing)); });
        }

        private async void OpenFormecoButton_Click(object sender, RoutedEventArgs e)
        {
            string stringFormeco = ConfigurationManager.AppSettings["stringFormeco"];
            await Task.Run(() => { Parallel.Invoke(() => MainMenu.OpenFolder(stringFormeco)); });
        }

        private async void OpenCollectionButton_Click(object sender, RoutedEventArgs e)
        {
            string stringCollection = ConfigurationManager.AppSettings["stringCollection"];
            await Task.Run(() => { Parallel.Invoke(() => MainMenu.OpenFolder(stringCollection)); });

        }

        private async void OpenLetterButton_Click(object sender, RoutedEventArgs e)
        {
            string stringLetter = ConfigurationManager.AppSettings["stringLetter"];
            await Task.Run(() => { Parallel.Invoke(() => MainMenu.OpenFolder(stringLetter)); });

        }

        public static void OpenFolder(string folder)
        {
            FileInfo fileInfo = new FileInfo(folder);
            FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            fileStream.Close();
 
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public static class DataGridTextSearch //честно-скоммуниженный код со stackowerflow для выделения требуемых строк в datagrid
    {
        // Using a DependencyProperty as the backing store for SearchValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchValueProperty =
            DependencyProperty.RegisterAttached("SearchValue", typeof(string), typeof(DataGridTextSearch),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.Inherits));

        public static string GetSearchValue(DependencyObject obj)
        {
            return (string)obj.GetValue(SearchValueProperty);
        }

        public static void SetSearchValue(DependencyObject obj, string value)
        {
            obj.SetValue(SearchValueProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsTextMatch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTextMatchProperty =
            DependencyProperty.RegisterAttached("IsTextMatch", typeof(bool), typeof(DataGridTextSearch), new UIPropertyMetadata(false));

        public static bool GetIsTextMatch(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsTextMatchProperty);
        }

        public static void SetIsTextMatch(DependencyObject obj, bool value)
        {
            obj.SetValue(IsTextMatchProperty, value);
        }
    }

    public class SearchValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string cellText = values[0] == null ? string.Empty : values[0].ToString();
            string searchText = values[1] as string;

            if (!string.IsNullOrEmpty(searchText) && !string.IsNullOrEmpty(cellText))
            {
                return cellText.ToLower().StartsWith(searchText.ToLower());
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}

        

