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

using ProjectGFN.Clients;

namespace ProjectGFN
{
    /// <summary>
    /// LoginWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly Action<LoginWindow> _onLogin;

        public LoginWindow(Action<LoginWindow> onLogin)
        {
            InitializeComponent();
            _onLogin = onLogin;
        }

        private async void xLogin_Click(object sender, RoutedEventArgs e)
        {
            var token = xToken.Password;

            if (string.IsNullOrWhiteSpace(token))
            {
                MessageBox.Show("Can't login", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            await GitManager.Initialize(token);
            _onLogin(this);
        }
    }
}
