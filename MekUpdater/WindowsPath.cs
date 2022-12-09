namespace MekUpdater;

/// <summary>
/// 
/// </summary>
public static class WindowsPath
{
    /// <summary>
    /// ResultPath to user's appdata folder (ending in path separator char)
    /// </summary>
    public static FolderPath AppDataFolder => new(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar);

    /// <summary>
    /// ResultPath to user's downloads folder (ending in path separator char)
    /// </summary>
    public static FolderPath DownloadsFolder => new(
        Environment.GetEnvironmentVariable("USERPROFILE")?.ToString() + @"\Downloads" + Path.DirectorySeparatorChar);

    /// <summary>
    /// ResultPath to user's appdata/temp (ending in path separator char)
    /// </summary>
    public static FolderPath UserTempFolder => new(Path.GetTempPath());

    /// <summary>
    /// Downloads/updates
    /// </summary>
    public static FolderPath DefaultUpdaterDestinationFolder => new(_defaultUpdateFolder);

    /// <summary>
    /// Downloads/updates/[hostAppName]Setup.zip
    /// </summary>
    public static ZipPath DefaultUpdaterZipFolder => new(Path.Combine(_defaultUpdateFolder, $"{AppInfo.GetHostAppName()}Setup.zip"));

    /// <summary>
    /// Default destination folder path for fluent mek updater
    /// </summary>
    /// <returns>full path to "Downloads/updates"</returns>
    static readonly string _defaultUpdateFolder = Path.Combine(DownloadsFolder.FullPath, "updates");

}
