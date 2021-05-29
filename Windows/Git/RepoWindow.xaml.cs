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

namespace ProjectGFN.Windows.Git
{
    /// <summary>
    /// Interaction logic for RepoWindow.xaml
    /// </summary>
    public partial class RepoWindow : Window
    {
        public static Dictionary<string, List<string>> RepoMap = new Dictionary<string, List<string>>();

        public event EventHandler<RepoEventArgs> Selected;
        public bool AutoClose { get; set; } = true;

        public RepoWindow()
        {
            InitializeComponent();

            this.Loaded += RepoWindow_Loaded;
        }

        public static void Initialize(Dictionary<string, List<string>> repoMap)
        {
            RepoMap = repoMap;
        }

        public void RepoWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var key in RepoMap.Keys)
            {
                var parent = new TreeViewItem
                {
                    Header = key
                };

                foreach (var repoName in RepoMap[key])
                {
                    var item = new TreeViewItem
                    {
                        Header = repoName,
                        Tag = key
                    };

                    parent.Items.Add(item);
                }

                xRepos.Items.Add(parent);
            }
        }

        private void xOk_Click(object sender, RoutedEventArgs e)
        {
            if (xRepos.SelectedItem != null && xRepos.SelectedItem is TreeViewItem item)
            {
                string owner = item.Tag as string;
                string name = item.Header as string;

                Selected?.Invoke(this, new RepoEventArgs(owner, name));

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
    }

    public class RepoEventArgs
    {
        public string Owner { get; set; }
        public string Name { get; set; }

        public RepoEventArgs(string owner, string name)
        {
            Owner = owner;
            Name = name;
        }
    }
}
