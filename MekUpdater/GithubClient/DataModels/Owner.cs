using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MekUpdater.GithubClient.DataModels;

/// <summary>
/// Content model for gihtub repository owner
/// </summary>
public class Owner
{
    /// <summary>
    /// Repository owner user name
    /// </summary>
    [JsonProperty("login")]
    [JsonPropertyName("login")]
    public string? UserName { get; set; }

    /// <summary>
    /// User Id
    /// </summary>
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Link to user avatar image
    /// </summary>
    [JsonProperty("avatar_url")]
    [JsonPropertyName("avatar_url")]
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// Link to user profile
    /// </summary>
    [JsonProperty("url")]
    [JsonPropertyName("url")]
    public string? ProfileUrl { get; set; }

    /// <summary>
    /// Link to user profile html page (Visual page )
    /// </summary>
    [JsonProperty("html_url")]
    [JsonPropertyName("html_url")]
    public string? VisualProfileUrl { get; set; }

    /// <summary>
    /// Link to people following owner
    /// </summary>
    [JsonProperty("followers_url")]
    [JsonPropertyName("followers_url")]
    public string? FollowersUrl { get; set; }

    /// <summary>
    /// Link to people owner is following
    /// </summary>
    [JsonProperty("following_url")]
    [JsonPropertyName("following_url")]
    public string? FollowingUrl { get; set; }

    /// <summary>
    /// Link to owner's gists
    /// </summary>
    [JsonProperty("gists_url")]
    [JsonPropertyName("gists_url")]
    public string? GistsUrl { get; set; }

    /// <summary>
    /// Link to starred repositories
    /// </summary>
    [JsonProperty("starred_url")]
    [JsonPropertyName("starred_url")]
    public string? StarredUrl { get; set; }


    /// <summary>
    /// Link to starred repositories
    /// </summary>
    [JsonProperty("subscriptions_url")]
    [JsonPropertyName("subscriptions_url")]
    public string? PinnedUrl { get; set; }

    /// <summary>
    /// Link to owner's organizations
    /// </summary>
    [JsonProperty("organizations_url")]
    [JsonPropertyName("organizations_url")]
    public string? OrganizationsUrl { get; set; }

    /// <summary>
    /// Link to owner's repositories
    /// </summary>
    [JsonProperty("repos_url")]
    [JsonPropertyName("repos_url")]
    public string? ReposUrl { get; set; }

    /// <summary>
    /// Link to owner's actions in github (pushes, merges)
    /// </summary>
    [JsonProperty("events_url")]
    [JsonPropertyName("events_url")]
    public string? EventsUrl { get; set; }

    /// <summary>
    /// Link to watch events from other users
    /// </summary>
    [JsonProperty("received_events_url")]
    [JsonPropertyName("received_events_url")]
    public string? ReceivedEventsUrl { get; set; }

    /// <summary>
    /// User type
    /// </summary>
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public string? AccountType { get; set; }

    /// <summary>
    /// Is site admin
    /// </summary>
    [JsonProperty("site_admin")]
    [JsonPropertyName("site_admin")]
    public bool IsSiteAdmin { get; set; }
}
