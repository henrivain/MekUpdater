/// Copyright 2021 Henri Vainio 
using System;
using System.IO;

namespace MekUpdater.ValueTypes;

public class LocalPath : ILocalPath
{
    /// <summary>
    /// Initialize new local path with path currently not identified
    /// </summary>
    public LocalPath() { }

    /// <summary>
    /// Initialize new local path with path given
    /// </summary>
    /// <param name="path"></param>
    /// <exception cref="ArgumentException"></exception>
    public LocalPath(string path) 
    { 
        FullPath = path;
    }

    string _fullPath = string.Empty;



    /// <summary>
    /// Validated full path, if no valid, throws ArgumentException
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public virtual string FullPath 
    { 
        get => _fullPath; 
        set 
        {
            _fullPath = GetFullPath(value);
            if (IsValidWindowsPath(this) is false) 
                throw new ArgumentException($"Given path is not valid ; was given {value}");
        }
    }
    
    /// <summary>
    /// Check if instance of object is valid
    /// </summary>
    /// <returns></returns>
    public virtual bool IsValid()
    {
        Path.GetInvalidFileNameChars();
        return (IsValidWindowsPath(this));
    }

    /// <summary>
    /// Check weather instance of object has path value
    /// </summary>
    /// <returns>true if has value (value not string.Empty), else false</returns>
    public virtual bool HasValue()
    {
        return string.IsNullOrEmpty(FullPath) is false;
    }

    /// <summary>
    /// Check weather path.FullPath is valid in windows
    /// </summary>
    /// <returns>true if is valid, else false</returns>
    protected static bool IsValidWindowsPath(ILocalPath path)
    {
        if (GetFullPath(path.FullPath) == string.Empty) return false;
        if (string.IsNullOrWhiteSpace(path.FullPath)) return false;
        if (HasInvalidChars(path.FullPath)) return false;
        return (Path.IsPathFullyQualified(path.FullPath));
    }

    /// <summary>
    /// Check weather FullPath is valid path to folder (does not have file extension) in windows
    /// </summary>
    /// <returns>true if is valid, else false</returns>
    protected static bool IsValidFolderPath(ILocalPath path)
    {
        if (IsValidWindowsPath(path) is false) return false;
        return (string.IsNullOrWhiteSpace(Path.GetExtension(path.FullPath)));
    }

    /// <summary>
    /// Check weather FullPath is valid path to file 
    /// in windows (has file extension and name)
    /// </summary>
    /// <returns>true if is valid, else false</returns>
    protected static bool IsValidFilePath(ILocalPath path)
    {
        if (IsValidWindowsPath(path) is false) return false;
        return (string.IsNullOrWhiteSpace(Path.GetExtension(path.FullPath)) is false);
    }

    /// <summary>
    /// If path has filename, checks weather it has invalid characters or not
    /// </summary>
    /// <param name="path"></param>
    /// <returns>true if has invalid chars or too long, else false</returns>
    protected static bool HasInvalidChars(string path)
    {
        string fileName = Path.GetFileName(path);
        if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1) 
            return true;

        char[] invalidChars = new char[] { '|', '<', '>', '"', '*', '?' };
        return (path.IndexOfAny(invalidChars) != -1);
    }




    public override string ToString()
    {
        return FullPath.ToString();
    }
    public override bool Equals(object? obj)
    {
        return obj is LocalPath path &&
               FullPath == path.FullPath;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(FullPath);
    }

    /// <summary>
    /// Get full path from relative, if bad path, returns string.empty. 
    /// If given path is full path, returns it unchanced.
    /// Writes all exceptions in console.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>full path if given path valid or valid relative path, else string.Empty</returns>
    public static string GetFullPath(string path)
    {
        try
        {
            return Path.GetFullPath(path) ?? string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}\n{path}");
            return string.Empty;
        }
    }



    public bool PathExist()
    {
        throw new NotImplementedException();
    }
}
