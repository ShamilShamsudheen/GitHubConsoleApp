using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubConsoleApp.models
{
    using System;
    using System.Text.Json.Serialization;

    public class Commit
    {
        [JsonPropertyName("commit")]
        public CommitDetails CommitDetails { get; set; }
    }

    public class CommitDetails
    {
        [JsonPropertyName("author")]
        public CommitAuthor Author { get; set; }
    }

    public class CommitAuthor
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }
}
