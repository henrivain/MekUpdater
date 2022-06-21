/// Copyright 2021 Henri Vainio 

namespace MekUpdater.ValueTypes;

public class ZipPath : LocalPath, IFilePath
{
    /// <summary>
    /// Initialize new ZipPath 
    /// </summary>
    public ZipPath() { }

    /// <summary>
    /// Initialize new ZipPath where given file is situated in 
    /// given folder path. If has no extension or wrong extension, changes to or adds ".zip"
    /// </summary>
    /// <param name="path"></param>
    /// <exception cref="ArgumentException"></exception>
    public ZipPath(string path)
    {
        FullPath = path;
    }

    public static string FileExtension { get; } = ".zip";

    public string FileName { get; private set; } = string.Empty;

    public FolderPath FolderPath { get; private set; } = new();

    /// <summary>
    /// Validated full ".zip" path, if not valid throws ArgumentException
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public override string FullPath
    {
        get => base.FullPath; 
        set
        {
            string path = FromString(value);
            if (string.IsNullOrWhiteSpace(path)) 
                throw new ArgumentException($"Given path is not valid ; was given {value}");
            base.FullPath = value;

            FileName = Path.GetFileNameWithoutExtension(path);
            FolderPath = new FolderPath(path);
        }
    }

    /// <summary>
    /// Checks weather current inctance is valid folder path
    /// </summary>
    /// <returns>true if is valid, else false</returns>
    public override bool IsValid()
    {
        return IsValidFilePath(this) && FullPath.Contains(FileExtension);
    }



    /// <summary>
    /// Get full path, add extension and validate path
    /// </summary>
    /// <param name="path"></param>
    /// <returns>valid path or string.Empty if not valid</returns>
    private static string FromString(string path)
    {
        string full = AddZipExtension(GetFullPath(path));
        return (string.IsNullOrWhiteSpace(full)) ? string.Empty : full;
    }
    
    /// <summary>
    /// Add ".zip" extension to file if needed
    /// </summary>
    /// <param name="path"></param>
    /// <returns>path with ".zip" extension valid, else string.Empty</returns>
    private static string AddZipExtension(string path)
    {
        return Path.ChangeExtension(path, FileExtension) ?? string.Empty;
    }
}
