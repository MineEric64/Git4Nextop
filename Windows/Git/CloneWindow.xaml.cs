using System;
using System.IO;
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

using Octokit;
using Ookii.Dialogs.Wpf;
using ProjectGFN.Clients;

using Path = System.IO.Path;
using GitHubRepo = Octokit.Repository;

namespace ProjectGFN.Windows.Git
{
    /// <summary>
    /// Interaction logic for CloneWindow.xaml
    /// </summary>
    public partial class CloneWindow : Window
    {
        public GitHubRepo Repository { get; set; }
        public List<Branch> Branches = new List<Branch>();

        public event EventHandler<CloneEventArgs> Selected;
        public bool AutoClose { get; set; } = true;

        public CloneWindow()
        {
            InitializeComponent();

            this.Loaded += CloneWindow_Loaded;
        }

        public async Task InitializeAsync(GitHubRepo repo)
        {
            Repository = repo;

            Branch[] branches = await RepositoryManager.GetBranchesAsync(repo);
            Branches = new List<Branch>(branches);
        }

        public async void CloneWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Path
            if (!string.IsNullOrWhiteSpace(RepositoryManager.RepositoryPath))
            {
                var prevDirectory = Directory.GetParent(RepositoryManager.RepositoryPath).FullName;
                xPath.Text = $@"{prevDirectory}\{Repository.Name}";
            }

            //Branches
            var gridView = new GridView();
            xBranches.View = gridView;

            gridView.Columns.Add(new GridViewColumn
                {Header = "Branch Name", DisplayMemberBinding = new Binding("BranchName")});
            gridView.Columns.Add(new GridViewColumn
                { Header = "Last Commit Date", DisplayMemberBinding = new Binding("LastCommitDate") });
            gridView.Columns.Add(new GridViewColumn
                { Header = "Default", DisplayMemberBinding = new Binding("Default") });

            foreach (var branch in Branches)
            {
                var listViewItem = new ListViewItem();
                var commit = await GitManager.Client.Repository.Commit.Get(Repository.Id, branch.Commit.Sha);
                bool isDefault = branch.Name == Repository.DefaultBranch;
                var cloneItem = new CloneItem(branch.Name, commit.Commit.Author.Date.DateTime, isDefault);

                listViewItem.Content = cloneItem;
                xBranches.Items.Add(listViewItem);

                if (isDefault)
                {
                    xBranches.SelectedItem = listViewItem;
                }
            }
        }

        private void xOk_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(xPath.Text) && xBranches.SelectedItem != null && xBranches.SelectedItem is ListViewItem listViewItem)
            {
                var cloneItem = listViewItem.Content as CloneItem;
                var branchName = cloneItem?.BranchName;

                Selected?.Invoke(this, new CloneEventArgs(xPath.Text, branchName));

                if (AutoClose)
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("You didn't select any repository.", MainWindow.MainTitle, MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void xPathOk_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog();

            if (dialog.ShowDialog(this).GetValueOrDefault())
            {
                xPath.Text = $@"{dialog.SelectedPath}\{Repository.Name}";
            }
        }
    }

    public class CloneItem
    {
        public string BranchName { get; set; }
        public string LastCommitDate { get; set; }
        public string Default { get; set; }

        public CloneItem(string branchName, DateTime date, bool isDefault)
        {
            BranchName = branchName;
            LastCommitDate = date.ToString();
            Default = isDefault ? "*" : string.Empty;
        }
    }

    public class CloneEventArgs
    {
        public string Path { get; set; }
        public string BranchName { get; set; }

        public CloneEventArgs(string path, string branchName)
        {
            Path = path;
            BranchName = branchName;
        }
    }
}
