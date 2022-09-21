using MekUpdater.UpdateBuilder.Interfaces;

namespace MekUpdater.UpdateBuilder;

/// <summary>
/// Information that is updated during the update. Includes information paths used and other stuff.
/// </summary>
public class UpdateCompletionInfo : IUpdateCompletionInfo
{
    /// <inheritdoc/>
    public SetupExePath? SetupExePath { get; set; }

    /// <inheritdoc/>
    public VersionTag? AvailableVersion { get; set; }

}
