using System.Data.SqlTypes;
using MekPathLibraryTests.UpdateBuilder;

namespace MekPathLibraryTests.UpdateRunner;

public class DefaultGithubUpdater : IUpdater
{
    public UpdateResult Result { get; private set; }
    private Update Update { get; }

    public DefaultGithubUpdater(Update update)
    {
        Result = new();
        Update = update;
    }

    public async Task<UpdateCheckResult> CheckForUpdatesAsync()
    {
        UpdateCheckResult result = new();


        return result;
    }

    public async Task<DownloadUpdateFilesResult> UpdateAndExtractAsync()
    {
        DownloadUpdateFilesResult result = new();
        


        return result;
    }

    public Task<bool> RunSetup()
    {



        return Task.FromResult(true);
    }

    public async Task<FinishCleanUpResult> TidyUpAsync()
    {
        FinishCleanUpResult result = new();

        return result;
    }
}
