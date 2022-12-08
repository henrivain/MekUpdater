namespace MekUpdater.ActionResults;

/// <summary>
/// Result from attempt to find setup.exe file
/// </summary>
public class SetupPathFinderResult
{
    internal SetupPathFinderResult(bool success)
    {
        Success = success;
    }

    /// <summary>
    /// Was action successful?
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// ResultPath to setup.exe file
    /// </summary>
    public virtual SetupExePath? SetupPath { get; init; }

    /// <summary>
    /// More thorought explanation of action 
    /// </summary>
    public virtual string Message { get; init; } = string.Empty;

    /// <summary>
    /// Short reason for action failing or ending
    /// </summary>
    public virtual SetupPathMsg SetupPathMsg { get; init; } = SetupPathMsg.None;
}
