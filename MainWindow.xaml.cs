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

using LibGit2Sharp;

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

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Action<LoginWindow> onLogin = async (LoginWindow s) =>
            {
                s.Close();
                this.IsEnabled = true;

                var repo = await GitManager.GetRepositoriesAsync();

                foreach (var item in repo)
                {
                    this.xRepos.Items.Add(item.Name);
                }
            };
            var login = new LoginWindow(onLogin);


            login.Show();
            this.IsEnabled = false;
        }

        private void xClone_Click(object sender, RoutedEventArgs e)
        {

        }

        private void xPush_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
