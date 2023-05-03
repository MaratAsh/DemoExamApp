using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для ProductsWindow.xaml
    /// </summary>
    public partial class ProductsWindow : Window
    {
        Models.User? user;
        Context context;
        public List<Models.Product> products { get; set; }
        public ProductsWindow(Models.User? user)
        {
            InitializeComponent();
            DataContext = this;
            this.user = user;
            context = new Context();
            context.Products.Load();
            products = context.Products.ToList();
        }
    }
}
