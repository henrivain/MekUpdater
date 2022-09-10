namespace MekUpdater.Helpers;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Short reason why action was ended, Use 'None' as Default 
/// </summary>
public enum SetupPathMsg
{
    None, EmptyFolder, NoMatchingFolder, CantEnumerateFolders,
    CantEnumerateFiles, NoMatchingFile, Success
}
