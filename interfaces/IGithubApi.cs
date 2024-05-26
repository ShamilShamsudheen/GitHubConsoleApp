using GitHubConsoleApp.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubConsoleApp.interfaces
{
    public interface IGitHubApi
    {
        
        Task<List<Project>> GetProjectsAsync(string username);
        Task<int> GetTotalCommitsAsync(string username);
        Task<int> GetNonForkCommitsAsync(string username);
        Task<int> GetLastYearCommitsAsync(string username);
    }
}
