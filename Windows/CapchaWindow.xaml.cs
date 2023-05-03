using EasyCaptcha.Wpf;
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

namespace WpfApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для CapchaWindow.xaml
    /// </summary>
    public partial class CapchaWindow : Window
    {
        private string current;
        private Action onConfirmation;
        private Action onFailure;

        public CapchaWindow(Action onConfirm, Action onFail)
        {
            InitializeComponent();
            capcha.Text = "";
            generate();
            onConfirmation = onConfirm;
            onFailure = onFail;
        }

        private string generateString()
        {
            const string AllowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random rng = new Random();
            string str = "";
            int strLenght = rng.Next(4, 10);

            for (int i = 0; i < strLenght; i++)
                str += AllowedChars[rng.Next(0, AllowedChars.Length)];
            return str;
        }
        private void generate()
        {
            MyCaptcha.CreateCaptcha(Captcha.LetterOption.Alphanumeric, 5);
            capcha.Text = MyCaptcha.CaptchaText;
            capchaInput.Text = "";
        }

        private void enterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (capchaInput.Text == MyCaptcha.CaptchaText)
            {
                Close();
                onConfirmation();
            }
            else
            {
                Close();
                onFailure();
            }
        }

        private void changeBtn_Click(object sender, RoutedEventArgs e)
        {
            generate();
        }
    }
}
