/// Copyright 2021 Henri Vainio 
using MekPathLibrary;

namespace MekUpdater.Helpers
{
    public static class Helper
    {
        private static readonly FolderPath _downloadsFolder = new(
            Environment.GetEnvironmentVariable("USERPROFILE")?.ToString() + @"\Downloads" + Path.DirectorySeparatorChar);

        private static readonly FolderPath _appDataFolder = new (
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                Path.DirectorySeparatorChar);
        
        private static readonly FolderPath _userTempFolder = new (Path.GetTempPath());


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
    }
}
