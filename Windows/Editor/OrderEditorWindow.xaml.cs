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

namespace WpfApp.Windows.Editor
{
    /// <summary>
    /// Логика взаимодействия для OrderEditorWindow.xaml
    /// </summary>
    public partial class OrderEditorWindow : Window
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

        }
    }
}
