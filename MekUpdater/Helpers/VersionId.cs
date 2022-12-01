namespace MekUpdater.Helpers;

/// <summary>
/// Defines version type, like Full, Preview, Beta and Alpha.
/// Smaller number means more "fuller" version ("Full" (0) numerical value is smaller than "Preview" (1))
/// </summary>
public enum VersionId : uint
{

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    Full = 0, 
    Preview = 1, 
    Beta = 2, 
    Alpha = 3
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
