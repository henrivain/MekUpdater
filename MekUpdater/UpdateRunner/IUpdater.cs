using MekPathLibraryTests.UpdateBuilder;

namespace MekPathLibraryTests.UpdateRunner;

internal interface IUpdater
{
    UpdateResult Result { get; }

    Task<UpdateCheckResult> CheckForUpdatesAsync();

    Task<DownloadUpdateFilesResult> UpdateAndExtractAsync();

    Task<bool> RunSetup();

    Task<FinishCleanUpResult> TidyUpAsync();
}
