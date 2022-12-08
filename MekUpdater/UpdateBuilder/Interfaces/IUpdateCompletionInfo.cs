namespace MekUpdater.UpdateBuilder.Interfaces;

/// <summary>
/// Information that is updated during the update. Includes information paths used and other stuff.
/// </summary>
public interface IUpdateCompletionInfo
{
    /// <summary>
    /// ResultPath that is defined after update if success and setup.exe is found from extraction folder.
    /// </summary>
    SetupExePath? SetupExePath { get; set; }

    /// <summary>
    /// Newest version read from github api
    /// </summary>
    VersionTag? AvailableVersion { get; set; }
}
