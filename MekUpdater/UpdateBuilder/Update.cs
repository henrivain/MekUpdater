using MekUpdater.UpdateRunner;
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
    internal FolderPath ExtractionFolder { get; set; } = Helper.DefaultFluentUpdaterDestinationFolder;
    internal ZipPath ZipPath { get; set; } = Helper.DefaultFluentUpdaterZipFolder;
    internal VersionTag CurrentVersion { get; set; } = VersionTag.Min;
    internal bool CanUpdatePreviewVersion { get; set; } = true;
    internal bool StartSetup { get; set; } = true;
    internal bool TidyUpWhenFinishing { get; set; } = true;


    public virtual async Task<UpdateResult> RunDefaultUpdaterAsync()
    {
        IUpdater updater = new DefaultGithubUpdater(this);
        UpdateResult result;

        var updateCheckResult = await updater.CheckForUpdatesAsync();
        if (updateCheckResult.Success is false) return updateCheckResult;
        if (CurrentVersion >= updateCheckResult.AvailableVersion)
        {
            return new(true)
            {
                UpdateMsg = UpdateMsg.UpdateAlreadyInstalled,
                Message = $"Installed version {CurrentVersion} is newer or same " +
                $"than available version {updateCheckResult.AvailableVersion}. " +
                $"Meaning process was successfull!"
            };
        }
        if (CanUpdatePreviewVersion is false && updateCheckResult.AvailableVersion?.VersionId is not Full)
        {
            return new(true)
            {
                Message = $"Success, can't install version '{updateCheckResult.AvailableVersion}' " +
                $"because preview installation is false.",
                UpdateMsg = UpdateMsg.Completed,
            };
        }

        result = await updater.DownloadAndExtractAsync();
        if (result.Success is false) return result;
        
        if (StartSetup is false)
        {
            return new(true)
            {
                Message = $"Doesn't start setup, because {nameof(StartSetup)} is false. " +
                $"Ending update as successful",
                UpdateMsg = UpdateMsg.Completed
            };
        }

        result = await updater.RunSetupAsync();
        if (result.Success is false) return result;

        if (TidyUpWhenFinishing is false)
        {
            return new(true)
            {
                Message = $"{nameof(TidyUpWhenFinishing)} is false, end update as successfull",
                UpdateMsg = UpdateMsg.Completed
            };
        }
        result = await updater.TidyUpAsync();
        return result;
    }
}
