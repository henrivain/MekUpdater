namespace MekUpdater.ActionResults;

public class SetupPathFinderResult
{
    internal SetupPathFinderResult(bool success)
    {
        Success = success;
    }
    public bool Success { get; }
    public virtual SetupExePath? SetupPath { get; init; }
    public virtual string Message { get; init; } = string.Empty;
    public virtual SetupPathMsg SetupPathMsg { get; init; } = SetupPathMsg.None;
}
