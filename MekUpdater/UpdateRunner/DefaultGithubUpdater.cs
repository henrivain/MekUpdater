using MekUpdater.CheckUpdates;
using MekUpdater.InstallUpdates;
using MekUpdater.UpdateBuilder;

namespace MekUpdater.UpdateRunner;

/// <summary>
/// Default updater used to run update process
/// </summary>
public class DefaultGithubUpdater : IUpdater
{
    /// <summary>
    /// Initialize new DefaultGithubUpdater with update
    /// </summary>
    /// <param name="update"></param>
    public DefaultGithubUpdater(Update update)
    {
        Result = new(true)
        {
            Message = "Update result not set yet"
        };
        Update = update;
    }

    /// <summary>
    /// Initialize new DefaultGithubUpdater with update and update download url. 
    /// <para/>If download url is initialized with updater, 
    /// you can skip check for updates part if you know you will download update anyway.
    /// </summary>
    /// <param name="update"></param>
    /// <param name="downloadUrl"></param>
    /// <exception cref="ArgumentException"></exception>
    public DefaultGithubUpdater(Update update, string downloadUrl) : this(update)
    {
        if (string.IsNullOrWhiteSpace(downloadUrl))
        {
            throw new ArgumentException($"'{nameof(downloadUrl)}' cannot be null or whitespace.", nameof(downloadUrl));
        }
        DownloadUrl = downloadUrl;
    }

    /// <summary>
    /// Result from the update process, defines reasons for certain actions like fails
    /// </summary>
    public UpdateResult Result { get; private set; }

    /// <summary>
    /// Send api request into github api to check weather new version is available
    /// </summary>
    /// <returns>
    /// UpdateCheckResult with 'Success' property indicating if process was successfull.
    /// If action fails also fills other data about failed action. 
    /// Sets this.DownloadUrl property if successfull.
    /// </returns>
    public async Task<UpdateCheckResult> CheckForUpdatesAsync()
    {
        GithubApiClient client = new(Update.RepoInfoUrl);
        UpdateCheckResult result = await client.GetVersionData();
        if (result.Success is false) return result;
        if (result.DownloadUrl is null)
        {
            return new(false)
            {
                Message = "github download url 'zipball_url' is null",
                UpdateMsg = UpdateMsg.BadUrl
            };
        }
        DownloadUrl = result.DownloadUrl;
        return result;
    }

    /// <summary>
    /// Download code zip file from github api, save file in zip path defined in Update. 
    /// Then extract that file into DestinationFolderPath
    /// </summary>
    /// <returns>
    /// GetSetupResult with 'Success' property indicating if process was successfull.
    /// If action fails also fills other data about failed action
    /// </returns>
    /// <exception cref="InvalidOperationException">throws when Download url is null and action can't be completed</exception>
    public async Task<GetSetupResult> DownloadAndExtractAsync()
    {
        if (string.IsNullOrEmpty(DownloadUrl))
        {
            throw new InvalidOperationException($"{nameof(DownloadUrl)} can't be empty or null");
        }

        GithubZipDownloader downloader = new(DownloadUrl, Update.ZipPath);
        DownloadUpdateFilesResult downloadResult = await downloader.DownloadAsync();

        if (downloadResult.Success is false) return downloadResult;

        ZipExtracter extracter = new(Update.ZipPath, Update.ExtractionFolder);
        return await extracter.ExtractAsync();
    }

    /// <summary>
    /// Get setup file path from ExtractFolderPath/Extracted/setup.zip. Then run if setup was found
    /// </summary>
    /// <returns>
    /// StartSetupResult with 'Success' property indicating weather process was successfull or not.
    /// If action fails also fills other data about why so
    /// </returns>
    public Task<StartSetupResult> RunSetupAsync()
    {
        SetupPathFinder finder = new(new(Update.ExtractionFolder, Update.RepoOwner, Update.RepoName));
        var setupFindResult = finder.TryFindPath();
        if (setupFindResult.Success is false) return Task.FromResult(
            new StartSetupResult(false)
            {
                Message = setupFindResult.Message,
                UpdateMsg = UpdateMsg.SetupNotFound,
                SetupExePath = null
            });
        SetupLauncher launcher = new(setupFindResult.SetupPath!);
        return Task.FromResult(launcher.StartSetup());
    }

    /// <summary>
    /// Gets list of temporary update files like downloaded zip file and deletes them from file system
    /// </summary>
    /// <returns>
    /// FinishCleanUpResult with 'Success' property indicating weather process was successfull or not.
    /// If action fails also fills other data about why so
    /// </returns>
    public async Task<FinishCleanUpResult> TidyUpAsync()
    {
        List<string> filesToDelete = new()
        {
            // Can't really delete setup.exe, because it might not be run to end by user
            Update.ZipPath.FullPath    
        };
        foreach (var file in filesToDelete)
        {
            FinishCleanUpResult? result = await TryDeleteFile(file);
            if (result is not null) return result;
        }
        return new(true);
    }

    private Update Update { get; }
    private string DownloadUrl { get; set; } = string.Empty;

    /// <summary>
    /// Delete file if it exist in filesystem using Task.Run
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>null if success, else FinishCleanUpResult with info about fail</returns>
    private static async Task<FinishCleanUpResult?> TryDeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            try
            {
                await Task.Run(() =>
                {
                    File.Delete(filePath);
                });
            }
            catch (Exception ex)
            {
                return new(false)
                {
                    Message = $"Failed to delete file {filePath} because of exception {ex}: {ex.Message}",
                    UpdateMsg = UpdateMsg.DeleteFileFailed
                };
            }
        }
        return null;
    }
}
