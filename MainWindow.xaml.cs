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

using Ookii.Dialogs.Wpf;
using LibGit2Sharp;

using ProjectGFN.Clients;
using ProjectGFN.Extensions;
using ProjectGFN.Windows;
using ProjectGFN.Windows.Git;

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
            Action<LoginWindow> onLogin = async s =>
            {
                void Initialize()
                {
                    xRepo.Content = $"{GitManager.UserName}/\n";
                }

                s.Close();
                this.IsEnabled = true;

                var repo = await GitManager.GetRepositoriesAsync();
                var repoMap = new Dictionary<string, List<string>>();

                foreach (var item in repo)
                {
                    string userName = item.Owner.Login;

                    if (repoMap.TryGetValue(userName, out var repoList))
                    {
                        repoList.Add(item.Name);
                    }
                    else
                    {
                        repoMap.Add(userName, new List<string>());
                        repoMap[userName].Add(item.Name);
                    }
                }

                RepoWindow.Initialize(repoMap);
                Initialize();

                MessageBox.Show($"Welcome, {GitManager.UserName}!", MainTitle, MessageBoxButton.OK,
                    MessageBoxImage.Information);
            };

            var login = new LoginWindow(onLogin);
            login.Show();

            this.IsEnabled = false;
        }

        public void CloneFromOtherRepository()
        {
            async void OnSelected(string owner, string name)
            {
                if (string.IsNullOrWhiteSpace(owner) || string.IsNullOrWhiteSpace(name))
                {
                    return;
                }

                GitHubRepo repo = await GitManager.Client.Repository.Get(owner, name);
                string path = string.Empty;

                if (MessageBox.Show("Please select the directory where to clone repository.\nWould you like to continue?", MainTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var dialog = new VistaFolderBrowserDialog();

                    if (dialog.ShowDialog(this).GetValueOrDefault())
                    {
                        path = $@"{dialog.SelectedPath}\{name}";
                        await repo.CloneAsync(path);

                        MessageBox.Show("Cloned the repository successfully.", MainTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }

            var repoWindow = new RepoWindow(OnSelected);
            repoWindow.Show();
        }

        private void xPush_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void xRepo_Click(object sender, RoutedEventArgs e)
        {
            Action<string, string> onSelected = async (owner, name) =>
            {
                await RepositoryManager.Initialize(owner, name);
                xRepo.Content = $"{owner}/\n{name}";
            };

            var repoWindow = new RepoWindow(onSelected);
            repoWindow.Show();
        }

        private void xClone_Click(object sender, RoutedEventArgs e)
        {
            CloneFromOtherRepository();
        }

        private void xAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void xCreate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
