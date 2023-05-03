using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static void openWindow(Window parent, Window w)
        {
            parent.Visibility = Visibility.Hidden;
            w.Closed += (o, e) =>
            {
                parent.Visibility = Visibility.Visible;
            };
            w.Show();
        }
    }
}
