using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Xml.Linq;

namespace AdvanceHomework
{
    /// <summary>
    /// Interaction logic for Manager.xaml
    /// </summary>
    public partial class Manager : Window
    {
        ProductDB abvanceHW = new ProductDB();
        private ColorDialog chooseColorDialog = new ColorDialog();
        private string filename = null;
        List<ClientInfo> clientInfos = new List<ClientInfo>();
        public Manager()
        {
            InitializeComponent();
            LoadUsers();
        }
        public Manager(string fileName)
        {
            InitializeComponent();
            this.filename = fileName;
            try
            {
                LoadSettings();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Something wrong");
            }
            
        }
        private void LoadUsers()
        {
            clientInfos.Clear();
            managerGrid.ItemsSource = null;
            foreach (var item in abvanceHW.Clients)
            {
                clientInfos.Add(item);
            }
            managerGrid.ItemsSource = clientInfos;
        }

        private void options_Click(object sender, RoutedEventArgs e)
        {
            if (chooseColorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Background = new SolidColorBrush(Color.FromArgb(chooseColorDialog.Color.A, chooseColorDialog.Color.R,
                    chooseColorDialog.Color.G, chooseColorDialog.Color.B));
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load($"{filename}.xml");
                XmlNode node = xmlDocument.SelectSingleNode("Settings");
                node.Attributes[0].Value = Background.ToString();
                xmlDocument.Save($"{filename}.xml");
            }
        }

        private static XmlDocument LoadSettingFromConfig()
        {
            XmlDocument document = null;
            try
            {
                document = new XmlDocument();
                //document.Load(Assembly.GetExecutingAssembly().Location + ".config");
                document.Load("Vadym");
                return document;
            }
            catch (Exception)
            {
                throw new Exception("Something goes wrong");
            }
        }
        bool LoadSettings()
        {
            string color = null;
            FileStream fileStream = new FileStream($"{filename}.xml", FileMode.Open);
            XmlTextReader xmlReader = new XmlTextReader(fileStream);
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.HasAttributes)
                    {
                        color = xmlReader.GetAttribute("BackgroundColor");
                    }
                }
            }
            var converter = new System.Windows.Media.BrushConverter();
            NameValueCollection nameValue = ConfigurationManager.AppSettings;
            if (nameValue.Count < 1) { return (false); }
            Background = (Brush)converter.ConvertFromString(color);
            fileStream.Close();
            xmlReader.Close();
            return true;
        }

        void SaveSettings()
        {
            /*XmlDocument document = LoadSettingFromConfig();
            XmlNode node = document.SelectSingleNode("//appSettings");
            string[] keys = new string[]
            {
                "BackGroundColor"
            };
            string[] values = new string[]
            {
                Background.ToString()
            };
            for (int i = 0; i < keys.Length; i++)
            {
                XmlElement element = node.SelectSingleNode(string.Format("//add[@key='{0}']", keys[i])) as XmlElement;
                if (element != null)
                {
                    element.SetAttribute("value", values[i]);
                }
                else
                {
                    element = document.CreateElement("add");
                    element.SetAttribute("key", keys[i]);
                    element.SetAttribute("value", values[i]);
                    node.AppendChild(element);
                }
            }
            document.Save(Assembly.GetExecutingAssembly().Location + ".config");*/
        }

        private void CreateNewUser(object sender, RoutedEventArgs e)
        {
            CreatingNewUser newUser = new CreatingNewUser();
            newUser.Show();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }

        private void logOutManager_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
