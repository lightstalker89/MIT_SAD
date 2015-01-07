using ServiceData;
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

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool soa = true;

        public MainWindow()
        {
            InitializeComponent();

            setListBoxCustomer();
        }

        private void buttonAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (ServiceFactory.GetService(soa).AddCustomer(new Customer() { Name = textBoxCustomer.Text }))
            {
                setListBoxCustomer();
            }
        }

        private void buttonDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxCustomer.SelectedItem != null)
            {
                if (ServiceFactory.GetService(soa).DeleteCustomer((Customer)listBoxCustomer.SelectedItem))
                {
                    setListBoxCustomer();
                }
            }
        }

        private void buttonAddOrder_Click(object sender, RoutedEventArgs e)
        {
            if(listBoxCustomer.SelectedItem != null)
            {
                if(ServiceFactory.GetService(soa).AddOrder(new Order() { Name = textBoxOrder.Text, CustomerName = ((Customer)listBoxCustomer.SelectedItem).Name }))
                {
                    setListBoxOrder();
                }
            }
        }

        private void buttonDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxOrder.SelectedItem != null)
            {
                if (ServiceFactory.GetService(soa).DeleteOrder((Order)listBoxOrder.SelectedItem))
                {
                    setListBoxOrder();
                }
            }
        }

        private void radioButtonSOA_Checked(object sender, RoutedEventArgs e)
        {
            soa = true;
            setListBoxCustomer();
        }

        private void radioButtonREST_Checked(object sender, RoutedEventArgs e)
        {
            soa = false;
            setListBoxCustomer();
        }

        private void listBoxCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (listBoxCustomer.SelectedItem != null)
            {
                setListBoxOrder();
            }
        }

        private void setListBoxCustomer()
        {
            listBoxCustomer.ItemsSource = ServiceFactory.GetService(soa).GetCustomers();
            listBoxCustomer.DisplayMemberPath = "Name";

            listBoxOrder.ItemsSource = null;
        }

        private void setListBoxOrder()
        {
            listBoxOrder.ItemsSource = ServiceFactory.GetService(soa).GetOrders(((Customer)listBoxCustomer.SelectedItem).Name);
            listBoxOrder.DisplayMemberPath = "Name";
        }
    }
}
