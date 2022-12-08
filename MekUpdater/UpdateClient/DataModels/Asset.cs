using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MekUpdater.GithubClient.DataModels;

/// <summary>
/// Information about Github version release asset, e.g. binary
/// This does not include unnesessary information, for example info about uploader
/// </summary>
public class Asset
{
    /// <summary>
    /// Url where asset is situated in api
    /// </summary>
    [JsonPropertyName("url")]
    [JsonProperty("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Asset name (file name)
    /// </summary>
    [JsonPropertyName("name")]
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Type of asset content, application/anything
    /// </summary>
    [JsonPropertyName("content_type")]
    [JsonProperty("content_type")]
    public string? ContentType { get; set; }


    /// <summary>
    /// Asset size in bytes
    /// </summary>
    [JsonPropertyName("size")]
    [JsonProperty("size")]
    public long Size { get; set; } = default;

    /// <summary>
    /// Count of asset downloads 
    /// </summary>
    [JsonPropertyName("download_count")]
    [JsonProperty("download_count")]
    public int DownloadCount { get; set; } = default;

    /// <summary>
    /// Date time when asset was first uploaded to Github
    /// </summary>
    [JsonPropertyName("created_at")]
    [JsonProperty("created_at")]
    public DateTime? CreationTime { get; set; }


    /// <summary>
    /// Date time when asset was last time updated
    /// </summary>
    [JsonPropertyName("updated_at")]
    [JsonProperty("updated_at")]
    public DateTime? UpdatedTime { get; set; }

    /// <summary>
    /// Full url to download this specific asset
    /// </summary>
    [JsonPropertyName("browser_download_url")]
    [JsonProperty("browser_download_url")]
    public string? DownloadUrl { get; set; }

}


