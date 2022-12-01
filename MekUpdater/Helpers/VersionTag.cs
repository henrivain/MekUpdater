// Copyright 2022 Henri Vainio 
using System.Reflection;
namespace MekUpdater.Helpers;

/// <summary>
/// Github style version tag using string format vX.X.X, where X is number
/// </summary>
public sealed class VersionTag : IEquatable<VersionTag>
{
    /// <summary>
    /// Initialize new empty version tag 
    /// </summary>
    public VersionTag() { }

    /// <summary>
    /// Build version tag from string. Input format must include at least one number. 
    /// Full format for version that can be parsed is "vX.X.X-versionId", where X is any number
    /// and versionId can be for example "-beta", "-alpha" or "-preview". 
    /// </summary>
    /// <param name="versionString"></param>
    /// <exception cref="ArgumentException">If input format is invalid.</exception>
    public VersionTag(string versionString)
    {
        FromString(versionString);
    }

    /// <summary>
    /// Build version tag from 3 version numbers.
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
    /// Get version in full format. Format will be "vX.X.X" if version is full. If has any other versionIds, format is "vX.X.X-versionId".
    /// </summary>
    public string FullVersion
    {
        get
        {
            if (VersionId is VersionId.Full)
            {
                return $"v{Major}.{Minor}.{Build}";
            }
            return $"v{Major}.{Minor}.{Build}-{VersionId.ToString().ToLower()}";
        }
    }

    /// <summary>
    /// Version as string using format "v.X.X.X" where X is positive number. Does not include versionIds.
    /// </summary>
    public string Version => $"v{Major}.{Minor}.{Build}";

    /// <summary>
    /// First number in version string
    /// </summary>
    public uint Major { get; private set; } = 0;

    /// <summary>
    /// Second number in version string
    /// </summary>
    public uint Minor { get; private set; } = 0;

    /// <summary>
    /// Third and last number in version string
    /// </summary>
    public uint Build { get; private set; } = 0;

    /// <summary>
    /// If version is beta, preview or alpha it will show here. Default is VersionId.Full (full release)
    /// </summary>
    public VersionId VersionId { get; private set; } = VersionId.Full;

    /// <summary>
    /// Set VersionId manually
    /// </summary>
    /// <param name="id"></param>
    public void SetVersionId(VersionId id)
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
        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly is null)
        {
            throw new InvalidOperationException(
                $"can't find {nameof(entryAssembly)}");
        }

        var version = entryAssembly.GetName().Version?.ToString();
        if (version is null)
        {
            throw new InvalidOperationException(
                $"can't find {nameof(version)}");
        }

        return new($"v{version}");
    }

    /// <summary>
    /// Minimum value for version tag 'v0.0.0-alpha'
    /// </summary>
    public static VersionTag Min => new("v0.0.0-alpha");

    /// <summary>
    /// Try convert string to version tag
    /// </summary>
    /// <param name="version">string to parse</param>
    /// <param name="tag">variable where tag will be parsed</param>
    /// <returns>true if success, else false</returns>
    public static bool TryParse(string version, out VersionTag tag)
    {
        try
        {
            tag = new VersionTag(version);
            return true;
        }
        catch (ArgumentException)
        {
            tag = Min;
            return false;
        }
    }

    /// <summary>
    /// Check weather this weather tag value matches another instance
    /// </summary>
    /// <param name="other"></param>
    /// <returns>True if has same values. False otherwise. Returns false if any of the values are null</returns>
    public bool Equals(VersionTag? other)
    {
        return other is not null &&
               this is not null &&
               Major == other.Major &&
               Minor == other.Minor &&
               Build == other.Build &&
               VersionId == other.VersionId;
    }

    /// <summary>
    /// Check weather this weather tag value matches given object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>True if has same values. False otherwise. Returns false if any of the values are null.</returns>
    public override bool Equals(object? obj)
    {
        return obj is VersionTag tag && Equals(tag);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Major, Minor, Build, VersionId);
    }

    /// <summary>
    /// Get current version object in format vX.X.X, where X is any number 
    /// <para/>Can also include preview tags like -beta, -alpha or -preview
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return FullVersion.ToString();
    }


    // Relational operators

    /// <summary>
    /// Check weather tag values are the same
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>true if all field values are the same, otherwise false. Returns false if any of the arguments are null</returns>
    public static bool operator ==(VersionTag? left, VersionTag? right)
    {
        return left is VersionTag tag && tag.Equals(right);
    }

    /// <summary>
    /// Check weather tag values are the different 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>True if any of the values between tags are different or any of the tags are null. Otherwise false.</returns>
    public static bool operator !=(VersionTag? left, VersionTag? right)
    {
        return left is null
            || right is null
            || !(left == right);
    }

    /// <summary>
    /// Checks weather left side version is bigger than right side. Compare order: Major > Minor > Build > VersionId
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>true if left side is bigger or right side is null, otherwise false</returns>
    public static bool operator >(VersionTag? left, VersionTag? right)
    {
        if (left is null) return false;
        if (right is null) return true;
        if (left.Major > right.Major) return true;
        if (left.Minor > right.Minor) return true;
        if (left.Build > right.Build) return true;
        if (left.VersionId < right.VersionId) return true;
        return false;
    }

    /// <summary>
    /// Checks if right side version is bigger than right side. Compare order: Major > Minor > Build
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>true if right side is bigger or left null, else false</returns>
    public static bool operator <(VersionTag? left, VersionTag? right)
    {
        return left != right && right > left;
    }

    /// <summary>
    /// Checks if right side version is bigger than left side or same. Compare order: Major > Minor > Build
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>true if right side is bigger or left null or both are same, else false</returns>
    public static bool operator <=(VersionTag? left, VersionTag? right)
    {
        return left == right || left < right;
    }

    /// <summary>
    /// Checks if left side version is bigger than right side or same. Compare order: Major > Minor > Build
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>true if left side is bigger or right null or both are same, else false</returns>
    public static bool operator >=(VersionTag? left, VersionTag? right)
    {
        return left == right || left > right;
    }




    /// <summary>
    /// Parse version to int[3] array (default {0,0,0})
    /// </summary>
    /// <param name="versionString"></param>
    /// <returns>int[3] of version number vX.X.X where X is int</returns>
    /// <exception cref="ArgumentException"></exception>
    private void FromString(string versionString)
    {
        if (versionString.StartsWith('v'))
        {
            versionString = versionString.Remove(0, 1);
        }

        (versionString, VersionId) = CheckAndRemoveSpecialIds(versionString);

        var versionNumbers = versionString.Split(".");
        var result = new uint[] { 0, 0, 0 };

        for (var i = 0; i < result.Length; i++)
        {
            try
            {
                result[i] = uint.Parse(versionNumbers[i]);
            }
            catch (IndexOutOfRangeException ex)
            {
                if (i < 1)
                {
                    throw new ArgumentException($"{nameof(versionString)} does not have any version numbers (it is not format \"vX.X.X\")", nameof(versionString), ex);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    $"version string is not right format 'vX.X.X-VersionId' ('v' and VersionId are optional). was given '{versionString}'", nameof(versionString), ex);
            }

        }
        Major = result[0];
        Minor = result[1];
        Build = result[2];
    }

    /// <summary>
    /// Get and remove special ids, in version string (removes also "-" -symbols)
    /// </summary>
    /// <param name="version"></param>
    /// <returns>trimmed version string and fitting VersionId</returns>
    private static (string versionString, VersionId id) CheckAndRemoveSpecialIds(string version)
    {
        var ids = Enum.GetNames(typeof(VersionId))
            .Select(s => s.ToLower())
            .ToArray();

        version = version.ToLower();
        VersionId versionId = VersionId.Full;
        for (var i = 0; i < ids.Length; i++)
        {
            int index = version.IndexOf(ids[i]);
            if (index > -1)
            {
                versionId = (VersionId)i;
                int removalIndex = index;
                if (index is not 0 && version[index - 1] is '-')
                {
                    removalIndex = index - 1;
                }
                version = version[..removalIndex] + version[(index + versionId.ToString().Length)..];
            }
        }
        return (version, versionId);
    }
}
