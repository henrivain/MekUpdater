/// Copyright 2021 Henri Vainio 
using System;
using System.IO;

namespace MekUpdater.ValueTypes;

public class FolderPath : LocalPath
{

    /// <summary>
    /// Create new FolderPath    
    /// </summary>
    public FolderPath() { }

    /// <summary>
    /// Create path to folder/directory by removing filename from given path
    /// </summary>
    /// <param name="path"></param>
    /// <exception cref="ArgumentException"></exception>
    public FolderPath(string path) 
    {
        FullPath = path;
    }






    public override string FullPath
    {
        get => base.FullPath;
        set
        {
            string path = FromString(value);
            if (string.IsNullOrWhiteSpace(path)) 
                throw new ArgumentException($"Given path is not valid ; was given {value}");
            base.FullPath = FromString(path);
        }
    }

    /// <summary>
    /// Checks weather current inctance is valid folder path
    /// </summary>
    /// <returns>true if is valid, else false</returns>
    public override bool IsValid()
    {
        return IsValidFolderPath(this);
    }



    /// <summary>
    /// Convert string to right format
    /// </summary>
    /// <param name="path"></param>
    /// <returns>full path to folder if path valid, else string.Empty</returns>
    private static string FromString(string path)
    {
        string full = GetFullPath(TryReadDirectoryPath(path));
        return (string.IsNullOrWhiteSpace(full)) ? string.Empty : full;
    }

    /// <summary>
    /// Get path to last directory from given path (remove filename if needed).
    /// Writes all exceptions to console.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>path to directory or string.Empty if path not valid</returns>
    private static string TryReadDirectoryPath(string path)
    {
        try
        {
            return Path.GetDirectoryName(path) ?? string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}\n{path}");
            return string.Empty;
        }
    }
}
