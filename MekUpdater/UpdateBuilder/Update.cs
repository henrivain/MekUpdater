using MekUpdater.UpdateBuilder.Interfaces;
using MekUpdater.UpdateRunner;

namespace MekUpdater.UpdateBuilder;

/// <summary>
/// Class store data needed for update run to be successful
/// </summary>
public class Update
{
    internal Update(string repoOwner, string repoName)
    {
        if (string.IsNullOrEmpty(repoOwner)) throw new ArgumentNullException(nameof(repoOwner));
        if (string.IsNullOrEmpty(repoName)) throw new ArgumentNullException(nameof(repoName));
        RepoOwner = repoOwner;
        RepoName = repoName;
    }

    /// <summary>
    /// Github username of repository owner (case sensitive)
    /// </summary>
    public string RepoOwner { get; }

    /// <summary>
    /// Name of repository, where update will be downloaded from releases
    /// </summary>
    public string RepoName { get; }

    /// <summary>
    /// Url path to Github api repository info
    /// </summary>
    internal string RepoInfoUrl => $"https://api.github.com/repos/{RepoOwner}/{RepoName}/releases/latest";

    /// <summary>
    /// ResultPath where zip file downloaded from github api will be stored
    /// <para/>Default: user/Downloads/updates/[hostAppName]Setup.zip
    /// </summary>
    public ZipPath ZipPath { get; internal set; } = WindowsPath.DefaultUpdaterZipFolder;

    /// <summary>
    /// ResultPath to folder where files from downloaded zip file will be stored after extraction
    /// <para/> Default: user/Downloads/updates
    /// </summary>
    public FolderPath ExtractionFolder { get; internal set; } = WindowsPath.DefaultUpdaterDestinationFolder;

    /// <summary>
    /// Available version will be compared to this version. 
    /// Update will be run if this version is older than available
    /// </summary>
    public VersionTag CurrentVersion { get; internal set; } = VersionTag.Min;

    /// <summary>
    /// Is updater permitted to download preview versions
    /// <para/>preview version are all that include -alpha, -beta or -preview in version tag
    /// </summary>
    public bool CanUpdatePreviewVersion { get; internal set; } = true;

    /// <summary>
    /// If true, setup.exe will be started at the end of update
    /// </summary>
    public bool StartSetup { get; internal set; } = true;

    /// <summary>
    /// Remove useless cache files after extraction is completed
    /// </summary>
    public bool TidyUpWhenFinishing { get; internal set; } = true;

    /// <summary>
    /// Logger that will be used if it is not null
    /// </summary>
    internal UpdateLogger Logger { get; set; } = new(null);

    /// <summary>
    /// Information about will be updated during the update, like where setup.exe path is stored
    /// </summary>
    public IUpdateCompletionInfo CompletionInfo { get; } = new UpdateCompletionInfo();

    /// <summary>
    /// Run update instance using DefaultGithubUpdater asynchronously
    /// </summary>
    /// <returns>awaitable task of UpdateResult or result might be also derived class of UpdateResult</returns>
    public virtual async Task<UpdateResult> RunDefaultUpdaterAsync()
    {
        Logger.LogMessage("Check for updates", LogType.Info);
        IUpdater updater = new DefaultGithubUpdater(this);
        UpdateResult result;

        var updateCheckResult = await updater.CheckForUpdatesAsync();
        Logger.LogResult(updateCheckResult, "Update check");
        if (updateCheckResult.Success is false) return updateCheckResult;

        CompletionInfo.AvailableVersion = updateCheckResult.AvailableVersion;
        if (CurrentVersion >= updateCheckResult.AvailableVersion)
        {
            return ExitUpdateAlreadyInstalled(updateCheckResult);
        }
        if (CanUpdatePreviewVersion is false && updateCheckResult.AvailableVersion?.VersionId is not VersionId.Full)
        {
            return ExitOnlyPreviewAvailable(updateCheckResult);
        }
        Logger.LogMessage("UPDATE: new update available, start update", LogType.Info);

        result = await updater.DownloadAndExtractAsync();
        Logger.LogResult(result, "Download and extract");
        if (result.Success is false) return result;

        if (StartSetup is false)
        {
            return ExitNoNeedToStartSetup();
        }
        StartSetupResult setupResult = await updater.RunSetupAsync();
        Logger.LogResult(setupResult, "Setup start");
        CompletionInfo.SetupExePath = setupResult.SetupExePath;
        if (setupResult.Success is false) return setupResult;

        if (TidyUpWhenFinishing is false)
        {
            return ExitNoNeedForCleanUp();
        }
        var finishResult = await updater.TidyUpAsync();
        Logger.LogResult(finishResult, "Tidy up");
        return finishResult;
    }

    private UpdateResult ExitNoNeedForCleanUp()
    {
        UpdateResult result = new(true)
        {
            Message = $"{nameof(TidyUpWhenFinishing)} is false, end update as successfull",
            UpdateMsg = UpdateMsg.Completed
        };
        Logger.LogResult(result, "Tidy up check");
        return result;
    }
    private UpdateResult ExitNoNeedToStartSetup()
    {
        var setupPath = TryFindSetupPath();
        if (setupPath is not null)
        {
            CompletionInfo.SetupExePath = setupPath;
        }

        UpdateResult result = new(true)
        {
            Message = $"Doesn't start setup, because {nameof(StartSetup)} is false. " +
            $"Ending update as successful",
            UpdateMsg = UpdateMsg.Completed
        };
        Logger.LogResult(result, "Setup start check");
        return result;
    }
    private SetupExePath? TryFindSetupPath()
    {
        SetupPathFinder finder = new(new(ExtractionFolder, RepoOwner, RepoName));
        var setupPathResult = finder.TryFindPath();
        Logger.LogMessage($"{StartSetup} is set to false. Try still find setup path", LogType.Info);
        if (setupPathResult.SetupPath is not null)
        {
            return setupPathResult.SetupPath;
        }
        if (setupPathResult?.Success is true)
        {
            Logger.LogMessage($"Look fot setup.exe was successful '{ExtractionFolder}' " +
                $"but path was still null", LogType.Warning);
            return null;
        }
        Logger.LogMessage($"Setup exe path was not found from folder '{ExtractionFolder}' " +
            $"because of '{setupPathResult?.SetupPathMsg}', " +
            $"message '{setupPathResult?.Message}'", LogType.Warning);
        return null;
    }
    private UpdateResult ExitOnlyPreviewAvailable(UpdateCheckResult updateCheckResult)
    {
        UpdateResult result = new(true)
        {
            Message = $"Success, can't install version '{updateCheckResult.AvailableVersion}' " +
            $"because preview installation is false.",
            UpdateMsg = UpdateMsg.Completed,
        };
        Logger.LogResult(result, "Version check");
        return result;
    }
    private UpdateResult ExitUpdateAlreadyInstalled(UpdateCheckResult updateCheckResult)
    {
        UpdateResult result = new(true)
        {
            UpdateMsg = UpdateMsg.UpdateAlreadyInstalled,
            Message = $"Installed version {CurrentVersion} is newer or same " +
                      $"than available version {updateCheckResult.AvailableVersion}. " +
                      $"Meaning process was successfull!"
        };
        Logger.LogResult(result, "Version check");
        return result;
    }
}
