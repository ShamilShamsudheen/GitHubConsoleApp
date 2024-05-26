using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubConsoleApp.models
{
    public class GitHubUser
    {
        public string Username { get; set; }
        public int TotalProjectCount { get; set; }
        public int TotalNonForkCommitCount { get; set; }
        public int LastYearCommitsCount { get; set; }
        public int TotalCommitsCount { get; set; }
    }
}
