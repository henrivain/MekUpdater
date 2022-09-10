using MekUpdater.UpdateRunner;

namespace MekUpdater.ActionResults;

/// <summary>
/// Base result for update actions
/// </summary>
public class UpdateResult
{
    internal UpdateResult(bool success)
    {
        Success = success;
    }

    /// <summary>
    /// Was action execution successful?
    /// </summary>
    public bool Success { get; }
    
    /// <summary>
    /// Short reason why action was ended (was it exception?)
    /// </summary>
    public virtual UpdateMsg UpdateMsg { get; init; } = UpdateMsg.None;

    /// <summary>
    /// More thorough explanation about action
    /// </summary>
    public virtual string Message { get; init; } = string.Empty;
}
