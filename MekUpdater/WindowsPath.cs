namespace MekUpdater;

/// <summary>
/// 
/// </summary>
public static class WindowsPath
{
    private static readonly FolderPath _downloadsFolder = new(
        Environment.GetEnvironmentVariable("USERPROFILE")?.ToString() + @"\Downloads" + Path.DirectorySeparatorChar);

    private static readonly FolderPath _appDataFolder = new(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar);

    private static readonly FolderPath _userTempFolder = new(Path.GetTempPath());

    /// <summary>
    /// Path to user's appdata folder (ending in path separator char)
    /// </summary>
    public static FolderPath AppDataFolder { get => _appDataFolder; }

    /// <summary>
    /// Path to user's downloads folder (ending in path separator char)
    /// </summary>
    public static FolderPath DownloadsFolder { get => _downloadsFolder; }

    /// <summary>
    /// Path to user's appdata/temp (ending in path separator char)
    /// </summary>
    public static FolderPath UserTempFolder { get => _userTempFolder; }

    /// <summary>
    /// Downloads/updates
    /// </summary>
    public static FolderPath DefaultUpdaterDestinationFolder { get; } = new(GetDefaultUpdateFolder());

    /// <summary>
    /// Downloads/updates/[hostAppName]Setup.zip
    /// </summary>
    public static ZipPath DefaultUpdaterZipFolder { get; } = new(Path.Combine(GetDefaultUpdateFolder(), $"{AppInfo.GetHostAppName()}Setup.zip"));






    /// <summary>
    /// Get default destination folder path for fluent mek updater
    /// </summary>
    /// <returns>full path to "Downloads/updates"</returns>
    private static string GetDefaultUpdateFolder()
    {
        return Path.Combine(DownloadsFolder.ToString(), "updates");
    }
}
