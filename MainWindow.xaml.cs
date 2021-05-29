﻿using System;
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
using ProjectGFN.Properties;

using GitHubRepo = Octokit.Repository;
using GitRepo = LibGit2Sharp.Repository;

using Res = ProjectGFN.Properties.Resources;

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
            void InitializeBef()
            {
                xIcon.Source = BitmapConverter.FromBitmap(Res.Git4Nextop);
                xRepositoryGrid.IsEnabled = false;
            }

            InitializeBef();

            async void OnLogin(LoginWindow s)
            {
                void InitializeAf()
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
                InitializeAf();

                MessageBox.Show($"Welcome, {GitManager.UserName}!", MainTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }

            var login = new LoginWindow(OnLogin);
            login.Show();

            this.IsEnabled = false;
        }

        public void Clone()
        {
            async void OnSelected(object sender, RepoEventArgs e)
            {
                GitHubRepo repo = await GitManager.Client.Repository.Get(e.Owner, e.Name);
                await CloneAsync(repo);
            }

            var repoWindow = new RepoWindow();
            repoWindow.Selected += OnSelected;

            repoWindow.Show();
        }

        public async Task CloneAsync(GitHubRepo repo)
        {
            async void OnSelected(object sender, CloneEventArgs e)
            {
                await CloneAsync(repo, e.BranchName, e.Path);
            }

            var cloneWindow = new CloneWindow();

            await cloneWindow.InitializeAsync(repo);
            cloneWindow.Selected += OnSelected;

            cloneWindow.Show();
        }

        public async Task CloneAsync(GitHubRepo repo, string branchName, string path)
        {
            RepositoryManager.RepositoryPath = path;

            await repo.CloneAsync(branchName, RepositoryManager.RepositoryPath);
            await SelectRepository(repo, branchName);

            if (!xRepositoryGrid.IsEnabled)
            {
                xRepositoryGrid.IsEnabled = true;
            }

            MessageBox.Show("Cloned the repository successfully.", MainTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void xPush_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void xRepo_Click(object sender, RoutedEventArgs e)
        {
            async void OnSelected(object s, RepoEventArgs re)
            {
                await SelectRepository(re.Owner, re.Name);
            }

            var repoWindow = new RepoWindow();
            repoWindow.Selected += OnSelected;

            repoWindow.Show();
        }

        public async Task SelectRepository(GitHubRepo repo, string branchName)
        {
            await RepositoryManager.InitializeRepositoryAsync(repo, branchName);
            RepositoryManager.LocalRepository = new GitRepo(RepositoryManager.RepositoryPath);

            xRepo.Content = $"{RepositoryManager.OwnerName}/\n{RepositoryManager.Repository.Name}";
            xBranch.Content = $"Current Branch\n'{repo.DefaultBranch}'";
        }

        public async Task SelectRepository(string owner, string name)
        {
            var repo = await RepositoryManager.GetRepositoryAsync(owner, name);
            await SelectRepository(repo, repo.DefaultBranch);
        }

        private void xClone_Click(object sender, RoutedEventArgs e)
        {
            Clone();
        }

        private void xAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void xCreate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void xInteraction_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
