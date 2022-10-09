using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

using CryptoNet;
using CryptoNet.Utils;
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
            var password = xPassword.Password;

            if (xPasswordCheck.IsChecked.HasValue && xPasswordCheck.IsChecked.Value && !string.IsNullOrWhiteSpace(password))
            {
                var buffer = Convert.FromBase64String(token);
                var aes = GetCryptoAes(password);

                token = aes.DecryptToString(buffer);
            }

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

        private CryptoNetAes GetCryptoAes(string plaintext)
        {
            const int keyLength = 32;
            const int ivLength = 16;

            string shaed = ComputeSHA256Hash(plaintext);
            byte[] key = Encoding.UTF8.GetBytes(shaed.Substring(0, keyLength));
            byte[] iv = Encoding.UTF8.GetBytes(shaed.Substring(keyLength, ivLength));

            return new CryptoNetAes(key, iv);
        }

        public static string ComputeSHA256Hash(string text)
        {
            using (var sha256 = new SHA256Managed())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "").ToLower();
            }
        }

        private void xPasswordCheck_Checked(object sender, RoutedEventArgs e)
        {
            xPassword.IsEnabled = xPasswordCheck.IsChecked.Value;
        }
    }
}
