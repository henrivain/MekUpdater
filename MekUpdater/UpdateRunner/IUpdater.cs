namespace MekUpdater.UpdateRunner;

internal interface IUpdater
{
    UpdateResult Result { get; }

    Task<UpdateCheckResult> CheckForUpdatesAsync();

    Task<GetSetupResult> DownloadAndExtractAsync();

    Task<StartSetupResult> RunSetupAsync();

    Task<FinishCleanUpResult> TidyUpAsync();
}
