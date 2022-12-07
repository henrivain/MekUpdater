using System.IO.Compression;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MekUpdater.GithubClient.FileManagers;

/// <summary>
/// Extract zip files into folders.
/// </summary>
public class FileExtracter
{
    /// <summary>
    /// New FileExtracter that extracts files into zip file containing folder by default. No logging.
    /// </summary>
    /// <param name="filePath"></param>
    public FileExtracter(ZipPath filePath) : this(filePath, filePath.FolderPath, NullLogger.Instance) { }

    /// <summary>
    /// New FileExtracter with given zip path, destination folder and logging.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="destinationFolder"></param>
    /// <param name="logger"></param>
    public FileExtracter(ZipPath filePath, FolderPath destinationFolder, ILogger logger) 
    {
        ZipPath = filePath;
        DestinationFolderHandler = new(destinationFolder, logger);
        Logger = logger;
    }

    
    private FolderHandler DestinationFolderHandler { get; }
    private ILogger Logger { get; }

    /// <summary>
    /// File to be extracted
    /// </summary>
    public ZipPath ZipPath { get; }

    /// <summary>
    /// Extract ZipPath into destination folder.
    /// </summary>
    /// <returns>(true, "") if operation successful, otherwise (false; errorMessage)</returns>
    public async Task<(bool Valid, string Msg)> ExtractAsync()
    {
        var dirCreateResult = DestinationFolderHandler.TryCreateDirectory();
        if (dirCreateResult.Valid is false)
        {
            Logger.LogError("Unable to create {path}, cannot extract zip file.", 
                DestinationFolderHandler.TargetFolder.FullPath);
            return dirCreateResult;
        }

        if (ZipPath.PathExist is false)
        {
            Logger.LogError("Cannot extract zip file '{path}' that does not exist.", ZipPath);
            return (false, $"Zip file '{ZipPath}' does not exist, cannot extract.");
        }
        try
        {
            await Task.Run(() =>
            {
                ZipFile.ExtractToDirectory(ZipPath.FullPath, DestinationFolderHandler.TargetFolder.FullPath);
            });
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            string errorMessage = ex switch
            {
                _ => $"Unknown zip extraction error in {nameof(FileExtracter)}, ex: '{ex.GetType()}', '{ex.Message}'"
            };

            return (false, errorMessage);
        }
    }
}
