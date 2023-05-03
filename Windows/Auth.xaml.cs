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
using static System.Collections.Specialized.BitVector32;

namespace WpfApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        int failed;
        DateTime lockedUntil;
        Context context;
        
        public Auth()
        {
            InitializeComponent();
            passwordInputShow();
            failed = 0;
            context = new Context();
            context.Roles.Load();
            context.Users.Load();
        }

        private void passwordTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (passwordTB.Visibility != Visibility.Visible)
                return;
            passwordPB.Password = passwordTB.Text;
        }

        private void passwordPB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (passwordPB.Visibility != Visibility.Visible)
                return;
            passwordTB.Text = passwordPB.Password;
        }

        private void passwordVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (passwordTB.Visibility == Visibility.Visible)
            {
                passwordInputHide();
            }
            else
            {
                passwordInputShow();
            }
        }
        private void passwordInputHide()
        {
            passwordTB.Visibility = Visibility.Hidden;
            passwordPB.Visibility = Visibility.Visible;
            passwordVisibility.Content = "Показать";
        }
        private void passwordInputShow()
        {
            passwordPB.Visibility = Visibility.Hidden;
            passwordTB.Visibility = Visibility.Visible;
            passwordVisibility.Content = "Скрыть";
        }

        private void entryButton_Click(object sender, RoutedEventArgs e)
        {
            if (loginTB.Text.Equals(""))
            {
                MessageBox.Show("Введите логин!");
                return;
            }
            if (passwordTB.Text.Equals(""))
            {
                MessageBox.Show("Введите пароль!");
                return;
            }
            string login = loginTB.Text, password = passwordTB.Text;
            Action authRequestAction = () =>
            {
                // check auth request
                Models.User user = authUserRequest(login, password);
                if (user == null)
                {
                    failed++;
                    MessageBox.Show("Некорректные данные");
                    return;
                }
                failed = 0;
                if (user.Role.RoleName == "Администратор"
                    || user.Role.RoleName == "Менеджер")
                {
                    App.openWindow(this, new AdminWindow(user));
                }
                else if(user.Role.RoleName == "Клиент")
                {
                    App.openWindow(this, new ClientWindow(user));
                }
            };
            Action capchaFailAction = () =>
            {
                TimeSpan timeSpan = new TimeSpan(0, 0, 10);
                lockedUntil = DateTime.Now + timeSpan;
                MessageBox.Show($"Попытки входа заблокированы на {timeSpan.Seconds} секунд!");
            };
            if (lockedUntil >= DateTime.Now)
            {
                MessageBox.Show($"Попробуйте еще раз через {lockedUntil.Second - DateTime.Now.Second} секунд!");
                return;
            }
            if (failed >= 1)
            {
                Window w;
                w = new CapchaWindow(authRequestAction, capchaFailAction);
                w.ShowDialog();
            }
            else
            {
                authRequestAction();
            }
        }
        private bool authRequest(string login, string password)
        {
            Models.User user = context.Users.First(u=>u.UserLogin==login);
            if (user != null && user.UserPassword == password)
                return true;
            return false;
        }
        private Models.User? authUserRequest(string login, string password)
        {
            var list = context.Users.ToList();
            Models.User? user = context.Users.FirstOrDefault(u => u.UserLogin == login);
            if (user == null)
                return null;
            if (user.UserPassword == password)
                return user;
            return null;
        }
        private void entryGuestButton_Click(object sender, RoutedEventArgs e)
        {
            Window w = new GuestWindow();
            openWindow(w);
        }
        private void openWindow(Window w)
        {
            w.Show();
            w.Closed += (o, e) =>
            {
                this.Visibility = Visibility.Visible;
            };
            this.Visibility = Visibility.Hidden;
        }
    }
}
