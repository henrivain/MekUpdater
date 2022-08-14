using MekPathLibraryTests.UpdateBuilder;
using MekUpdater.Check;
using MekUpdater.InstallUpdates;
using MekUpdater.UpdateBuilder;

namespace MekUpdater.UpdateRunner;

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
    public DefaultGithubUpdater(Update update, string downloadUrl) : this(update)
    {
        DownloadUrl = downloadUrl;
    }


    public UpdateResult Result { get; private set; }
    private Update Update { get; }
    private string DownloadUrl { get; set; } = string.Empty;
    public SetupExePath? SetupExePath { get; private set; }


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

        GithubZipDownloader downloader = new(DownloadUrl, Update.ZipPath, Update.ExtractionFolder);
        DownloadUpdateFilesResult downloadResult = await downloader.DownloadAsync();

        if (downloadResult.Success is false) return downloadResult;

        ZipExtracter extracter = new(Update.ZipPath);
        return await extracter.ExtractAsync();
    }

    public Task<StartSetupResult> RunSetup()
    {
        //SetupExePath path = SetupExePath.TryFindSetup(Update.ExtractionFolder);
        SetupPathFinder finder = new(new(Update.ExtractionFolder, Update.RepoOwner, Update.RepoName));
        var setupFindResult = finder.TryFindPath();
        if (setupFindResult.Success is false) return Task.FromResult(new StartSetupResult(false)
        {
            Message = setupFindResult.Message,
            UpdateMsg = UpdateMsg.SetupNotFound
        });
        SetupLauncher launcher = new(setupFindResult.SetupPath!);
        return Task.FromResult(launcher.StartSetup());
    }

    public async Task<FinishCleanUpResult> TidyUpAsync()
    {
        FinishCleanUpResult result = new();

        return result;
    }
}
