using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml;

namespace Canteen
{
    public partial class MainWindow : Window
    {
        XmlDocument doc = new XmlDocument();
        List<string> names = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            doc.Load("CustomerBalance.xml");
            LoadCustomerList();
        }

        private void LoadCustomerList()
        {
            XmlNodeList nodes;
            nodes = doc.GetElementsByTagName("name");

            names = new List<string>();

            foreach(XmlNode node in nodes)
            {
                names.Add(node.InnerText);
            }

            cmbRmvCamper.ItemsSource = names;
            cmbViewBalance.ItemsSource = names;
            cmbAddBalance.ItemsSource = names;
            cmbCharge.ItemsSource = names;
        }

        private void UpdateData()
        {
            doc.Save("CustomerBalance.xml");
            LoadCustomerList();
        }

        private void btnAddCamper_Click(object sender, RoutedEventArgs e)
        {
            if (txtAddCamper.Text.Equals("") || txtAddCamper.Text == null)
                return;

            foreach(string name in names)
            {
                if (txtAddCamper.Text.Equals(name))
                {
                    MessageBox.Show("A camper with this name is already saved!");
                    return;
                }
            }

            XmlElement newCustomer = doc.CreateElement("", "customer", "");

            XmlElement newName = doc.CreateElement("", "name", "");
            newName.InnerText = txtAddCamper.Text;

            XmlElement newBal = doc.CreateElement("", "balance", "");
            newBal.InnerText = "0";

            newCustomer.AppendChild(newName);
            newCustomer.AppendChild(newBal);
            doc.DocumentElement.AppendChild(newCustomer);

            UpdateData();
        }
    }
}
