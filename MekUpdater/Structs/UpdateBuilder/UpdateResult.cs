using static MekUpdater.UpdateDownloadInfo;

namespace MekPathLibraryTests.UpdateBuilder;

public class UpdateResult
{
    internal UpdateResult(bool success)
    {
        Success = success;
    }

    public bool Success { get; }
    public virtual string Message { get; init; } = string.Empty;
    public virtual ErrorMsg ErrorMsg { get; init; } = ErrorMsg.None;
}
