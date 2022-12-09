namespace MekUpdater.Interfaces;

/// <summary>
/// Implementation instructions for version (release) tag that updater accepts
/// </summary>
public interface IVersion : IComparable<IVersion>
{
    /// <summary>
    /// Get full version string
    /// </summary>
    public string FullVersion { get; }
    /// <summary>
    /// Major release number of this version
    /// </summary>
    public uint Major { get; }

    /// <summary>
    /// Minor release number of this version
    /// </summary>
    public uint Minor { get; }

    /// <summary>
    /// Build number of this version 
    /// </summary>
    public uint Build { get; }

    /// <summary>
    /// Special tag as uint that might be part of this release (Smaller number means bigger release). 
    /// Means something like "beta" or "preview"
    /// </summary>
    public uint SpecialId { get; }
}
