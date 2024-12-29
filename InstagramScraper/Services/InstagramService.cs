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
            
            // Log configuration details
            _logger.LogInformation("Initializing InstagramService with API Host: {ApiHost}", _settings.ApiHost);
            _logger.LogInformation("API Key present: {HasApiKey}", !string.IsNullOrEmpty(_settings.ApiKey));
            
            // Validate API settings
            if (string.IsNullOrEmpty(_settings.ApiHost))
                throw new ArgumentException("API Host cannot be empty", nameof(_settings.ApiHost));
            if (string.IsNullOrEmpty(_settings.ApiKey))
                throw new ArgumentException("API Key cannot be empty", nameof(_settings.ApiKey));

            // Set base address and headers
            _httpClient.BaseAddress = new Uri($"https://{_settings.ApiHost}/");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", _settings.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", _settings.ApiHost);

            // Log headers
            LogHeaders();
        }

        private void LogHeaders()
        {
            foreach (var header in _httpClient.DefaultRequestHeaders)
            {
                _logger.LogInformation("Header: {Key} = {Value}", 
                    header.Key, 
                    header.Key.StartsWith("X-RapidAPI-Key") ? "[MASKED]" : string.Join(", ", header.Value));
            }
        }

        public async Task<InstagramProfile> GetProfileAsync(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    throw new ArgumentException("Username cannot be empty", nameof(username));
                }

                // Log request details
                var requestUrl = $"v1/info?username_or_id_or_url={Uri.EscapeDataString(username)}";
                _logger.LogInformation("Making request to: {BaseAddress}{RequestUrl}", _httpClient.BaseAddress, requestUrl);

                // Log current headers again before request
                LogHeaders();

                var response = await _httpClient.GetAsync(requestUrl);
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

                // Rest of the implementation remains the same...
                var jsonDocument = JsonDocument.Parse(responseContent);
                var data = jsonDocument.RootElement.GetProperty("data");
                
                // Your existing profile mapping code...
                return new InstagramProfile
                {
                    // ... existing properties
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
