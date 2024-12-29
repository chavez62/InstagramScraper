using InstagramScraper.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace InstagramScraper.Service
{
    public class InstagramService
    {
        private readonly HttpClient _httpClient;
        private readonly InstagramApiSettings _settings;
        private readonly ILogger<InstagramService> _logger;

        public InstagramService(
            IHttpClientFactory httpClientFactory,
            IOptions<InstagramApiSettings> settings,
            ILogger<InstagramService> logger)
        {
            _settings = settings.Value;
            _httpClient = httpClientFactory.CreateClient("InstagramAPI");
            _logger = logger;

            // Configure HttpClient
            _httpClient.BaseAddress = new Uri($"https://{_settings.ApiHost}/");
            _httpClient.DefaultRequestHeaders.Clear();
            
            // Add RapidAPI headers
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", _settings.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", _settings.ApiHost);
            
            // Add CORS headers
            _httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
            _httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            _httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Headers", "Content-Type, X-RapidAPI-Key, X-RapidAPI-Host");
        }

        public async Task<InstagramProfile> GetProfileAsync(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    throw new ArgumentException("Username cannot be empty", nameof(username));
                }

                // Create request message to add additional headers if needed
                var request = new HttpRequestMessage(HttpMethod.Get, $"v1/info?username_or_id_or_url={Uri.EscapeDataString(username)}");
                
                // Add any request-specific headers here if needed
                request.Headers.Add("Accept", "application/json");

                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Response Status: {StatusCode}", response.StatusCode);
                _logger.LogInformation("Response Headers: {Headers}", 
                    string.Join(", ", response.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}")));

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Request failed. Status: {StatusCode}, Content: {Content}", 
                        response.StatusCode, responseContent);
                    
                    throw new HttpRequestException($"API request failed with status code: {response.StatusCode}");
                }

                var jsonDocument = JsonDocument.Parse(responseContent);
                var data = jsonDocument.RootElement.GetProperty("data");

                // Rest of your code remains the same...
                return new InstagramProfile
                {
                    // Your existing profile mapping
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Instagram profile for {Username}", username);
                throw;
            }
        }
    }
}
