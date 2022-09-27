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
using System.Xml;
using System.Windows.Shapes;
using System.Collections;

namespace AdvanceHomework
{
    /// <summary>
    /// Interaction logic for CreatingNewUser.xaml
    /// </summary>
    public partial class CreatingNewUser : Window
    {
        public CreatingNewUser()
        {
            InitializeComponent();
        }

        private void CanCreateNewUser()
        {
            ProductDB abvanceHW = new ProductDB();
            var iterator = abvanceHW.Clients.Select(x => x.Login);
            if (!iterator.Contains(loginBox.Text))
            {
                ClientInfo client = new ClientInfo(int.Parse(idBox.Text), nameBox.Text,
                    surnameBox.Text, int.Parse(telephoneNumberBox.Text), loginBox.Text, passwordBox.Text);
                abvanceHW.Clients.Add(client);
                abvanceHW.SaveChanges();
                CreatingXMLFile(client.Login);
                MessageBox.Show("User has been created");
            }
            else
            {
                MessageBox.Show("User with this login already exist");
            }
            idBox.Text = null;
            nameBox.Text = null;
            surnameBox.Text = null;
            telephoneNumberBox.Text = null;
            loginBox.Text = null;
            passwordBox.Text = null;
        }

        private void creatingUserButton_Click(object sender, RoutedEventArgs e)
        {
            CanCreateNewUser();
            Close();
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

    }
}
