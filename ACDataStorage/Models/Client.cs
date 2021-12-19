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
    class Client : INotifyPropertyChanged
    {
        private string name;
        private string contacts;
        public int Id { get; set; }
        public string Name { get => name; set { name = value; OnPropertyChanged(nameof(Name)); } }
        public string Contacts { get => contacts; set { contacts = value; OnPropertyChanged(nameof(Contacts)); } }
        public Client() { }
        public Client(string name, string contacts) { this.name = name; this.contacts = contacts; }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        // Загружаем с ПК все данные по контактам в БД (имя компании из названия папки, контакты из фалов *.txt)
        public static void DownLoadCollection()
        { 
            string stringForDownloading = ConfigurationManager.AppSettings["stringForDownLoadingClients"];//папка, в которой хранятся все файлы, через  app.config
            DirectoryInfo directory = new DirectoryInfo(stringForDownloading);
            FileInfo[] fileInfo = directory.GetFiles("*.txt", SearchOption.AllDirectories);// все файлы *.txt
            AppDbContext appcon = new AppDbContext();
            foreach (var i in fileInfo)
            {
                
                using (StreamReader stream = File.OpenText(i.FullName)) //поток для чтения конкретного файла txt по адресу i.fullname
                {
                   
                    if (stream.ReadToEnd() != null) 
                    {

                        var text = File.ReadAllText(i.FullName, Encoding.GetEncoding(1251));// читаем txt применяя кодировку 1251. иначе не читается корректно.
                        appcon.Clients.Add(new Client()
                        {
                            Name = i.DirectoryName.Substring((i.DirectoryName.LastIndexOf('\\')) + 1), //имя компании - название папки
                            Contacts = text // контакты - текст в файле txt
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
