using GitHubConsoleApp.interfaces;
using GitHubConsoleApp.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubConsoleApp.services
{
    public class GitHubDataService
    {
        private readonly IGitHubApi _gitHubApi;

        public GitHubDataService(IGitHubApi gitHubApi)
        {
            _gitHubApi = gitHubApi;
        }

        public async Task<GitHubUser> GetGitHubUserData(CsvData csvData)
        {
            string username = ExtractUsernameFromUrl(csvData.GitHubProfileLink);

            int totalProjectCount = (await _gitHubApi.GetProjectsAsync(username)).Count;
            int totalCommitsCount = await _gitHubApi.GetTotalCommitsAsync(username);
            int totalNonForkCommitCount = await _gitHubApi.GetNonForkCommitsAsync(username);
            int lastYearCommitsCount = await _gitHubApi.GetLastYearCommitsAsync(username);

            return new GitHubUser
            {
                Username = username,
                TotalProjectCount = totalProjectCount,
                TotalCommitsCount = totalCommitsCount,
                TotalNonForkCommitCount = totalNonForkCommitCount,
                LastYearCommitsCount = lastYearCommitsCount
            };
        }
        private string ExtractUsernameFromUrl(string url)
    {
        // Assuming the URL is in the form "https://github.com/username"
        return url.Split('/')[3];
    }
    }
}
