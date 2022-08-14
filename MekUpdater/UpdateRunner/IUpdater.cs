using MekPathLibraryTests.UpdateBuilder;

namespace MekUpdater.UpdateRunner;

internal interface IUpdater
{
    UpdateResult Result { get; }

    Task<UpdateCheckResult> CheckForUpdatesAsync();

    Task<GetSetupResult> DownloadAndExtractAsync();

    Task<bool> RunSetup();

    Task<FinishCleanUpResult> TidyUpAsync();
}
