namespace MekUpdater.ActionResults;

public class UpdateCheckResult : UpdateResult
{
    public UpdateCheckResult(bool success) : base(success) { }
    public string UsedUrl { get; set; } = string.Empty;
    public VersionTag? AvailableVersion { get; set; }
    public string? DownloadUrl { get; set; }
}
