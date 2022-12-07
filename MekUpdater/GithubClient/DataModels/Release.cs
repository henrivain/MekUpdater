using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MekUpdater.GithubClient.DataModel;

/// <summary>
/// Paresed result from github api request
/// </summary>
public class Release
{
    /// <summary>
    /// Url to release
    /// </summary>
    [JsonProperty("url")]
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Url to access assets array
    /// </summary>
    [JsonProperty("assets_url")]
    [JsonPropertyName("assets_url")]
    public string? AssetsUrl { get; set; }

    /// <summary>
    /// Url to access release page
    /// </summary>
    [JsonProperty("html_url")]
    [JsonPropertyName("html_url")]
    public string? PageUrl { get; set; }

    /// <summary>
    /// Release tag e.g. v1.2.0
    /// </summary>
    [JsonProperty("tag_name")]
    [JsonPropertyName("tag_name")]
    public string? TagName { get; set; }

    /// <summary>
    /// Branch where version was released from
    /// </summary>
    [JsonProperty("target_commitish")]
    [JsonPropertyName("target_commitish")]
    public string? ReleaseBranch { get; set; }

    /// <summary>
    /// Branch where version was released from
    /// </summary>
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string? ReleaseName { get; set; }

    /// <summary>
    /// Date time when release was first created in Github
    /// </summary>
    [JsonPropertyName("created_at")]
    [JsonProperty("created_at")]
    public DateTime? CreationTime { get; set; }

    /// <summary>
    /// Date time when release was published in github
    /// </summary>
    [JsonPropertyName("published_at")]
    [JsonProperty("published_at")]
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// Url to download source code as .tar 
    /// </summary>
    [JsonPropertyName("tarball_url")]
    [JsonProperty("tarball_url")]
    public string? TarUrl { get; set; }

    /// <summary>
    /// Url to download source code as .zip 
    /// </summary>
    [JsonPropertyName("zipball_url")]
    [JsonProperty("zipball_url")]
    public string? ZipUrl { get; set; }

    /// <summary>
    /// Whats new text part as strin
    /// </summary>
    [JsonPropertyName("body")]
    [JsonProperty("body")]
    public string? WhatsNew { get; set; }

    /// <summary>
    /// Array of release assets, e.g. binaries
    /// </summary>
    [JsonProperty("assets")]
    [JsonPropertyName("assets")]
    public Asset[] Assets { get; set; } = Array.Empty<Asset>();
}
