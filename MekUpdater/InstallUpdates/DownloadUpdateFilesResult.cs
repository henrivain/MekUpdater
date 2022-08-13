using MekPathLibraryTests.UpdateRunner;

namespace MekPathLibraryTests.InstallUpdates;

public class DownloadUpdateFilesResult : GetSetupResult
{
    internal DownloadUpdateFilesResult(bool success) : base(success) { }
}
