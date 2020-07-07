using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml;

namespace Canteen
{
    public partial class MainWindow : Window
    {
        XmlDocument doc = new XmlDocument();
        XmlNodeList nodes;
        List<String> names = new List<String>();

        public MainWindow()
        {
            InitializeComponent();

            doc.Load("CustomerBalance.xml");
            LoadCustomerList();
        }

        private void LoadCustomerList()
        {
            nodes = doc.GetElementsByTagName("name");

            names = new List<String>();

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

        #region Admin
        private void btnAddCamper_Click(object sender, RoutedEventArgs e)
        {
            if (txtAddCamper.Text.Equals("") || txtAddCamper.Text == null)
                return;

            foreach(String name in names)
            {
                if (txtAddCamper.Text.Equals(name))
                {
                    MessageBox.Show("A camper with this name is already saved!");
                    return;
                }
            }

            XmlElement newCustomer = doc.CreateElement("", "customer", "");

            XmlElement addedName = doc.CreateElement("", "name", "");
            addedName.InnerText = txtAddCamper.Text;

            XmlElement addedBalNode = doc.CreateElement("", "balance", "");
            addedBalNode.InnerText = "0";

            newCustomer.AppendChild(addedName);
            newCustomer.AppendChild(addedBalNode);
            doc.DocumentElement.AppendChild(newCustomer);

            UpdateData();

            txtAddCamper.Text = "";
            MessageBox.Show("Camper added!");
        }

        private void btnRmvCamper_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRmvCamper.SelectedItem == null || cmbRmvCamper.SelectedItem.ToString().Equals(""))
                return;

            String name = cmbRmvCamper.SelectedItem.ToString();
            XmlNode thisNode;

            foreach(XmlNode node in nodes)
            {
                if(node.InnerText.Equals(name))
                {
                    thisNode = node.ParentNode;
                    doc.DocumentElement.RemoveChild(thisNode);
                    UpdateData();

                    MessageBox.Show(name + " removed!");
                }
            }
        }

        private void btnViewBalance_Click(object sender, RoutedEventArgs e)
        {
            if (cmbViewBalance.SelectedItem == null || cmbViewBalance.SelectedItem.ToString().Equals(""))
                return;

            String name = cmbViewBalance.SelectedItem.ToString();
            XmlNode thisNode;

            foreach (XmlNode node in nodes)
            {
                if (node.InnerText.Equals(name))
                {
                    thisNode = node.NextSibling;
                    blkBal.Text = name + " has " + float.Parse(thisNode.InnerText).ToString(("c2"));
                }
            }
        }

        private void btnAddBalance_Click(object sender, RoutedEventArgs e)
        {
            if (cmbAddBalance.SelectedItem == null || cmbAddBalance.SelectedItem.ToString().Equals(""))
                return;

            if(txtAddBalance.Text == null || txtAddBalance.Text.Equals(""))
            {
                MessageBox.Show("Please input a number to add!");
                return;
            }

            String name = cmbAddBalance.SelectedItem.ToString();
            float addBal = float.Parse(txtAddBalance.Text);

            XmlNode thisNode;

            foreach (XmlNode node in nodes)
            {
                if (node.InnerText.Equals(name))
                {
                    thisNode = node.NextSibling;
                    float curBal = float.Parse(thisNode.InnerText);
                    thisNode.InnerText = (curBal + addBal).ToString();
                    UpdateData();

                    txtAddBalance.Text = "";
                    MessageBox.Show(name + " added balance!");
                }
            }
        }

        private void btnCharge_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCharge.SelectedItem == null || cmbCharge.SelectedItem.ToString().Equals(""))
                return;

            if (txtCharge.Text == null || txtCharge.Text.Equals(""))
            {
                MessageBox.Show("Please input a number to charge!");
                return;
            }

            String name = cmbCharge.SelectedItem.ToString();
            float rmvBal = float.Parse(txtCharge.Text);

            XmlNode thisNode;

            foreach (XmlNode node in nodes)
            {
                if (node.InnerText.Equals(name))
                {
                    thisNode = node.NextSibling;
                    float curBal = float.Parse(thisNode.InnerText);

                    if(curBal - rmvBal < 0)
                    {
                        MessageBox.Show(name + " does not have enough money!");
                        return;
                    }


                    thisNode.InnerText = (curBal - rmvBal).ToString();
                    UpdateData();

                    txtCharge.Text = "";
                    MessageBox.Show(name + " charged successfully!");
                }
            }
        }
        #endregion


    }
}
