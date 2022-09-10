namespace MekUpdater.ActionResults;

/// <summary>
/// Update returned after check for updates
/// </summary>
public class UpdateCheckResult : UpdateResult
{
    internal UpdateCheckResult(bool success) : base(success) { }

    /// <summary>
    /// Url used to find repository from github api
    /// </summary>
    public string UsedUrl { get; set; } = string.Empty;

    /// <summary>
    /// Latest available RELEASED version in github
    /// </summary>
    public VersionTag? AvailableVersion { get; set; }

    /// <summary>
    /// Url to download update files
    /// </summary>
    public string? DownloadUrl { get; set; }
}
