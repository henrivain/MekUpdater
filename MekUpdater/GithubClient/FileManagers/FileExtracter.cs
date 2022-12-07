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
    /// <param name="filePath"></param>
    public FileExtracter(ZipPath filePath) : this(filePath, filePath.FolderPath, NullLogger.Instance) { }

    /// <summary>
    /// New FileExtracter with given zip path and logging. 
    /// Destination path is zip path's folder path.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="logger"></param>
    public FileExtracter(ZipPath filePath, ILogger logger) : this(filePath, filePath.FolderPath, logger) { }

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
    /// Folder where zip file is extracted
    /// </summary>
    public FolderPath DestinationFolder => DestinationFolderHandler.TargetFolder;

    /// <summary>
    /// Extract ZipPath into destination folder.
    /// </summary>
    /// <returns>(true, "") if operation successful, otherwise (false; errorMessage)</returns>
    public async Task<ExtractionPathResult> ExtractAsync(bool overwrite = true)
    {
        Logger.LogInformation("Extract zip file '{zipPath}' to folder '{folderPath}'.",
            ZipPath.FullPath, DestinationFolder.FullPath);
        var (dirCreateValid, dirCreateMsg) = DestinationFolderHandler.TryCreateDirectory();
        if (dirCreateValid is false)
        {
            Logger.LogError("Cannot extract zip file, unable to create directory '{path}'.",
                DestinationFolder.FullPath);
            return new(ResponseMessage.ExtractionError, dirCreateMsg);
        }

        if (ZipPath.PathExist is false)
        {
            Logger.LogError("Cannot extract zip file '{path}' because it does not exist.", ZipPath);
            return new(ResponseMessage.ExtractionError, $"Cannot extract zip file '{ZipPath}' because it does not exist.");
        }
        try
        {
            await Task.Run(() =>
            {
                ZipFile.ExtractToDirectory(ZipPath.FullPath, DestinationFolder.FullPath, overwrite);
            });
            Logger.LogInformation("Zip file extracted successfully.");
            return new(ResponseMessage.Success)
            {
                ResultFolder = DestinationFolder.AppendSingleFolder(ZipPath.FileName)
            };
        }
        catch (Exception ex)
        {
            string errorMessage = ex switch
            {
                ArgumentException => "Zip path or destination was invalid or empty.",
                PathTooLongException => "Zip or destination path was too long.",
                DirectoryNotFoundException => "Directory for zip or destination path should exit, but it doesn't.",
                FileNotFoundException => $"Zip file '{ZipPath}' does not exist.",
                InvalidDataException => $"Zip file at '{ZipPath}' isn't zip file or it is corrupted.",
                IOException => "Invalid zip or destination path or I/O error.",
                UnauthorizedAccessException => "No requires permission",
                NotSupportedException => "Invalid zip or destination path format.",
                _ => $"Unknown zip extraction error in {nameof(FileExtracter)}, ex: '{ex.GetType()}', '{ex.Message}'"
            };
            Logger.LogError("Failed to extract zip file, because of {errorMessage}", errorMessage);
            return new(ResponseMessage.ExtractionError, $"Cannot extract zip. {errorMessage}.");
        }
    }
}
