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
using WpfApp.Models;

namespace WpfApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProductsWindow.xaml
    /// </summary>
    public partial class ProductsWindow : Window
    {
        Models.User? user;
        User25Context context;
        public List<string> filterSales { get; set; }
        public List<string> sortTypes { get; set; }
        public List<Models.Product> products { get; set; }
        public ProductsWindow(Models.User? user)
        {
            InitializeComponent();
            DataContext = this;
            this.user = user;
            context = new User25Context();
            context.Products.Load();
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
        
        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            productCalculation();
        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                                    || prod.ProductName.EndsWith(searchQueryBox.Text)
                                    || prod.ProductName.Contains(searchQueryBox.Text));
            }

            if (sortComboBox.SelectedItem as string == "Сортировка по возрастанию")
            {
                p = p.OrderBy(prod => prod.ProductMaxDiscountAmount);
            }
            else if (sortComboBox.SelectedItem as string == "Сортировка по убыванию")
            {
                p = p.OrderByDescending(prod => prod.ProductMaxDiscountAmount);
            }

            if (filterComboBox.SelectedItem as string == "0-9,99%")
            {
                p = p.Where(prod => prod.ProductMaxDiscountAmount < 10);
            }
            else if (filterComboBox.SelectedItem as string == "10-14,99%")
            {
                p = p.Where(prod => prod.ProductMaxDiscountAmount < 15);
            }
            else if (filterComboBox.SelectedItem as string == "15% и более")
            {
                p = p.Where(prod => prod.ProductMaxDiscountAmount >= 15);
            }
            products = p.ToList();
            currentCountBox.Text = products.Count().ToString();
            fullCountBox.Text = context.Products.Count().ToString();
            productsContainer.ItemsSource = products;
        }

        private void deleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is not Models.Product product){
                return;
            }
            MessageBox.Show($"Delete {product.ProductName}");
        }

        private void editProductButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is not Models.Product product)
            {
                return;
            }
            App.openWindow(this, new Editor.ProductEditorWindow(product, (editedProduct) =>
            {
                context.SaveChanges();
                productCalculation();
            }).setUnits(context.UnitTypes.ToList())
                .setManufacturers(context.ProductManufacturers.ToList()));
        }
        private void addProductButton_Click(object sender, RoutedEventArgs e)
        {
            App.openWindow(this, new Editor.ProductEditorWindow((product) =>
            {
                context.Products.Add(product);
                context.SaveChanges();
                productCalculation();
            }).setUnits(context.UnitTypes.ToList())
                .setManufacturers(context.ProductManufacturers.ToList()));
        }
    }
}
