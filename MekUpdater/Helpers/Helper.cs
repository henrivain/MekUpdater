/// Copyright 2021 Henri Vainio 
using System.Reflection;

namespace MekUpdater.Helpers
{
    public static class Helper
    {
        private static readonly FolderPath _downloadsFolder = new(
            Environment.GetEnvironmentVariable("USERPROFILE")?.ToString() + @"\Downloads" + Path.DirectorySeparatorChar);

        private static readonly FolderPath _appDataFolder = new(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                Path.DirectorySeparatorChar);

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
        /// Gets entry assembly name
        /// </summary>
        /// <returns>entry assembly name or "MekUpdater" if entry assembly name null</returns>
        public static string GetHostAppName()
        {
            return Assembly.GetEntryAssembly()?.GetName()?.Name ?? "MekUpdater";
        }

        /// <summary>
        /// Downloads/updates
        /// </summary>
        public static FolderPath DefaultFluentUpdaterDestinationFolder { get; } = new(GetDefaultUpdateFolder());

        /// <summary>
        /// Downloads/updates/[hostAppName]Setup.zip
        /// </summary>
        public static ZipPath DefaultFluentUpdaterZipFolder { get; } = new(Path.Combine(GetDefaultUpdateFolder(), $"{GetHostAppName()}Setup.zip"));

        /// <summary>
        /// Get default destination folder path for fluent mek updater
        /// </summary>
        /// <returns>full path to "Downloads/updates"</returns>
        private static string GetDefaultUpdateFolder()
        {
            return Path.Combine(DownloadsFolder.ToString(), "updates");
        }
    }
}
