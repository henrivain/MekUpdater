using System.IO.Compression;
using MekUpdater.GithubClient.ApiResults;
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
    /// <param name="zipPath"></param>
    public FileExtracter(ZipPath zipPath) : this(zipPath, NullLogger.Instance) { }

    /// <summary>
    /// New FileExtracter with given zip path and logging. 
    /// Destination path is zip path's folder path.
    /// </summary>
    /// <param name="zipPath"></param>
    /// <param name="logger"></param>
    public FileExtracter(ZipPath zipPath, ILogger logger) 
        : this(zipPath, zipPath.FolderPath.AppendSingleFolder(zipPath.FileName), logger) { }

    /// <summary>
    /// New FileExtracter with given zip path, destination folder and logging.
    /// </summary>
    /// <param name="zipPath"></param>
    /// <param name="destinationFolder"></param>
    /// <param name="logger"></param>
    public FileExtracter(ZipPath zipPath, FolderPath destinationFolder, ILogger logger)
    {
        ZipPath = zipPath;
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
    /// Folder where zip file is extracted
    /// </summary>
    public FolderPath DestinationFolder => DestinationFolderHandler.TargetFolder;

    /// <summary>
    /// Extract ZipPath into destination folder.
    /// </summary>
    /// <returns>(true, "") if operation successful, otherwise (false; errorMessage)</returns>
    public async Task<FileSystemResult<FolderPath>> ExtractAsync(bool overwrite = true)
    {
        Logger.LogInformation("Extract zip file '{zipPath}' to folder '{folderPath}'.",
            ZipPath.FullPath, DestinationFolder.FullPath);
        var dirResult = DestinationFolderHandler.TryCreateDirectory();
        if (dirResult.NotSuccess())
        {
            Logger.LogError("Cannot extract zip file, '{reason}'", dirResult.Message);
            return dirResult;
        }
        if (ZipPath.PathExist is false)
        {
            Logger.LogError("Cannot extract zip file '{path}' that does not exist.", ZipPath);
            return new(ResponseMessage.ExtractionError, $"Cannot extract zip file '{ZipPath}' because it does not exist.");
        }

        string? errorMessage = await Task.Run(() => Extract(overwrite));
        if (errorMessage is not null) 
        {
            Logger.LogError("{msg}", errorMessage);
            return new(ResponseMessage.ExtractionError, errorMessage);
        }

        Logger.LogInformation("Zip file extracted successfully.");
        return new(ResponseMessage.Success)
        {
            ResultPath = DestinationFolder
        };
    }
    
    /// <summary>
    /// Extract file into given path. 
    /// </summary>
    /// <param name="overwrite"></param>
    /// <returns>null if success, otherwise error message.</returns>
    private protected string? Extract(bool overwrite = false)
    {
        try
        {
            ZipFile.ExtractToDirectory(ZipPath.FullPath, DestinationFolder.FullPath, overwrite);
            return null;
        }
        catch(Exception ex)
        {
            string errorMessage = "Cannot extract zip. ";
            errorMessage += ex switch
            {
                ArgumentException => "Zip path or destination was invalid or empty.",
                PathTooLongException => "Zip or destination path was too long.",
                DirectoryNotFoundException => "Directory for zip or destination path should exit, but it doesn't.",
                FileNotFoundException => $"Zip file '{ZipPath}' does not exist.",
                InvalidDataException => $"Zip file at '{ZipPath}' isn't zip file or it is corrupted.",
                IOException => "Invalid zip or destination path or I/O error (If overwrite was set to false, file might already exist).",
                UnauthorizedAccessException => "No requires permission",
                NotSupportedException => "Invalid zip or destination path format.",
                _ => $"Unknown zip extraction error in {nameof(FileExtracter)}, ex: '{ex.GetType()}', '{ex.Message}'"
            };
            return errorMessage;
        }
    }
}
