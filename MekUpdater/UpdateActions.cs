

using MekUpdater.CheckUpdates;
using MekUpdater.UpdateRunner;

namespace MekUpdater;
/// <summary>
/// Easy way to use parts of the updater
/// </summary>
public sealed class UpdateActions
{
    /// <summary>
    /// Make request to github api and get repository info
    /// </summary>
    /// <param name="repoOwner"></param>
    /// <param name="repoName"></param>
    /// <returns>value representing process success</returns>
    public static async Task<UpdateCheckResult> CheckForUpdates(string repoOwner, string repoName)
    {
        if (string.IsNullOrWhiteSpace(repoOwner))
        {
            return new(false)
            {
                Message = $"Parameter '{nameof(repoOwner)}' was null or empty",
                AvailableVersion = null,
                DownloadUrl = null,
                UpdateMsg = UpdateMsg.ParameterNullOrEmpty,
                UsedUrl = string.Empty
            };
        }
        if (string.IsNullOrWhiteSpace(repoName))
        {
            return new(false)
            {
                Message = $"Parameter '{nameof(repoName)}' was null or empty",
                AvailableVersion = null,
                DownloadUrl = null,
                UpdateMsg = UpdateMsg.ParameterNullOrEmpty,
                UsedUrl = string.Empty
            };
        }

        var url = $"https://api.github.com/repos/{repoOwner}/{repoName}/releases/latest";

        var client = new GithubApiClient(url);
        return await client.GetVersionData();
    }


    /// <summary>
    /// Download zip file from github api async
    /// </summary>
    /// <param name="downloadUrl"></param>
    /// <param name="zipPath"></param>
    /// <exception cref="ArgumentException">thrown if download url is empty or null, or if zip path does not have value</exception>
    /// <returns>value representing process success</returns>
    public static async Task<DownloadUpdateFilesResult> DownloadZip(string downloadUrl, ZipPath zipPath)
    {
        var downloader = new GithubZipDownloader(downloadUrl, zipPath);
        return await downloader.DownloadAsync();
    }

    /// <summary>
    /// Extract given zip file into given folder async
    /// </summary>
    /// <param name="zipPath"></param>
    /// <param name="extractPath"></param>
    /// <returns>value representing process success</returns>
    public static async Task<ZipExtractionResult> ExtractZipFile(ZipPath zipPath, FolderPath extractPath)
    {
        if (zipPath.HasValue is false)
        {
            return new(false)
            {
                Message = $"'{nameof(zipPath)}' does not have value",
                StackTrace = null,
                UpdateMsg = UpdateMsg.PathNullOrEmpty
            };
        }
        if (zipPath.PathExist is false)
        {
            return new(false)
            {
                Message = $"'{nameof(zipPath)}' with value '{zipPath}' does not exist in local device",
                StackTrace = null,
                UpdateMsg = UpdateMsg.FileNotFound
            };
        }
        if (extractPath.PathExist is false)
        {
            return new(false)
            {
                Message = $"'{nameof(extractPath)}' does not have value",
                StackTrace = null,
                UpdateMsg = UpdateMsg.PathNullOrEmpty
            };
        }

        var extracter = new ZipExtracter(zipPath, extractPath);
        return await extracter.ExtractAsync();
    }

    /// <summary>
    /// Launch setup.exe from given path, lauching requires admin rights if requireAdmin is true (default is true)
    /// </summary>
    /// <param name="path"></param>
    /// <param name="requireAdmin"></param>
    /// <returns>value representing process success</returns>
    public static StartSetupResult LaunchSetup(SetupExePath path, bool requireAdmin = true)
    {
        if (path.HasValue is false)
        {
            return new(false)
            {
                Message = "Path is not defined",
                SetupExePath = path,
                UpdateMsg = UpdateMsg.PathNullOrEmpty
            };
        }
        if (path.PathExist is false)
        {
            return new(false)
            {
                Message = "Path does not exit in device",
                SetupExePath = path,
                UpdateMsg = UpdateMsg.FileNotFound
            };
        }
        var launcer = new SetupLauncher(path, requireAdmin);
        return launcer.StartSetup();
    }
    
}
