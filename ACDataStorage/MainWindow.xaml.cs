
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Data;



namespace ACDataStorage
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonEnter_Click(object sender, RoutedEventArgs e)
        {
            string login = NameBox.Text.Trim();
            string password = PasswordBox.Password.Trim();
            if (login.Length > 0) // проверяем введён ли логин     
            {
                if (password.Length > 0) // проверяем введён ли пароль         
                {

                    string query = "SELECT * FROM Authorization Where Login=@login AND Password=@password";//запрос к таблице 

                    using (SQLiteConnection connection = new SQLiteConnection(@"Data Source=.\ACDataStorageDB.db;"))  //соединение с БЗ

                    {
                        connection.Open();
                        SQLiteCommand cmd = new SQLiteCommand(query, connection); 
                        cmd.Parameters.AddWithValue("@login", login);
                        cmd.Parameters.AddWithValue("@password", password);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows) //в таблице есть строка с запрашиваемым пользователем
                            {
                                MainMenu mainMenu = new MainMenu();
                                mainMenu.Show();
                                this.Close();
                            }

                            else { MessageBox.Show("Пользователя не найден"); }
                        }
                    }

                }
                else MessageBox.Show("Введите пароль2"); // выводим ошибку    
            }
            else MessageBox.Show("Введите логин"); // выводим ошибку 
        }
    }
}
