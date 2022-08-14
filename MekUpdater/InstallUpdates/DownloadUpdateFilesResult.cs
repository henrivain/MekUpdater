using MekUpdater.UpdateRunner;

namespace MekUpdater.InstallUpdates;

public class DownloadUpdateFilesResult : GetSetupResult
{
    internal DownloadUpdateFilesResult(bool success) : base(success) { }
}
