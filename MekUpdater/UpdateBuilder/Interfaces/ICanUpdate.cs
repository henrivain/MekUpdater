namespace MekUpdater.UpdateBuilder.Interfaces;

/// <summary>
/// Run update if some conditions are met, part of update builder fluent api
/// </summary>
public interface ICanRunUpdate
{
    /// <summary>
    /// Requires version bigger than this to make an update
    /// </summary>
    /// <param name="version"></param>
    /// <returns>calling instance</returns>
    ICanRunUpdate IfVersionBiggerThan(VersionTag version);

    /// <summary>
    /// Requires availabel version NOT to include preview tag to make an update
    /// </summary>
    /// <returns>calling instance</returns>
    ICanRunUpdate IfNotPreview();

    /// <summary>
    /// Start setup parameter, will define setup start mode in next call using IStartSetupMode interface
    /// </summary>
    /// <returns>calling instance</returns>
    IStartSetupMode StartsSetup();
}
