using MekPathLibraryTests.UpdateRunner;

namespace MekPathLibraryTests.UpdateBuilder;

public class Update
{
    internal Update(string repoOwner, string repoName)
    {
        RepoInfoUrl = $"https://api.github.com/repos/{repoOwner}/{repoName}/releases/latest";
       
    }
    internal string RepoInfoUrl { get; }
    internal FolderPath SetupDestinationFolder { get; set; } = Helper.DefaultFluentUpdaterDestinationFolder;
    internal ZipPath ZipPath { get; set; } = Helper.DefaultFluentUpdaterZipFolder;
    internal VersionTag CurrentVersion { get; set; } = VersionTag.Min;
    internal bool CanUpdatePreviewVersion { get; set; } = true;
    internal bool StartSetup { get; set; } = true;
    internal bool TidyUpWhenFinishing { get; set; } = true;


    public virtual async Task<UpdateResult> RunDefaultUpdaterAsync()
    {
        IUpdater updater = new DefaultGithubUpdater(this);



        //await updater.




        return updater.Result;
    }
}
