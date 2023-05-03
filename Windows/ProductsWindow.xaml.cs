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
        public List<string> filterSales { get; set; }
        public List<string> sortTypes { get; set; }
        public List<Models.Product> products { get; set; }
        public ProductsWindow(Models.User? user)
        {
            InitializeComponent();
            DataContext = this;
            this.user = user;
            context = new Context();
            context.Products.Load();
            products = context.Products.ToList();
            filterSales = new List<string>();
            filterSales.Add("Все диапазоны");
            filterSales.Add("0-9,99%");
            filterSales.Add("10-14,99%");
            filterSales.Add("15% и более");
            sortTypes = new List<string>();
            sortTypes.Add("Без сортировки");
            sortTypes.Add("Сортировка по возрастанию");
            sortTypes.Add("Сортировка по убыванию");
            productCalculation();
        }

        private void filterListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            productCalculation();
        }

        private void sortListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            productCalculation();
        }

        private void searchQueryBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            productCalculation();
        }

        private void searchQueryButton_Click(object sender, RoutedEventArgs e)
        {
            searchQueryBox.Text = "";
            productCalculation();
        }

        private void productCalculation()
        {
            IQueryable<Models.Product> p = context.Products;
            if (searchQueryBox.Text != "")
            {
                p = p.Where(prod => prod.ProductName.StartsWith(searchQueryBox.Text)
                                    || prod.ProductName.EndsWith(searchQueryBox.Text));
            }

            if (sortListBox.SelectedItem as string == "Сортировка по возрастанию")
            {
                p = p.OrderBy(prod => prod.ProductMaxDiscountAmount);
            }
            else if (sortListBox.SelectedItem as string == "Сортировка по убыванию")
            {
                p = p.OrderByDescending(prod => prod.ProductMaxDiscountAmount);
            }

            if (filterListBox.SelectedItem as string == "0-9,99%")
            {
                p = p.Where(prod => prod.ProductMaxDiscountAmount < 10);
            }
            else if (filterListBox.SelectedItem as string == "10-14,99%")
            {
                p = p.Where(prod => prod.ProductMaxDiscountAmount < 15);
            }
            else if (filterListBox.SelectedItem as string == "15% и более")
            {
                p = p.Where(prod => prod.ProductMaxDiscountAmount >= 15);
            }
            products = p.ToList();
        }
    }
}
