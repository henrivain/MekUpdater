using MekPathLibraryTests.Helpers;

namespace MekPathLibraryTests.Fluent.Interfaces;

public interface ICanDownloadUpdate
{
    Task<ICanStartSetup> DownloadAnyway();
    Task<ICanStartSetup> DownloadIfNewVersion(VersionTag currentVersion);

    UpdateDownloadInfo Info { get; }

}
