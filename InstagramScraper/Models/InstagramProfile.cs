using System.Text.Json.Serialization;

namespace InstagramScraper.Models
{
    public class InstagramProfile
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        [JsonPropertyName("biography")]
        public string Biography { get; set; }

        [JsonPropertyName("profile_pic_url")]
        public string ProfilePicUrl { get; set; }

        [JsonPropertyName("follower_count")]
        public int FollowerCount { get; set; }

        [JsonPropertyName("following_count")]
        public int FollowingCount { get; set; }

        [JsonPropertyName("media_count")]
        public int MediaCount { get; set; }

        [JsonPropertyName("is_verified")]
        public bool IsVerified { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("public_email")]
        public string PublicEmail { get; set; }

        [JsonPropertyName("public_phone_number")]
        public string PublicPhoneNumber { get; set; }

        [JsonPropertyName("external_url")]
        public string ExternalUrl { get; set; }

        [JsonPropertyName("is_private")]
        public bool IsPrivate { get; set; }

        [JsonPropertyName("is_business")]
        public bool IsBusiness { get; set; }

        [JsonPropertyName("has_igtv")]
        public bool HasIGTV { get; set; }

        [JsonPropertyName("account_type")]
        public string AccountType { get; set; }

        [JsonPropertyName("total_igtv_videos")]
        public int TotalIgtvVideos { get; set; }

        [JsonPropertyName("latest_reel_media")]
        public long LatestReelMedia { get; set; }

        [JsonPropertyName("has_guides")]
        public bool HasGuides { get; set; }

        [JsonPropertyName("category_id")]
        public int CategoryId { get; set; }

        [JsonPropertyName("business_contact_method")]
        public string BusinessContactMethod { get; set; }

        [JsonPropertyName("can_hide_category")]
        public bool CanHideCategory { get; set; }

        [JsonPropertyName("can_hide_public_contacts")]
        public bool CanHidePublicContacts { get; set; }

        [JsonPropertyName("direct_messaging")]
        public string DirectMessaging { get; set; }

        [JsonPropertyName("show_shoppable_feed")]
        public bool ShowShoppableFeed { get; set; }

        [JsonPropertyName("third_party_downloads_enabled")]
        public int ThirdPartyDownloadsEnabled { get; set; }

        [JsonPropertyName("bio_links")]
        public List<BioLink> BioLinks { get; set; }

        [JsonPropertyName("location_data")]
        public LocationData LocationData { get; set; }
    }

    public class BioLink
    {
        [JsonPropertyName("icon_url")]
        public string IconUrl { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("link_type")]
        public string LinkType { get; set; }

        [JsonPropertyName("is_pinned")]
        public bool IsPinned { get; set; }

        [JsonPropertyName("is_verified")]
        public bool IsVerified { get; set; }
    }

    public class LocationData
    {
        [JsonPropertyName("address_street")]
        public string? AddressStreet { get; set; }

        [JsonPropertyName("city_name")]
        public string? CityName { get; set; }

        [JsonPropertyName("zip")]
        public string? Zip { get; set; }

        [JsonPropertyName("instagram_location_id")]
        public long? InstagramLocationId { get; set; }
    }
}