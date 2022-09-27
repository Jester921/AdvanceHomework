using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Xml;
using MessageBox = System.Windows.MessageBox;

namespace AdvanceHomework
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private ColorDialog chooseColorDialog = new ColorDialog();
        private string filename = null;
        List<ProductInfo> products = new List<ProductInfo>();
        ProductDB abvanceHW = new ProductDB();
        public UserWindow()
        {
            InitializeComponent();
            products.Clear();
            foreach (var item in abvanceHW.ProductInfo)
            {
                products.Add(item);
            }
            productsTable.ItemsSource = null;
            productsTable.ItemsSource = products;
            abvanceHW.SaveChanges();
        }
        public UserWindow(string fileName)
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
            products.Clear();
            foreach (var item in abvanceHW.ProductInfo)
            {
                products.Add(item);
            }
            productsTable.ItemsSource = null;
            productsTable.ItemsSource = products;
            abvanceHW.SaveChanges();
        }

        private bool LoadSettings()
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

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
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

        private void makeOrder_Click(object sender, RoutedEventArgs e)
        {
            if (productNameToBuy.Text == null || productQuantityToBuy.Text == null)
            {
                MessageBox.Show("Enter correctly what you want to buy");
                productNameToBuy.Text = null;
                productQuantityToBuy.Text = null;
                return;
            }
            foreach (var item in products)
            {
                if (item.ProductName == productNameToBuy.Text)
                {
                    List<History> histories = new List<History>();
                    histories.Clear();
                    histories.Add(new History(productNameToBuy.Text, int.Parse(productQuantityToBuy.Text)));
                    float cost = new History(productNameToBuy.Text, int.Parse(productQuantityToBuy.Text)).Cost;
                    historyGrid.ItemsSource = histories;
                    FileStream fileStream = File.Create($"{filename}Order.txt");
                    WriteToFile(fileStream, filename);
                    WriteToFile(fileStream, "\n");
                    WriteToFile(fileStream, DateTime.Now.ToString());
                    WriteToFile(fileStream, "\n");
                    WriteToFile(fileStream, productNameToBuy.Text);
                    WriteToFile(fileStream, "\n");
                    WriteToFile(fileStream, productQuantityToBuy.Text);
                    WriteToFile(fileStream, "\n");
                    WriteToFile(fileStream, cost.ToString());
                    fileStream.Close();
                    abvanceHW.SaveChanges();
                    productNameToBuy.Text = null;
                    productQuantityToBuy.Text = null;
                }
            }
        }

        private async void WriteToFile(FileStream file,  string filename)
        {
            byte[] buffer = Encoding.Default.GetBytes(filename.ToCharArray());
            await file.WriteAsync(buffer, 0, buffer.Length);
        }

        private void logOutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }


    public class History
    {
        ProductDB abvanceHW = new ProductDB();
        public string ProductName { get; set; }
        public int Quantity { get; set; }

        public float Cost { get; set; }
        public History(string name, int amount)
        {
            this.ProductName = name;
            this.Quantity = amount;
            /*var item = abvanceHW.ProductInfo.First(i => i.ProductName == name).Cost;*/
            Cost = amount * abvanceHW.ProductInfo.First(i => i.ProductName == name).Cost;
        }
    }
}
