using System.Text.Json;

namespace REST_API_CV_hantering.Endpoints.GithubEndpoints
{
    public class GithubEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            app.MapGet("/api/github/{username}/repos", async (string username) =>
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
                var response = await client.GetAsync($"https://api.github.com/users/{username}/repos");
                if (!response.IsSuccessStatusCode)
                {
                    return Results.StatusCode((int)response.StatusCode);
                }
                var content = await response.Content.ReadAsStringAsync();
                var repos = JsonSerializer.Deserialize<List<Repo>>(content) ?? new List<Repo>();
                var result = repos.Select(repo => new { repo.name, language = repo.language ?? "okänt", description = repo.description ?? "saknas", repo.html_url }).ToList();
                return Results.Ok(result);
            });
        }
        private class Repo
        {
            public string name { get; set; } = string.Empty;
            public string language { get; set; } = string.Empty;
            public string description { get; set; } = string.Empty;
            public string html_url { get; set; } = string.Empty;
        }
    }
}
