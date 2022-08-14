using MekPathLibraryTests.UpdateBuilder;

namespace MekUpdater.UpdateRunner;

internal interface IUpdater
{
    UpdateResult Result { get; }

    Task<UpdateCheckResult> CheckForUpdatesAsync();

    Task<GetSetupResult> DownloadAndExtractAsync();

    Task<StartSetupResult> RunSetup();

    Task<FinishCleanUpResult> TidyUpAsync();
}
