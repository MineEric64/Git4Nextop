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

using GitHubRepo = Octokit.Repository;
using GitRepo = LibGit2Sharp.Repository;

namespace ProjectGFN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string MainTitle = "Git4Nextop";

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
                    this.xRepos.Items.Add(item.FullName);
                }
            };
            var login = new LoginWindow(onLogin);


            login.Show();
            this.IsEnabled = false;
        }

        private async void xClone_Click(object sender, RoutedEventArgs e)
        {
            string fullName = xRepos.SelectedItem as string;

            if (!string.IsNullOrWhiteSpace(fullName))
            {
                string[] names = fullName.Split(new[] { '/' });

                if (names.Length >= 2)
                {
                    string owner = names[0];
                    string name = names[1];

                    GitHubRepo repo = await GitManager.Client.Repository.Get(owner, name);
                    await GitManager.CloneAsync(repo.CloneUrl, "");

                    MessageBox.Show("CLONE SUCESSFULLY", MainTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void xPush_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
