using GitHubConsoleApp.interfaces;
using GitHubConsoleApp.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

public class GitHubApi : IGitHubApi
{
    private readonly HttpClient _httpClient;

    public GitHubApi(string token)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.github.com/")
        };
        _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));
        if (!string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<List<Project>> GetProjectsAsync(string username)
    {
        List<Project> allProjects = new List<Project>();
        string nextPageUrl = $"users/{username}/repos";

        try
        {
            do
            {
                var response = await _httpClient.GetAsync(nextPageUrl);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var projects = JsonSerializer.Deserialize<List<Project>>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (projects.Any())
                {
                    allProjects.AddRange(projects);
                }

                nextPageUrl = GetNextPageUrl(response.Headers);

            } while (!string.IsNullOrEmpty(nextPageUrl));
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching repositories for user '{username}': {ex.Message} and current count: {allProjects.Count}");
        }

        return allProjects;
    }

    private string GetNextPageUrl(HttpResponseHeaders headers)
    {
        if (headers.TryGetValues("Link", out IEnumerable<string> linkValues))
        {
            var linkHeader = linkValues.FirstOrDefault();
            if (linkHeader != null)
            {
                var links = linkHeader.Split(',');
                foreach (var link in links)
                {
                    var parts = link.Split(';');
                    if (parts.Length == 2 && parts[1].Trim() == "rel=\"next\"")
                    {
                        return parts[0].Trim('<', '>', ' ');
                    }
                }
            }
        }
        return null;
    }

    public async Task<int> GetTotalCommitsAsync(string username)
    {
        var projects = await GetProjectsAsync(username);
        int totalCommits = 0;

        foreach (var project in projects)
        {
            var commits = await GetCommitsAsync(username, project.Name);
            totalCommits += commits.Count;
        }

        return totalCommits;
    }

    public async Task<int> GetNonForkCommitsAsync(string username)
    {
        var projects = await GetProjectsAsync(username);
        int nonForkCommits = 0;

        foreach (var project in projects)
        {
            if (!project.Fork)
            {
                var commits = await GetCommitsAsync(username, project.Name);
                nonForkCommits += commits.Count;
            }
        }

        return nonForkCommits;
    }

    public async Task<int> GetLastYearCommitsAsync(string username)
    {
        var projects = await GetProjectsAsync(username);
        int lastYearCommits = 0;

        foreach (var project in projects)
        {
            var commits = await GetCommitsAsync(username, project.Name);
            foreach (var commit in commits)
            {
                if (commit.CommitDetails.Author.Date > DateTime.UtcNow.AddYears(-1))
                {
                    lastYearCommits++;
                }
            }
        }

        return lastYearCommits;
    }

    private async Task<List<Commit>> GetCommitsAsync(string username, string repo)
    {
        List<Commit> allCommits = new List<Commit>();
        string nextPageUrl = $"repos/{username}/{repo}/commits";

        try
        {
            do
            {
                var response = await _httpClient.GetAsync(nextPageUrl);

                if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    // Skip the repository if it's empty (409 Conflict)
                    //Console.WriteLine($"Repository '{repo}' is empty. Skipping...");
                    return allCommits;
                }

                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var commits = JsonSerializer.Deserialize<List<Commit>>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (commits.Any())
                {
                    allCommits.AddRange(commits);
                }

                nextPageUrl = GetNextPageUrl(response.Headers);

            } while (!string.IsNullOrEmpty(nextPageUrl));
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching commits for repository '{repo}' of user '{username}': {ex.Message}");
        }

        return allCommits;
    }

}
