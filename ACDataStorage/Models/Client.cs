using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Configuration;
using System.IO;
using System.Data.SQLite;
using System.Windows.Threading;
using System.Windows;

namespace ACDataStorage.Models
{
    public  class Client : INotifyPropertyChanged
    {
        private string name;
        private string contacts;
        public int Id { get; set; }
        public string Name { get => name; set { name = value; OnPropertyChanged(nameof(Name)); } }
        public string Contacts { get => contacts; set { contacts = value; OnPropertyChanged(nameof(Contacts)); } }
        public Client() { }
        public Client(string name, string contacts) { this.name = name; this.contacts = contacts; }
        public override string ToString()// переопределяем для вывода в ListBox в окне FindClientsMenu
        {
            return $"{Name} ---------  {Contacts}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        // Метод для загрузки с ПК всех данных по контактам клиентов в БД (имя компании берем из названия папки, контакты из фалов *.txt)

        public static void DownLoadCollection()

        {   //папка, в которой хранятся все нужные файлы, получаем через  app.config(ConfigurationManager.AppSettings)т.к. компьютеров несколько, необходимо менять путь.

            string stringForDownloading = ConfigurationManager.AppSettings["stringForDownLoadingClients"];
            DirectoryInfo directory = new DirectoryInfo(stringForDownloading);

            // получаем массив FileInfo папок,в которых есть файл с расширением txt

            FileInfo[] fileInfo = directory.GetFiles("*.txt", SearchOption.AllDirectories);        // все файлы *.txt
            AppDbContext appcon = new AppDbContext();
            foreach (var i in fileInfo)
            {
                
                using (StreamReader stream = File.OpenText(i.FullName))        //поток для чтения конкретного файла txt по адресу i.fullname
                {
                   
                    if (stream.ReadToEnd() != null) 
                    {

                        var text = File.ReadAllText(i.FullName, Encoding.GetEncoding(1251));      // читаем txt применяя кодировку 1251. иначе не читается корректно.
                        if (appcon.Clients.Where(x => x.Name == text).Count() > 0) return;       // проверка на дублирование
                        appcon.Clients.Add(new Client()
                        {
                            Name = i.DirectoryName.Substring((i.DirectoryName.LastIndexOf('\\')) + 1),      //имя компании - название папки
                            Contacts = text          // контакты - текст в файле txt
                        });
                    }
                    else
                        appcon.Clients.Add(new Client()
                        {
                            Name = i.DirectoryName.Substring((i.DirectoryName.LastIndexOf('\\')) + 1),
                            Contacts = "контактов нет"
                        });

                }
            }
            appcon.SaveChanges();
            MessageBox.Show($"данные загружены! Перезагрузите приложение для отображения изменений!");
        }
        //удаляем все данные из БД Клиентов
        public static void DropTableClients()
        {
            AppDbContext appcon = new AppDbContext();
            appcon.Clients.RemoveRange(appcon.Clients);
            appcon.SaveChanges();
        }
    }
}
