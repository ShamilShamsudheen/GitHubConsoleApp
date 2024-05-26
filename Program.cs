using GitHubConsoleApp.interfaces;
using GitHubConsoleApp.models;
using GitHubConsoleApp.services;

class Program
{
    static async Task Main(string[] args)
    {
        // Read CSV file
        ICsvReader csvReader = new CsvReader();
        List<CsvData> csvDataList = csvReader.ReadCsv(@"C:\Users\mohamedshamil\source\repos\Internship Source.csv");

        // Initialize GitHub API
        IGitHubApi gitHubApi = new GitHubApi("token");
        GitHubDataService gitHubDataService = new GitHubDataService(gitHubApi);

        // Process each CSV data and collect results
        var gitHubUsers = new List<GitHubUser>();
        foreach (var csvData in csvDataList)
        {
            GitHubUser gitHubUser = await gitHubDataService.GetGitHubUserData(csvData);
            Console.WriteLine($"User: {gitHubUser.Username}, Total Projects: {gitHubUser.TotalProjectCount}, Total Non-Fork Commits: {gitHubUser.TotalNonForkCommitCount}, Last Year Commits: {gitHubUser.LastYearCommitsCount}, Total Commits: {gitHubUser.TotalCommitsCount}");
            gitHubUsers.Add(gitHubUser);
        }

        // Write results to CSV
        ICsvWriter csvWriter = new CsvWriter();
        string outputCsvFilePath = @"C:\Users\mohamedshamil\source\repos\GitHubUserDataOutput.csv";
        csvWriter.WriteCsv(outputCsvFilePath, gitHubUsers);

        Console.WriteLine($"Data saved to {outputCsvFilePath}");
    }
}