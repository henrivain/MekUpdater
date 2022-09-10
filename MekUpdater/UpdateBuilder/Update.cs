using MekUpdater.UpdateRunner;
using Microsoft.Extensions.Logging;
using static MekUpdater.Helpers.VersionTag.SpecialId;

namespace MekUpdater.UpdateBuilder;

public class Update
{
    internal Update(string repoOwner, string repoName)
    {
        if (string.IsNullOrEmpty(repoOwner)) throw new ArgumentNullException(nameof(repoOwner));
        if (string.IsNullOrEmpty(repoName)) throw new ArgumentNullException(nameof(repoName));
        RepoOwner = repoOwner;
        RepoName = repoName;
    }

    public string RepoOwner { get; }
    public string RepoName { get; }
    internal string RepoInfoUrl => $"https://api.github.com/repos/{RepoOwner}/{RepoName}/releases/latest";
    public FolderPath ExtractionFolder { get; internal set; } = WindowsPath.DefaultUpdaterDestinationFolder;
    public ZipPath ZipPath { get; internal set; } = WindowsPath.DefaultUpdaterZipFolder;
    public VersionTag CurrentVersion { get; internal set; } = VersionTag.Min;
    public bool CanUpdatePreviewVersion { get; internal set; } = true;
    public bool StartSetup { get; internal set; } = true;
    public bool TidyUpWhenFinishing { get; internal set; } = true;
    internal UpdateLogger Logger { get; set; } = new(null);
    public SetupExePath? SetupExePath { get; internal set; }


    public virtual async Task<UpdateResult> RunDefaultUpdaterAsync()
    {
        Logger.LogMessage("Check for updates", LogType.Info);
        IUpdater updater = new DefaultGithubUpdater(this);
        UpdateResult result;

        var updateCheckResult = await updater.CheckForUpdatesAsync();
        Logger.LogResult(updateCheckResult, "Update check");
        if (updateCheckResult.Success is false) return updateCheckResult;
        
        if (CurrentVersion >= updateCheckResult.AvailableVersion)
        {
            return ExitUpdateAlreadyInstalled(updateCheckResult);
        }
        if (CanUpdatePreviewVersion is false && updateCheckResult.AvailableVersion?.VersionId is not Full)
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
        SetupExePath = setupResult.SetupExePath;
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
        UpdateResult result = new(true)
        {
            Message = $"Doesn't start setup, because {nameof(StartSetup)} is false. " +
            $"Ending update as successful",
            UpdateMsg = UpdateMsg.Completed
        };
        Logger.LogResult(result, "Setup start check");
        return result;
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
