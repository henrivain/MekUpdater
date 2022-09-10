/// Copyright 2021 Henri Vainio 
using System.Reflection;

namespace MekUpdater.Helpers
{
    public static class AppInfo
    {
        /// <summary>
        /// Gets entry assembly name
        /// </summary>
        /// <returns>entry assembly name or "MekUpdater" if entry assembly name null</returns>
        public static string GetHostAppName()
        {
            return Assembly.GetEntryAssembly()?.GetName()?.Name ?? "MekUpdater";
        }
    }
}
