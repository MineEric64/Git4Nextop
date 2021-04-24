using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Octokit;

using ProjectGFN.Clients;

namespace ProjectGFN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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

            var user = await GitManager.GetCurrentUserAsync();

            foreach (var repo in GitManager.Repositories)
            {
                Trace.WriteLine(repo.Name);
            }
        }
    }
}
