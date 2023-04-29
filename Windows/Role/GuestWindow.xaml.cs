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
    /// Логика взаимодействия для GuestWindow.xaml
    /// </summary>
    public partial class GuestWindow : Window
    {
        public GuestWindow()
        {
            InitializeComponent();
        }

        private void showItemsBtn_Click(object sender, RoutedEventArgs e)
        {
            openWindow(new ItemsWindow());
        }
        private void openWindow(Window w)
        {
            this.Visibility = Visibility.Hidden;
            w.Closed += (o, e) =>
            {
                this.Visibility = Visibility.Visible;
            };
        }
    }
}
