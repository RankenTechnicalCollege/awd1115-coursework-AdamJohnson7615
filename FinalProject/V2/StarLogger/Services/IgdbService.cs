using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StarLogger.ViewModels;
using System.Text;

namespace StarLogger.Services
{
    public interface IIgdbService
    {
        Task<List<GameResult>> SearchGamesAsync(string query);
    }

    public class IgdbService : IIgdbService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private static string _accessToken;
        private static DateTime _tokenExpiry;

        public IgdbService(IConfiguration config)
        {
            _config = config;
            _httpClient = new HttpClient();
        }

        public async Task<List<GameResult>> SearchGamesAsync(string query)
        {
            await EnsureAccessTokenAsync();

            var clientId = _config["IGDB:ClientId"];

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.igdb.com/v4/games");
            request.Headers.Add("Client-ID", clientId);
            request.Headers.Add("Authorization", $"Bearer {_accessToken}");

            string queryBody = $"search \"{query}\"; fields name, cover.url, platforms.name; limit 20;";
            request.Content = new StringContent(queryBody, Encoding.UTF8, "text/plain");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return new List<GameResult>();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(json);

            var results = new List<GameResult>();

            if (data != null)
            {
                foreach (var item in data)
                {
                    string coverUrl = "https://placehold.co/100x140?text=No+Image";
                    if (item.cover != null && item.cover.url != null)
                    {
                        string rawUrl = item.cover.url.ToString();
                        coverUrl = "https:" + rawUrl.Replace("t_thumb", "t_cover_big");
                    }

                    string platformStr = "Unknown";
                    if (item.platforms != null)
                    {
                        var pList = new List<string>();
                        foreach (var p in item.platforms)
                        {
                            pList.Add(p.name.ToString());
                        }
                        platformStr = string.Join(", ", pList.Take(2));
                    }
                    results.Add(new GameResult
                    {
                        Title = item.name,
                        IgdbId = item.id,
                        Platform = platformStr,
                        CoverUrl = coverUrl
                    });
                }
            }

            return results;
        }
        private async Task EnsureAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(_accessToken) && DateTime.Now < _tokenExpiry) return;

            var clientId = _config["IGDB:ClientId"];
            var clientSecret = _config["IGDB:ClientSecret"];

            var url = $"https://id.twitch.tv/oauth2/token?client_id={clientId}&client_secret={clientSecret}&grant_type=client_credentials";

            var response = await _httpClient.PostAsync(url, null);
            var json = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(json);

            _accessToken = data["access_token"].ToString();
            _tokenExpiry = DateTime.Now.AddHours(1);
        }
    }
}