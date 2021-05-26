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
using ProjectGFN.Others;

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

            if (await GitManager.Initialize(token))
            {
                _onLogin(this);
            }
            else
            {
                string reason = "Invalid token";

                if (!Network.IsAvailable)
                {
                    reason = "Network not available";
                }

                MessageBox.Show($"Can't login on GitHub.\n{reason}", MainWindow.MainTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
