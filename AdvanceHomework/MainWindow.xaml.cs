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
using System.Data.Entity;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace AdvanceHomework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ProductDB advanceHWDB = new ProductDB();


        //Листы для заполнения БД при запуске приложения
        List<ProductInfo> products = new List<ProductInfo>()
        {
            new ProductInfo(1, "Banana", 30),
            new ProductInfo(2, "Apple", 35),
            new ProductInfo(3, "Cherry", 23),
            new ProductInfo(4, "Wildcherry", 45)
        };

        List<ManegersInfo> manegers = new List<ManegersInfo>()
        {
            new ManegersInfo(1, "Vadym", "Boichenko", "Manager1", "Password1"),
            new ManegersInfo(2, "Yuriy", "Shpak", "Manager2", "Password2")
        };

        List<ClientInfo> clients = new List<ClientInfo>()
        {
            new ClientInfo(1, "Vadym", "Boichenko", 123, "Vadym1", "User1"),
            new ClientInfo(2, "Oleg", "Bocharov", 123, "Oleg1", "User2")
        };
        public MainWindow()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<ProductDB>());
            foreach (var item in manegers)
            {
                advanceHWDB.Manegers.Add(item);
            }
            foreach (var item in clients)
            {
                advanceHWDB.Clients.Add(item);
            }
            foreach (var item in products)
            {
                advanceHWDB.ProductInfo.Add(item);
            }
            advanceHWDB.SaveChanges();
            InitializeComponent();
        }

        private void CreatingXMLFile(string fileName)
        {
            XmlTextWriter xmlWriter = new XmlTextWriter($"{fileName}.xml", Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Settings");
            xmlWriter.WriteStartAttribute("BackgroundColor");
            xmlWriter.WriteString("White");
            xmlWriter.WriteEndAttribute();
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
        }

        private void LogInClick(object sender, RoutedEventArgs e)
        {
            var users = advanceHWDB.Clients.Select(x => x);
            foreach (var user in users)
            {
                if (user.Login == login.Text && user.Password == password.Text)
                {
                    if (File.Exists($"{user.Login}.xml"))
                    {
                        UserWindow existingUser = new UserWindow(user.Login);
                        existingUser.Show();
                        Close();
                        return;
                    }
                    else
                    {
                        CreatingXMLFile(user.Login);
                        UserWindow existingUser = new UserWindow(user.Login);
                        existingUser.Show();
                        Close();
                        return;
                    }
                }
            }
            var managers = advanceHWDB.Manegers.Select(x => x);
            foreach (var manager in managers)
            {
                if (manager.Login == login.Text && manager.Password == password.Text)
                {
                    Manager manager1 = new Manager();
                    manager1.Show();
                    Close();
                    return;
                }
            }
            MessageBox.Show("Non-Existent User");
            login.Text = null;
            password.Text = null;
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
