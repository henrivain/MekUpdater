using MekUpdater.ValueTypes;

namespace MekUpdater.Fluent.Interfaces;

public interface ICanDownloadUpdate
{
    Task<ICanStartSetup> DownloadAnyway();
    Task<ICanStartSetup> DownloadIfNewVersion(VersionTag currentVersion);

    UpdateDownloadInfo Info { get; }

}
