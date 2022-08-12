using MekUpdater.Helpers;

namespace MekUpdater.Fluent.Interfaces;

public interface ICanCheckUpdates
{
    Task<ICanDownloadUpdate> CheckUpdates();

    ICanDownloadUpdate UseChecker(UpdateChecker checker);

    ICanDownloadUpdate UseUrl(string downloadUrl, VersionTag openVersion);

    ICanCheckUpdates UnInstallOldVersion();

    ICanCheckUpdates UseLogging(int milliSecondsDelay = 1000);

    UpdateDownloadInfo Info { get; }


}
