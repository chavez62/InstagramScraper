using InstagramScraper.Models;
using Microsoft.Extensions.Options;
using System.Numerics;
using System.Text.Json;

namespace InstagramScraper.Service
{
    public class InstagramService
    {
        private readonly HttpClient _httpClient;
        private readonly InstagramApiSettings _settings;

        public InstagramService(IOptions<InstagramApiSettings> settings)
        {
            _settings = settings.Value;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", _settings.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", _settings.ApiHost);
        }

        public async Task<InstagramProfile> GetProfileAsync(string username)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://{_settings.ApiHost}/v1/info?username_or_id_or_url={username}");
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(jsonString);
                var data = jsonDocument.RootElement.GetProperty("data");

                // Helper function to safely get string from either string or number
                string GetStringValue(JsonElement element)
                {
                    return element.ValueKind switch
                    {
                        JsonValueKind.String => element.GetString() ?? "",
                        JsonValueKind.Number => element.ToString(),
                        _ => ""
                    };
                }

                // Helper function to safely get int from nullable number
                int GetIntValue(JsonElement element, int defaultValue = 0)
                {
                    return element.ValueKind switch
                    {
                        JsonValueKind.Number => element.GetInt32(),
                        _ => defaultValue
                    };
                }

                // Helper function to safely get long from nullable number
                long GetLongValue(JsonElement element, long defaultValue = 0)
                {
                    return element.ValueKind switch
                    {
                        JsonValueKind.Number => element.GetInt64(),
                        _ => defaultValue
                    };
                }

                var profilePicUrl = data.GetProperty("hd_profile_pic_url_info")
                                       .GetProperty("url")
                                       .GetString() ?? "";

                return new InstagramProfile
                {
                    Username = data.GetProperty("username").GetString() ?? "",
                    FullName = data.GetProperty("full_name").GetString() ?? "",
                    Biography = data.GetProperty("biography").GetString() ?? "",
                    ProfilePicUrl = profilePicUrl,
                    FollowerCount = data.TryGetProperty("follower_count", out var followerCount) ? GetIntValue(followerCount) : 0,
                    FollowingCount = data.TryGetProperty("following_count", out var followingCount) ? GetIntValue(followingCount) : 0,
                    MediaCount = data.TryGetProperty("media_count", out var mediaCount) ? GetIntValue(mediaCount) : 0,
                    IsVerified = data.GetProperty("is_verified").GetBoolean(),
                    IsPrivate = data.GetProperty("is_private").GetBoolean(),
                    Category = data.TryGetProperty("category", out var category) ? category.GetString() : "",
                    PublicEmail = data.TryGetProperty("public_email", out var email) ? email.GetString() : "",
                    PublicPhoneNumber = data.TryGetProperty("public_phone_number", out var phone) ? phone.GetString() : "",
                    ExternalUrl = data.TryGetProperty("external_url", out var url) ? url.GetString() : "",
                    IsBusiness = data.TryGetProperty("is_business", out var isBusiness) ? isBusiness.GetBoolean() : false,
                    HasIGTV = data.TryGetProperty("has_igtv", out var hasIgtv) ? hasIgtv.GetBoolean() : false,
                    AccountType = data.TryGetProperty("account_type", out var accountType) ? GetStringValue(accountType) : "",
                    TotalIgtvVideos = data.TryGetProperty("total_igtv_videos", out var igtvVideos) ? GetIntValue(igtvVideos) : 0,
                    LatestReelMedia = data.TryGetProperty("latest_reel_media", out var reelMedia) ? GetLongValue(reelMedia) : 0,
                    HasGuides = data.TryGetProperty("has_guides", out var hasGuides) ? hasGuides.GetBoolean() : false,
                    CategoryId = data.TryGetProperty("category_id", out var categoryId) ? GetIntValue(categoryId) : 0,
                    BusinessContactMethod = data.TryGetProperty("business_contact_method", out var contactMethod) ? contactMethod.GetString() : "",
                    CanHideCategory = data.TryGetProperty("can_hide_category", out var canHideCategory) ? canHideCategory.GetBoolean() : false,
                    CanHidePublicContacts = data.TryGetProperty("can_hide_public_contacts", out var canHideContacts) ? canHideContacts.GetBoolean() : false,
                    DirectMessaging = data.TryGetProperty("direct_messaging", out var directMessaging) ? directMessaging.GetString() : "",
                    ShowShoppableFeed = data.TryGetProperty("show_shoppable_feed", out var shoppableFeed) ? shoppableFeed.GetBoolean() : false,
                    ThirdPartyDownloadsEnabled = data.TryGetProperty("third_party_downloads_enabled", out var downloads) ? GetIntValue(downloads) : 0,
                    BioLinks = data.TryGetProperty("bio_links", out var bioLinks) && bioLinks.ValueKind != JsonValueKind.Null
                        ? JsonSerializer.Deserialize<List<BioLink>>(bioLinks.GetRawText())
                        : new List<BioLink>(),
                    LocationData = data.TryGetProperty("location_data", out var locationData) && locationData.ValueKind != JsonValueKind.Null
                        ? new LocationData
    {
                        AddressStreet = locationData.TryGetProperty("address_street", out var street) ? street.GetString() : null,
                        CityName = locationData.TryGetProperty("city_name", out var city) ? city.GetString() : null,
                        Zip = locationData.TryGetProperty("zip", out var zip) ? zip.GetString() : null,
                        InstagramLocationId = locationData.TryGetProperty("instagram_location_id", out var locId) ?
                            (locId.ValueKind == JsonValueKind.Number ? locId.GetInt64() : null) : null
                        }
                        : null
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Full exception details: {ex}");
                throw new Exception($"Error fetching Instagram profile: {ex.Message}");
            }
        }
    }
}