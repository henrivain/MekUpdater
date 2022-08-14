using MekPathLibraryTests.UpdateBuilder;
using MekUpdater.Check;
using MekUpdater.InstallUpdates;
using MekUpdater.UpdateBuilder;

namespace MekUpdater.UpdateRunner;

public class DefaultGithubUpdater : IUpdater
{
    public UpdateResult Result { get; private set; }
    private Update Update { get; }
    private string DownloadUrl { get; set; } = string.Empty;
    public DefaultGithubUpdater(Update update)
    {
        Result = new(true)
        {
            Message = "Update result not set yet"
        };
        Update = update;
    }
    public async Task<UpdateCheckResult> CheckForUpdatesAsync()
    {
        GithubApiClient client = new(Update.RepoInfoUrl);
        UpdateCheckResult result = await client.GetVersionData();
        if (result.Success is false) return result;
        if (result.VersionData?.zipball_url is null)
        {
            return new(false)
            {
                Message = "github download url 'zipball_url' is null",
                ErrorMsg = UpdateDownloadInfo.ErrorMsg.BadUrl,
                VersionData = null,
                UsedUrl = Update.RepoInfoUrl
            };
        }
        DownloadUrl = result.VersionData!.zipball_url;
        return result;
    }
    public async Task<GetSetupResult> DownloadAndExtractAsync()
    {
        if (string.IsNullOrEmpty(DownloadUrl))
        {
            throw new InvalidOperationException($"{nameof(DownloadUrl)} can't be empty or null");
        }

        GithubZipDownloader downloader = new(DownloadUrl, Update.ZipPath, Update.SetupDestinationFolder);
        DownloadUpdateFilesResult downloadResult = await downloader.DownloadAsync();

        if (downloadResult.Success is false) return downloadResult;

        ZipExtracter extracter = new(Update.ZipPath);
        return await extracter.ExtractAsync();
    }

    public Task<bool> RunSetup()
    {
        //SetupExePath path = SetupExePath.TryFindSetup(Update.SetupDestinationFolder);


        return Task.FromResult(true);
    }

    public async Task<FinishCleanUpResult> TidyUpAsync()
    {
        FinishCleanUpResult result = new();

        return result;
    }




}
