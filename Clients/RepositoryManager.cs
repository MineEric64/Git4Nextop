using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

using GitRepo = LibGit2Sharp.Repository;
using GitHubRepo = Octokit.Repository;

namespace ProjectGFN.Clients
{
    public class RepositoryManager
    {
        public static string OwnerName => Repository.Owner.Login;

        public static GitHubRepo Repository { get; private set; }
        public static GitRepo LocalRepository { get; set; }

        public static string BranchDefaultName { get; private set; } = string.Empty;
        public static Branch BranchDefault { get; private set; }
        public static Branch[] Branches { get; private set; }

        public static string RepositoryPath { get; set; } = string.Empty;

        public static async Task<GitHubRepo> GetRepositoryAsync(string owner, string name)
        {
            if (!string.IsNullOrEmpty(owner) && !string.IsNullOrWhiteSpace(name))
            {
                GitHubRepo repo = await GitManager.Client.Repository.Get(owner, name);
                return repo;
            }

            return null;
        }

        public static async Task InitializeRepositoryAsync(GitHubRepo repo, string branchName)
        {
            Repository = repo;

            Branches = await GetBranchesAsync();
            BranchDefault = GetBranch(branchName);
            BranchDefaultName = branchName;
        }

        public static async Task<Branch[]> GetBranchesAsync()
        {
            return await GetBranchesAsync(Repository);
        }

        public static async Task<Branch[]> GetBranchesAsync(GitHubRepo repo)
        {
            var branches = await GitManager.Client.Repository.Branch.GetAll(repo.Id);
            return branches.ToArray();
        }

        public static Branch GetBranch()
        {
            return GetBranch(BranchDefaultName);
        }

        public static Branch GetBranch(string name)
        {
            foreach (var branch in Branches)
            {
                if (branch.Name == name)
                {
                    return branch;
                }
            }

            return null;
        }
    }
}