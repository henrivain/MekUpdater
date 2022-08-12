/// Copyright 2021 Henri Vainio 

namespace MekUpdater.Helpers
{
    internal static class FilePathHelper
    {
        /// <summary>
        /// Get user's downloads folder path
        /// </summary>
        /// <returns>Downloads path</returns>
        private static string DownloadsPath { get => Environment.GetEnvironmentVariable("USERPROFILE") + @"\Downloads"; }

        /// <summary>
        /// Path to user's Downloads folder with added path "temp\update.zip"
        /// </summary>
        internal static string DownloadsTemp { get => Path.Combine(DownloadsPath, @"temp"); }

        /// <summary>
        /// Returns path to local user app data folder
        /// </summary>
        internal static string LocalAppData 
        {
            get => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }


        /// <summary>
        /// Get path to update.zip file in application's appdata folder 
        /// </summary>
        /// <param name="appName"></param>
        /// <returns>path to "appData/appName/temp/update.zip"</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static string GetAppDataTempZipPath(string appName)
        { 
            if (string.IsNullOrWhiteSpace(appName)) throw new ArgumentNullException(nameof(appName));
            return Path.Combine(AppDataTemp, appName, "update.zip"); 
        }

        /// <summary>
        /// Get path to local folder "appData/appName/temp
        /// </summary>
        /// <param name="appName"></param>
        /// <returns>path to "appData/appName/temp"</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static string GetAppDataTempFolder(string appName)
        { 
            if (string.IsNullOrWhiteSpace(appName)) throw new ArgumentNullException(nameof(appName));
            return Path.Combine(AppDataTemp, appName); 
        }


        /// <summary>
        /// Returns path to local user appdata folder
        /// </summary>
        private static string AppDataTemp { get => Path.Combine(LocalAppData, "temp"); }
    }
}
