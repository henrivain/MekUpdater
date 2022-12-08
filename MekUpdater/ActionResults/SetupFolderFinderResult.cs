namespace MekUpdater.ActionResults;

/// <summary>
/// Result from attempt to find setup.exe file containing folder
/// </summary>
public class SetupFolderFinderResult : SetupPathFinderResult
{
    internal SetupFolderFinderResult(bool success) : base(success) { }

    /// <summary>
    /// This is ALWAYS NULL if not overridden, because it is only derived from base and should not be defined in this result
    /// </summary>
    public override SetupExePath? SetupPath => null;

    /// <summary>
    /// ResultPath to folder that contains setup.exe
    /// </summary>
    public virtual string? SetupFolderName { get; init; }
}
