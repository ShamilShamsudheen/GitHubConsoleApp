using CsvHelper.Configuration;
using GitHubConsoleApp.interfaces;
using GitHubConsoleApp.models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubConsoleApp.services
{
    public class CsvWriter : ICsvWriter
    {
        public void WriteCsv(string filePath, List<GitHubUser> records)
        {
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvHelper.CsvWriter(writer, config))
            {
                csv.WriteRecords(records);
            }
        }
    }
}
