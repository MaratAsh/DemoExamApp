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

namespace WpfApp.Windows.Editor
{
    /// <summary>
    /// Логика взаимодействия для ProductEditorWindow.xaml
    /// </summary>
    public partial class ProductEditorWindow : Window
    {
        Action<Models.Product?> saveHandler;
        public Models.Product product { get; set; }
        public List<Models.UnitType> units { get; set; } = new List<Models.UnitType>();
        public List<Models.ProductManufacturer> manufacturers { get; set; } = new List<Models.ProductManufacturer>();
        public ProductEditorWindow(Action<Models.Product?> saveHandler)
        {
            InitializeComponent();
            product = new Models.Product()
            {
                ProductCategoryId = 1,
                ProductSupplierId = 1
            };
            DataContext = this;
            this.saveHandler = saveHandler;
        }
        public ProductEditorWindow(Models.Product product, Action<Models.Product?> saveHandler)
        {
            InitializeComponent();
            this.product = product;
            DataContext = this;
            this.saveHandler = saveHandler;
        }
        public ProductEditorWindow setUnits(List<Models.UnitType> units)
        {
            this.units = units;
            if (product != null && product.UnitTypeId == 0) {
                product.UnitType = units[0];
                product.UnitTypeId = units[0].UnitTypeId;
            }
            return this;
        }
        public ProductEditorWindow setManufacturers(List<Models.ProductManufacturer> manufacturers)
        {
            this.manufacturers = manufacturers;
            if (product != null && product.ProductManufacturerId == 0)
            {
                product.ProductManufacturer = manufacturers[0];
                product.ProductManufacturerId = manufacturers[0].ProductManufacturerId;
            }
            return this;
        }
        private void enterButton_Click(object sender, RoutedEventArgs e)
        {
            saveHandler?.Invoke(product);
            Close();
        }
    }
}
