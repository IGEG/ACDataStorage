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


namespace ACDataStorage
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        AppDbContext appDb;
        public MainMenu()
        {

            InitializeComponent();
            appDb = new AppDbContext();
            appDb.Orders.Load();
            appDb.Clients.Load();
            DataGridOrders.DataContext = appDb.Orders.Local.ToBindingList();
            DataGridClients.DataContext = appDb.Clients.Local.ToBindingList();
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
            if(addOrderMenu.ShowDialog()==true)
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

 

        private  async void DownLoadButton(object sender, RoutedEventArgs e)
        {

            await Task.Run(() => { Parallel.Invoke(()=>Client.DownLoadCollection());});
           
        }

        private async void DropTableButton(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => { Parallel.Invoke(() => Client.DropTableClients()); });
            
        }
    }
    #region ORDERSDataGridTextSearch
    public static class DataGridTextSearch //честно-скоммуниженный код со stackowerflow
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
    #endregion DataGridTextSearch
}