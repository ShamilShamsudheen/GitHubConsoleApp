using GitHubConsoleApp.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubConsoleApp.interfaces
{
    public interface ICsvWriter
    {
        void WriteCsv(string filePath, List<GitHubUser> records);
    }
}
