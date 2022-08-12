using MekUpdater.Helpers;

namespace MekUpdater.Fluent.Interfaces;

public interface ICanDownloadUpdate
{
    Task<ICanStartSetup> DownloadAnyway();
    Task<ICanStartSetup> DownloadIfNewVersion(VersionTag currentVersion);

    UpdateDownloadInfo Info { get; }

}
