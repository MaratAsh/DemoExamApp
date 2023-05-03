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
using System.Windows.Shapes;

namespace WpfApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        public Models.User? user { get; set; }

        public ClientWindow(Models.User? user)
        {
            InitializeComponent();
            this.user = user;
            DataContext = this;
        }

        private void showItemsBtn_Click(object sender, RoutedEventArgs e)
        {
            App.openWindow(this, new ProductsWindow(user));
        }

        private void showOrdersBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
