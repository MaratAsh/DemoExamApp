using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
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
        public Models.User? user { get; set; }
        User25Context context;
        public List<string> filterSales { get; set; }
        public List<string> sortTypes { get; set; }
        public List<Models.Product> products { get; set; }
        public Models.Order order { get; set; }
        public List<Models.OrderProduct> orderProducts { get; set; }
        

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
            orderPanel.Visibility = Visibility.Hidden;
            //userPanel.Text = user == null ? "Гость" : $"{user.UserSurname} {user.UserName} {user.UserPatronymic}";
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
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Debug.WriteLine(@"Entity of type ""{0}"" in state ""{1}"" 
                   has the following validation errors:",
                            eve.Entry.Entity.GetType().Name,
                            eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Debug.WriteLine(@"- Property: ""{0}"", Error: ""{1}""",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                productCalculation();
            }).setUnits(context.UnitTypes.ToList())
                .setManufacturers(context.ProductManufacturers.ToList()));
        }

        private void Grid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var product = (sender as Grid).DataContext as Models.Product;

        }

        private void productContextMenu_Opened(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_OrderAdd_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            var product = mi.DataContext as Models.Product;
            if (orderProducts == null || order == null)
            {
                orderPanel.Visibility = Visibility.Visible;
                order = new Order()
                {
                    OrderStatus = context.OrderStatuses.First(),
                    UserId = (user == null ? null : user.UserId),
                    OrderCreateDate = DateTime.Now,
                    OrderDeliveryDate = new DateTime(1970, 1, 1),
                    PickupPoint = context.PickupPoints.First()
                };
                orderProducts = new List<OrderProduct>();
                context.Orders.Add(order);
                context.SaveChanges();
            }
            mi.Visibility = Visibility.Hidden;
            OrderProduct? orderProduct = orderProducts.Find((orderProduct) => {
                if (orderProduct == null) return false;
                return orderProduct.Product == product;
            });
            if (orderProduct != null)
                return;
            orderProduct = new OrderProduct()
            {
                Order = order,
                Product = product,
                Count = 1
            };
            orderProducts.Add(orderProduct);
            context.SaveChanges();
        }

        private void showOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            if (order == null)
                return;
        }
    }
}
