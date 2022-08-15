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
    internal FolderPath ExtractionFolder { get; set; } = Helper.DefaultUpdaterDestinationFolder;
    internal ZipPath ZipPath { get; set; } = Helper.DefaultUpdaterZipFolder;
    internal VersionTag CurrentVersion { get; set; } = VersionTag.Min;
    internal bool CanUpdatePreviewVersion { get; set; } = true;
    internal bool StartSetup { get; set; } = true;
    internal bool TidyUpWhenFinishing { get; set; } = true;
    internal UpdateLogger Logger { get; set; } = new(null);

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
        result = await updater.RunSetupAsync();
        Logger.LogResult(result, "Setup start");
        if (result.Success is false) return result;

        if (TidyUpWhenFinishing is false)
        {
            return ExitNoNeedForCleanUp();
        }
        result = await updater.TidyUpAsync();
        Logger.LogResult(result, "Tidy up");
        return result;
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
