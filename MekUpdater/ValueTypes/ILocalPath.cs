namespace MekUpdater.ValueTypes;

/// <summary>
/// Interface to define how local path value types should be implemented
/// </summary>
public interface ILocalPath
{
    /// <summary>
    /// Value of the ILocalPath
    /// </summary>
    string FullPath { get; set; }
    /// <summary>
    /// Check weather ILocalPath instance has FullPath value or not
    /// </summary>
    bool HasValue { get; }
    /// <summary>
    /// Check if value of instance is valid path
    /// </summary>
    /// <returns>true if valid, else false</returns>
    bool IsValid();
    int GetHashCode();
    string ToString();
    bool Equals(object? obj);
    /// <summary>
    /// Check weather defined path exist or not
    /// </summary>
    /// <returns>true if path exist, else false</returns>
    bool PathExist();
}
