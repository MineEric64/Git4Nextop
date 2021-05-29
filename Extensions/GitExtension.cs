using System.Threading.Tasks;
using System.Windows;
using ProjectGFN.Clients;

using GitHubRepo = Octokit.Repository;
using Credentials = Octokit.Credentials;

using GitRepo = LibGit2Sharp.Repository;
using TokenCredentials = LibGit2Sharp.UsernamePasswordCredentials;

namespace ProjectGFN.Extensions
{
    public static class GitExtension
    {
        public static void Clone(this GitHubRepo repo, string branchName, string path)
        {
            GitManager.Clone(repo.CloneUrl, branchName, path);
        }

        public static async Task CloneAsync(this GitHubRepo repo, string branchName, string path)
        {
            await Task.Run(() => GitManager.Clone(repo.CloneUrl, branchName, path));
        }
    }
}