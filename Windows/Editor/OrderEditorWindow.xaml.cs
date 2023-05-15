using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Microsoft.Office.Interop;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using System.IO;

namespace WpfApp.Windows.Editor
{
    /// <summary>
    /// Логика взаимодействия для OrderEditorWindow.xaml
    /// </summary>
    public partial class OrderEditorWindow : System.Windows.Window
    {
        public List<OrderStatus> statuses { get; set; }
        public List<PickupPoint> pickupPoints { get; set; }
        public Order order { get; set; }
        public decimal orderCost {
            get
            {
                return order.Cost;
            }
            set
            {

            }
        }
        public List<Models.OrderProduct> orderProducts { get; set; }
        public Models.User user { get; set; }
        private Action<Models.Order> _action;
        public OrderEditorWindow(Models.User user, Models.Order order, List<Models.OrderProduct> orderProducts)
        {
            InitializeComponent();
            this.user = user;
            this.order = order;
            this.orderProducts = orderProducts;
            DataContext = this;
            Closed += (s, e) =>
            {
                _action!.Invoke(order);
            };
        }
        public OrderEditorWindow setStatuses(List<Models.OrderStatus> orderStatuses)
        {
            statuses = orderStatuses;
            orderPickupPointSelector.SelectedItem = order.PickupPoint;
            return this;
        }
        public OrderEditorWindow setPickupPoints(List<Models.PickupPoint> pickupPoints)
        {
            this.pickupPoints = pickupPoints;
            orderPickupPointSelector.SelectedItem = order.PickupPoint;
            return this;
        }
        public OrderEditorWindow setAction(Action<Models.Order> action)
        {
            this._action = action;
            return this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void countAddBtn_Click(object sender, RoutedEventArgs e)
        {
            var orderProduct = (sender as Button).DataContext as Models.OrderProduct;

            if (orderProduct == null)
                return;
            orderProduct.Count += 1;
            orderProductsPanel.Items.Refresh();
            orderCostLabel.Text = order.Cost.ToString();
        }

        private void countRemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            var orderProduct = (sender as Button).DataContext as Models.OrderProduct;

            if (orderProduct == null)
                return;
            
            if (orderProduct.Count == 1)
            {
                var result = MessageBox.Show("Подтверждаете удаление товара из заказа?", "Подтверждение удаления продукта", MessageBoxButton.YesNo);
                
                if (result == MessageBoxResult.Yes)
                {
                    orderProducts.Remove(orderProduct);
                    order.OrderProducts.Remove(orderProduct);
                }
            }
            orderProduct.Count -= 1;
            orderProductsPanel.Items.Refresh();
            orderCostLabel.Text = order.Cost.ToString();
        }

        private void pdfCreatorButton_Click(object sender, RoutedEventArgs e)
        {
            var app = new Microsoft.Office.Interop.Word.Application();
            Document document = app.Documents.Add();
            var header = document.Paragraphs.Add();
            header.Range.Text = $"Заказ №{order.OrderId}";
            header.Range.InsertParagraphAfter();
            var date = document.Paragraphs.Add();
            date.Range.Text = $"От {order.OrderCreateDate}";
            date.Range.InsertParagraphAfter();
            var recieve = document.Paragraphs.Add();
            recieve.Range.Text = $"Для получения заказа вы выбрали: {order.PickupPoint.Address}";
            recieve.Range.InsertParagraphAfter();
            var code = document.Paragraphs.Add();
            code.Range.Text = "Код для получения: ";
            code.Range.Font.Size = 16;
            code.Range.Font.BoldBi = 1;
            code.Range.InsertAfter(order.OrderGetCode.ToString());
            code.Range.Font.BoldBi = 0;
            code.Range.InsertParagraphAfter();
            var products = document.Paragraphs.Add();
            var range = products.Range;
            range.InsertParagraphAfter();

            var table = document.Tables.Add(range, order.OrderProducts.Count + 1, 5);
            table.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            table.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
            Row row = table.Rows[1];
            row.Cells[1].Range.Text = "Номер";
            row.Cells[2].Range.Text = "Название";
            row.Cells[3].Range.Text = "Цена за единицу";
            row.Cells[4].Range.Text = "Количество";
            row.Cells[5].Range.Text = "Цена";
            var list = order.OrderProducts.ToList();
            for (int i = 0; i < order.OrderProducts.Count(); i++)
            {
                row = table.Rows[i + 2];
                row.Cells[1].Range.Text = (i + 1).ToString();
                row.Cells[2].Range.Text = list[i].Product.ProductName.ToString();
                row.Cells[3].Range.Text = list[i].Product.ProductCost.ToString();
                row.Cells[4].Range.Text = list[i].Count.ToString();
                row.Cells[5].Range.Text = list[i].Cost.ToString();
            }
            var result = document.Paragraphs.Add();
            result.Range.Text = $"Итого: {order.Cost}";
            result.Range.InsertParagraphAfter();
            app.Visible = false;
            document.SaveAs2($"Заказ-{order.OrderId}-{DateTime.Now.Millisecond}.pdf", WdExportFormat.wdExportFormatPDF);
        }
    }
}
