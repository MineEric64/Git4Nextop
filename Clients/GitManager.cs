using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Octokit;
using LibGit2Sharp;

using ProjectGFN.Others;

using GitHubRepo = Octokit.Repository;
using Credentials = Octokit.Credentials;

using GitRepo = LibGit2Sharp.Repository;
using TokenCredentials = LibGit2Sharp.UsernamePasswordCredentials;

using Network = ProjectGFN.Others.Network;

namespace ProjectGFN.Clients
{
    public class GitManager
    {
        public static GitHubClient Client { get; private set; } //GitHub API
        public static IReadOnlyList<GitHubRepo> Repositories { get; private set; }

        public static string UserName { get; private set; } = string.Empty;
        public static TokenCredentials Token => new TokenCredentials();

        public static async Task<bool> Initialize(string token)
        {
            try
            {
                if (!Network.IsAvailable)
                {
                    return false;
                }

                Client = new GitHubClient(new ProductHeaderValue("Git4Nextop"))
                {
                    Credentials = new Credentials(token)
                };
                Repositories = await GetRepositoriesAsync();
                UserName = (await GetCurrentUserAsync()).Name;

                Token.Username = UserName;
                Token.Password = token;
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static async Task<User> GetCurrentUserAsync()
        {
            return await Client.User.Current();
        }

        public static async Task<IReadOnlyList<GitHubRepo>> GetRepositoriesAsync()
        {
            return await Client.Repository.GetAllForCurrent();
        }

        public static void Clone(string url, string branchName, string path)
        {
            CloneOptions option = new CloneOptions
            {
                CredentialsProvider = (_url, _user, _cred) => Token,
                BranchName = branchName
            };

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            GitRepo.Clone(url, path, option);
        }

        public static async Task CloneAsync(string url, string branchName, string path)
        {
            await Task.Run(() => Clone(url, branchName, path));
        }
    }
}