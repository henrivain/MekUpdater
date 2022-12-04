using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MekUpdater.GithubClient.DataModel;

/// <summary>
/// Data structure for github api response for repository Licence
/// </summary>
public class Licence
{
    /// <summary>
    /// Short version of Licence name
    /// </summary>
    [JsonProperty("key")]
    [JsonPropertyName("key")]
    public string? ShortName { get; set; }

    /// <summary>
    /// Human friendly licence name
    /// </summary>
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Licence id (name)
    /// </summary>
    [JsonProperty("spdx_id")]
    [JsonPropertyName("spdx_id")]
    public string? LisenceId { get; set; }

    /// <summary>
    /// Url to licence
    /// </summary>
    [JsonProperty("url")]
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Node id for licence
    /// </summary>
    [JsonProperty("node_id")]
    [JsonPropertyName("node_id")]
    public string? NodeId { get; set; }
}
