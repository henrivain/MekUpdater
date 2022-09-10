namespace MekUpdater.ActionResults;

/// <summary>
/// Update result returned after update files are downloaded from github api
/// </summary>
public class DownloadUpdateFilesResult : GetSetupResult
{
    internal DownloadUpdateFilesResult(bool success) : base(success) { }
}
