// Copyright 2022 Henri Vainio 

using System.Reflection;

namespace MekUpdater.Helpers;

/// <summary>
/// Information about application assemblies
/// </summary>
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
