using MekPathLibraryTests.UpdateBuilder;
using MekUpdater.Helpers;
using MekUpdater.UpdateRunner;
using static MekUpdater.UpdateDownloadInfo;
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
                UpdateMsg = UpdateMsg.CantInstallPreview,
                Message = $"Can't install {updateCheckResult.AvailableVersion} " +
                $"because can install preview is false. " +
                $"Meaning process was successfull!"
            };
        }

        result = await updater.DownloadAndExtractAsync();
        if (result.Success is false) return result;
        if (StartSetup is false)
        {
            return new(true)
            {
                
            };
        }




        return updater.Result;
    }

    private bool IsCurrentVersionSameOrBiggerThan(string? tag_name)
    {
        throw new NotImplementedException();
    }
}
