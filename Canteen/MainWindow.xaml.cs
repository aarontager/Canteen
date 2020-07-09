using System;
using System.ComponentModel;
using System.Windows;
using System.Xml;

namespace Canteen
{
    public partial class MainWindow : Window
    {
        XmlDocument doc = new XmlDocument();
        XmlNodeList nodes;
        BindingList<String> names = new BindingList<String>(), cart = new BindingList<string>();
        Double price = 0;
        String filename;

        public MainWindow()
        {
            filename = "CustomerBalance.xml";
            InitializeComponent();

            doc.Load(filename);
            LoadCustomerList();

            lbCart.ItemsSource = cart;
            lblPrice.Content = price.ToString("c2");
        }

        private void LoadCustomerList()
        {
            nodes = doc.GetElementsByTagName("name");

            foreach(XmlNode node in nodes)
            {
                names.Add(node.InnerText);
            }

            cmbRmvCamper.ItemsSource = names;
            cmbViewBalance.ItemsSource = names;
            cmbAddBalance.ItemsSource = names;
            cmbCharge.ItemsSource = names;
            lblPrice.Content = price.ToString("c2");
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

            names.Add(txtAddCamper.Text);
            newCustomer.AppendChild(addedName);
            newCustomer.AppendChild(addedBalNode);
            doc.DocumentElement.AppendChild(newCustomer);
            doc.Save(filename);

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
                    names.Remove(name);
                    doc.Save(filename);

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
                    blkBal.Text = name + " has " + Double.Parse(thisNode.InnerText).ToString("c2");
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
            Double addBal = Double.Parse(txtAddBalance.Text);

            XmlNode thisNode;

            foreach (XmlNode node in nodes)
            {
                if (node.InnerText.Equals(name))
                {
                    thisNode = node.NextSibling;
                    Double curBal = Double.Parse(thisNode.InnerText);
                    thisNode.InnerText = (curBal + addBal).ToString();
                    doc.Save(filename);

                    txtAddBalance.Text = "";
                    MessageBox.Show(name + " added balance!");
                }
            }
        }

        private void btnClearCharge_Click(object sender, RoutedEventArgs e)
        {
            txtCharge.Text = "";
            price = 0;
            lblPrice.Content = price.ToString("c2");
            cart.Clear();
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
            Double rmvBal = Double.Parse(txtCharge.Text);

            XmlNode thisNode;

            foreach (XmlNode node in nodes)
            {
                if (node.InnerText.Equals(name))
                {
                    thisNode = node.NextSibling;
                    Double curBal = Double.Parse(thisNode.InnerText);

                    if(curBal - rmvBal < 0)
                    {
                        MessageBox.Show(name + " does not have enough money!");
                        return;
                    }


                    thisNode.InnerText = (curBal - rmvBal).ToString();
                    doc.Save(filename);

                    txtCharge.Text = "";
                    price = 0;
                    lblPrice.Content = price.ToString("c2");
                    cart.Clear();
                    MessageBox.Show(name + " charged successfully!");
                }
            }
        }

        #endregion

        #region Dairy
        private void btnPizzaSlice_Click(object sender, RoutedEventArgs e)
        {
            price += 2.5;
            cart.Add("Pizza Slice");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnPizzaRoll_Click(object sender, RoutedEventArgs e)
        {
            price += 3;
            cart.Add("Pizza Roll");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnMozerallaSticks_Click(object sender, RoutedEventArgs e)
        {
            price += 1.5;
            cart.Add("Mozzerella Sticks");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnShoreshSlice_Click(object sender, RoutedEventArgs e)
        {
            price += 3;
            cart.Add("Shoresh Slice");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }
        #endregion

        #region Meat
        private void btnHotDog_Click(object sender, RoutedEventArgs e)
        {
            price += 2;
            cart.Add("Hot Dog");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnBurger_Click(object sender, RoutedEventArgs e)
        {

            price += 3.25;
            cart.Add("Burger");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnPastramiBurger_Click(object sender, RoutedEventArgs e)
        {
            price += 4;
            cart.Add("Pastrami Burger");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnShoreshBurger_Click(object sender, RoutedEventArgs e)
        {
            price += 5;
            cart.Add("Shoresh Burger");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnPoppers_Click(object sender, RoutedEventArgs e)
        {
            price += 5.5;
            cart.Add("Poppers");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnShnitzelSandwish_Click(object sender, RoutedEventArgs e)
        {
            price += 6;
            cart.Add("Shnitzel Sandwich");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnCholent_Click(object sender, RoutedEventArgs e)
        {
            price += 3;
            cart.Add("Cholent");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnPastramiDog_Click(object sender, RoutedEventArgs e)
        {
            price += 2.75;
            cart.Add("Pastrami Dog");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }
        #endregion

        #region Parve
        private void btnTraditionSoup_Click(object sender, RoutedEventArgs e)
        {
            price += 1;
            cart.Add("Tradition Soup");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnHotPretzel_Click(object sender, RoutedEventArgs e)
        {
            price += 1;
            cart.Add("Hot Pretzel");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnRegFries_Click(object sender, RoutedEventArgs e)
        {
            price += 2.5;
            cart.Add("Regular Fries");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnSpicyFries_Click(object sender, RoutedEventArgs e)
        {
            price += 3;
            cart.Add("Spicy Fries");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnMixedFries_Click(object sender, RoutedEventArgs e)
        {
            price += 2.75;
            cart.Add("Mixed Fries");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnPotatoKnish_Click(object sender, RoutedEventArgs e)
        {
            price += 1;
            cart.Add("Potato Knish");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }
        #endregion

        #region Snacks
        private void btnMikeIkes_Click(object sender, RoutedEventArgs e)
        {
            price += .25;
            cart.Add("Mike & Ikes");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnZours_Click(object sender, RoutedEventArgs e)
        {
            price += .35;
            cart.Add("Zours");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnTaffy_Click(object sender, RoutedEventArgs e)
        {
            price +=.15;
            cart.Add("Laffy Taffy");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnTaffyBar_Click(object sender, RoutedEventArgs e)
        {
            price += 1;
            cart.Add("Laffy Taffy Bar");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnFruitBFoot_Click(object sender, RoutedEventArgs e)
        {
            price += .5;
            cart.Add("Fruit by the Foot");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnRancher_Click(object sender, RoutedEventArgs e)
        {
            price += 1;
            cart.Add("Jolly Rancher");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnSourStix_Click(object sender, RoutedEventArgs e)
        {
            price += 1.25;
            cart.Add("Sour Stix");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnDubbleBubble_Click(object sender, RoutedEventArgs e)
        {
            price += .1;
            cart.Add("Dubble Bubble");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnMust_Click(object sender, RoutedEventArgs e)
        {
            price += 1.5;
            cart.Add("Must Gum");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnMilkMunch_Click(object sender, RoutedEventArgs e)
        {
            price += 1.25;
            cart.Add("Milk Munch");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnLaHit_Click(object sender, RoutedEventArgs e)
        {
            price += 1.35;
            cart.Add("La Hit");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnKlikIn_Click(object sender, RoutedEventArgs e)
        {
            price += 1.35;
            cart.Add("Klin In");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnEncore_Click(object sender, RoutedEventArgs e)
        {
            price += 1.25;
            cart.Add("Encore");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnSuperSnacks_Click(object sender, RoutedEventArgs e)
        {
            price += .5;
            cart.Add("Super Snacks");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnDipsyDoodles_Click(object sender, RoutedEventArgs e)
        {
            price += .35;
            cart.Add("Dipsy Doodles");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }
        private void btnPringles_Click(object sender, RoutedEventArgs e)
        {
            price += 1;
            cart.Add("Pringles");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }
        #endregion

        #region Drinks
        private void btnSodaBottle_Click(object sender, RoutedEventArgs e)
        {
            price += 1.5;
            cart.Add("Soda Bottle");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnSodaCan_Click(object sender, RoutedEventArgs e)
        {
            price += 1;
            cart.Add("Soda Can");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnWater_Click(object sender, RoutedEventArgs e)
        {
            price += 1;
            cart.Add("Water");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnGatorade_Click(object sender, RoutedEventArgs e)
        {
            price += 1.5;
            cart.Add("Gatorade");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnSnapple_Click(object sender, RoutedEventArgs e)
        {
            price += 1.5;
            cart.Add("Snapple");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnSeltzer_Click(object sender, RoutedEventArgs e)
        {
            price += 1;
            cart.Add("Seltzer Can");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }
        #endregion

        #region Misc
        private void btnDanish_Click(object sender, RoutedEventArgs e)
        {
            price += 1.25;
            cart.Add("Danish");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnJumboPop_Click(object sender, RoutedEventArgs e)
        {
            price += .5;
            cart.Add("Jumbo Pop");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnJerky_Click(object sender, RoutedEventArgs e)
        {
            price += 5;
            cart.Add("Beef Jerky");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnSlush_Click(object sender, RoutedEventArgs e)
        {
            price += 1;
            cart.Add("Slush");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }

        private void btnCoffee_Click(object sender, RoutedEventArgs e)
        {
            price += 1.25;
            cart.Add("Iced Coffee");
            lblPrice.Content = price.ToString("c2");
            txtCharge.Text = price.ToString();
        }
        #endregion
    }
}
