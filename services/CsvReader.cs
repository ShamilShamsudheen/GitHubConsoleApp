using GitHubConsoleApp.interfaces;
using GitHubConsoleApp.models;
using System.Globalization;
using CsvHelper;
using System.Collections.Generic;
using System.IO;
using CsvHelper.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace GitHubConsoleApp.services
{
    public class CsvReader : ICsvReader
    {
        public bool IsValidGitHubUrl(string url)
        {
            //define the pattern for a valid github profileUrl
            string pattern = @"^https:\/\/github\.com\/[a-zA-Z0-9-]+$";
            return Regex.IsMatch(url, pattern) && !string.IsNullOrWhiteSpace(url);
        }
        public List<CsvData> ReadCsv(string filePath)
        {
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
            };
            using (var reader = new StreamReader(filePath))
            {
                using (var csv = new CsvHelper.CsvReader(reader, (IReaderConfiguration)config))
                {
                    var records = csv.GetRecords<CsvData>().ToList();
                    // filter the valid Github profile urls
                    var validRecords = records.Where(record => IsValidGitHubUrl(record.GitHubProfileLink)).ToList();
                    return validRecords;
                }
            }

        }
    }

}
