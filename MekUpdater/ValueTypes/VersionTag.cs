
using System.Reflection;
/// Copyright 2021 Henri Vainio 
namespace MekUpdater.ValueTypes;

public class VersionTag
{
    public VersionTag() { }

    /// <summary>
    /// Build version tag from string. Input format must be "vX.X.X" and can include -beta, -alpha or -preview tag. 
    /// If input format is invalid throws ArgumentException.
    /// </summary>
    /// <param name="versionString"></param>
    /// <exception cref="ArgumentException"></exception>
    public VersionTag(string versionString)
    {
        ConvertFromString(versionString);
    }

    /// <summary>
    /// Build version tag from 3 version numbers using format "v{major}.{minor}.{build}" where all variables are positive numbers 
    /// </summary>
    /// <param name="major"></param>
    /// <param name="minor"></param>
    /// <param name="build"></param>
    public VersionTag(uint major, uint minor, uint build)
    {
        Major = major;
        Minor = minor;
        Build = build;
    }

    /// <summary>
    /// Version as string using format "v.X.X.X" where X is positive number
    /// </summary>
    public string Version { get => $"v{Major}.{Minor}.{Build}"; }

    /// <summary>
    /// First number in version string
    /// </summary>
    public uint Major { get; private set; } = 1;
    
    /// <summary>
    /// Second number in version string
    /// </summary>
    public uint Minor { get; private set; } = 0;

    /// <summary>
    /// Third and last number in version string
    /// </summary>
    public uint Build { get; private set; } = 0;

    /// <summary>
    /// If version is beta, preview or alpha it will show here. Default is SpecialId.Full (full release)
    /// </summary>
    public SpecialId VersionId { get; private set; } = SpecialId.Full;

    /// <summary>
    /// Defines version type
    /// </summary>
    public enum SpecialId
    {
        Full, Preview, Beta, Alpha
    }

    /// <summary>
    /// Set VersionId manually
    /// </summary>
    /// <param name="id"></param>
    public void SetVersionId(SpecialId id)
    {
        VersionId = id;
    }

    /// <summary>
    /// Get version from entry assembly 
    /// (can be defined in .csproj file using Version tag)
    /// <para/>throws exception if entryassembly or version is null
    /// </summary>
    /// <returns>version tag for entry assembly version</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static VersionTag GetEntryAssemblyVersion()
    {
        Assembly? entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly is null)
        {
            throw new InvalidOperationException(
                $"can't find {nameof(entryAssembly)}");
        }
        
        string? version = entryAssembly.GetName().Version?.ToString();
        if (version is null)
        {
            throw new InvalidOperationException(
                $"can't find {nameof(version)}");
        }
        
        return new($"v{version}");
    }

    public override bool Equals(object? obj)
    {
        return obj is VersionTag tag &&
               Major == tag.Major &&
               Minor == tag.Minor &&
               Build == tag.Build;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Major, Minor, Build);
    }

    public override string ToString()
    {
        return Version.ToString();
    }

    public static bool operator ==(VersionTag? left, VersionTag? right)
    {
        return EqualityComparer<VersionTag>.Default.Equals(left, right);
    }

    public static bool operator !=(VersionTag? left, VersionTag? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Checks if left side version is bigger than right side. Compare order: Major > Minor > Build
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>true if left side is bigger or right null, else false</returns>
    public static bool operator > (VersionTag? left, VersionTag? right)
    {
        if (left is null) return false;
        if (right is null) return true;
        return IsVersionBigger(left, right);
    }

    /// <summary>
    /// Checks if right side version is bigger than right side. Compare order: Major > Minor > Build
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>true if right side is bigger or left null, else false</returns>
    public static bool operator < (VersionTag? left, VersionTag? right)
    {
        return !(left > right);
    }

    /// <summary>
    /// Checks if right side version is bigger than left side or same. Compare order: Major > Minor > Build
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>true if right side is bigger or left null or both are same, else false</returns>
    public static bool operator <= (VersionTag? left, VersionTag? right)
    {
        if (right is null && left is null) return true;
        if (left is null) return false;
        if (right is null) return true;
        return IsVersionBigger(right, left);
    }

    /// <summary>
    /// Checks if left side version is bigger than right side or same. Compare order: Major > Minor > Build
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>true if left side is bigger or right null or both are same, else false</returns>
    public static bool operator >= (VersionTag? left, VersionTag? right)
    {
        return !(left <= right);
    }



    /// <summary>
    /// Parse version to int[3] array (default {0,0,0})
    /// </summary>
    /// <param name="versionString"></param>
    /// <returns>int[3] of version number vX.X.X where X is int</returns>
    /// <exception cref="ArgumentException"></exception>
    private void ConvertFromString(string versionString)
    {

        if (versionString.StartsWith('v'))
        {
            versionString = versionString.Remove(0, 1);
        }

        (versionString, VersionId) = CheckAndRemoveSpecialIds(versionString);

        string[] versionNumbers = versionString.Split(".");
        uint[] result = new uint[] { 0, 0, 0 };

        for (int i = 0; i <= 2; i++)
        {
            try
            {
                result[i] = uint.Parse(versionNumbers[i]);
            }
            catch
            {
                throw new ArgumentException(
                    $"version string is not right format \"vX.X.X\"; was given {versionString}");
            }
        }
        Major = result[0];
        Minor = result[1];
        Build = result[2];
    }

    /// <summary>
    /// Get and remove special ids, in version string (removes also "-" -symbols)
    /// </summary>
    /// <param name="versionString"></param>
    /// <returns>trimmed version string and fitting SpecialId</returns>
    private static (string versionString, SpecialId id) CheckAndRemoveSpecialIds(string versionString)
    {
        string[] ids = Enum.GetNames(typeof(SpecialId));
        ids = ids.Select(s => s.ToLower()).ToArray();
        
        return (RemoveSpecialIds(versionString, ids), GetIdFromString(versionString, ids));
    }

    /// <summary>
    /// Remove all Special id name strings and "-" -symbols from input
    /// </summary>
    /// <param name="versionString"></param>
    /// <param name="ids"></param>
    /// <returns>input with Special id names and "-" removed</returns>
    private static string RemoveSpecialIds(string versionString, string[] ids)
    {
        foreach (string id in ids)
        {
            versionString = versionString.Replace(id, "");
        }
        return versionString.Replace("-", "");
    }

    /// <summary>
    /// Check if any SpecialIds are present in version string
    /// </summary>
    /// <param name="versionString"></param>
    /// <param name="ids"></param>
    /// <returns>fitting SpecialId or SpecialId.Normal by default</returns>
    private static SpecialId GetIdFromString(string versionString, string[] ids)
    {
        for (var i = 0; i < ids.Length; i++)
        {
            if (versionString.Contains(ids[i]))
            {
                return (SpecialId)i;
            }
        }
        return SpecialId.Full;
    }

    /// <summary>
    /// Compare two version tags if tag bigger than reference
    /// </summary>
    /// <param name="data"></param>
    /// <returns>Is tag version bigger than reference</returns>
    private static bool IsVersionBigger(VersionTag tag, VersionTag reference)
    {
        if (tag.Major > reference.Major) return true;
        if (tag.Minor > reference.Minor) return true;
        if (tag.Build > reference.Build) return true;
        if (tag.VersionId is SpecialId.Full && reference.VersionId != SpecialId.Full) return true;
        return false;
    }

    private static bool AreVersionsSame(VersionTag tag, VersionTag reference)
    {
        bool MajorsSame = (tag.Major == reference.Major);
        bool MinorSame = (tag.Minor == reference.Minor);
        bool BuildSame = (tag.Build == reference.Build);
        return MajorsSame && MinorSame && BuildSame;
    }
}
