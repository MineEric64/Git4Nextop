using System.Threading.Tasks;
using Octokit;

using GitHubRepo = Octokit.Repository;

namespace ProjectGFN.Clients
{
    public class RepositoryManager
    {
        public static string OwnerName => Repository.Owner.Login;
        public static GitHubRepo Repository { get; private set; }

        public static async Task Initialize(string owner, string name)
        {
            if (!string.IsNullOrEmpty(owner) && !string.IsNullOrWhiteSpace(name))
            {
                GitHubRepo repo = await GitManager.Client.Repository.Get(owner, name);
                Initialize(repo);
            }
        }

        public static void Initialize(GitHubRepo repo)
        {
            Repository = repo;
        }
    }
}