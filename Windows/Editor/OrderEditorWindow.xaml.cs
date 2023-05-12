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
        public List<Models.OrderProduct> orderProducts { get; set; }
        public OrderEditorWindow(Models.Order order, List<Models.OrderProduct> orderProducts)
        {
            InitializeComponent();
            this.order = order;
            this.orderProducts = orderProducts;
        }
        public OrderEditorWindow setStatuses(List<Models.OrderStatus> orderStatuses)
        {
            statuses = orderStatuses;
            return this;
        }
        public OrderEditorWindow setPickupPoints(List<Models.PickupPoint> pickupPoints)
        {
            this.pickupPoints = pickupPoints;
            return this;
        }
    }
}
