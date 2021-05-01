using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Octokit;

namespace ProjectGFN.Clients
{
    public class GitManager
    {
        public static GitHubClient Client { get; private set; } //GitHub API
        public static IReadOnlyList<Repository> Repositories { get; private set; }

        public static string UserName { get; private set; } = string.Empty;

        public static async Task Initialize(string token)
        {
            Client = new GitHubClient(new ProductHeaderValue("Git4Nextop"))
            {
                Credentials = new Credentials(token)
            };
            Repositories = await GetRepositoriesAsync();
            UserName = (await GetCurrentUserAsync()).Name;
        }

        public static async Task<User> GetCurrentUserAsync()
        {
            return await Client.User.Current();
        }

        public static async Task<IReadOnlyList<Repository>> GetRepositoriesAsync()
        {
            return await Client.Repository.GetAllForCurrent();
        }
    }
}