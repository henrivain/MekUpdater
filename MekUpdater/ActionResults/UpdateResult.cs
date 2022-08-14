using MekUpdater.UpdateRunner;

namespace MekUpdater.ActionResults;

public class UpdateResult
{
    internal UpdateResult(bool success)
    {
        Success = success;
    }

    public bool Success { get; }
    public virtual string Message { get; init; } = string.Empty;
    public virtual UpdateMsg UpdateMsg { get; init; } = UpdateMsg.None;
}
