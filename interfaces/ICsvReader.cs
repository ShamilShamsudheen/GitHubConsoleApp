using GitHubConsoleApp.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubConsoleApp.interfaces
{
    public interface ICsvReader
    {
        List<CsvData> ReadCsv(string filePath);

    }
}
