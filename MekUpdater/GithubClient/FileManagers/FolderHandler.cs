using MekUpdater.GithubClient.ApiResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MekUpdater.GithubClient.FileManagers;

/// <summary>
/// Manage folders with FolderHandler.
/// </summary>
internal class FolderHandler
{
    /// <summary>
    /// New FolderHandler to manage folder, no logging.
    /// </summary>
    /// <param name="filePath"></param>
    internal FolderHandler(FolderPath filePath) : this(filePath, NullLogger.Instance) { }

    /// <summary>
    /// New FileHandler to create files and copy data to them. Uses given logger.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="logger"></param>
    internal FolderHandler(FolderPath filePath, ILogger logger)
    {
        TargetFolder = filePath;
        Logger = logger;
    }

    /// <summary>
    /// Logger used if initialized
    /// </summary>
    internal ILogger Logger { get; init; } = NullLogger.Instance;

    /// <summary>
    /// ResultPath to file with class is working with.
    /// </summary>
    internal FolderPath TargetFolder { get; }

    /// <summary>
    /// Create full path to folder, if it doesn't already exist.
    /// </summary>
    /// <returns>(true, "") if valid operation or directory exist, (false, errorMessage) if failed.</returns>
    internal virtual FileSystemResult<FolderPath> TryCreateDirectory()
    {
        if (TargetFolder.PathExist)
        {
            return new(ResponseMessage.Success, TargetFolder);
        }

        Logger.LogInformation("Try to create directory '{path}'.", TargetFolder.FullPath);
        try
        {
            Directory.CreateDirectory(TargetFolder.FullPath);
            Logger.LogInformation("Directory created successfully.");
            return new(ResponseMessage.Success, TargetFolder);
        }
        catch (Exception ex)
        {
            string failReason = ex switch
            {
                PathTooLongException => $"Too long path '{TargetFolder}'.",
                IOException => $"Specified directory, '{TargetFolder}', is file, not directory.",
                UnauthorizedAccessException => $"Cannot create directory '{TargetFolder}', lacking required permissions.",
                ArgumentException => $"Bad direcotry path '{TargetFolder}'.",
                NotSupportedException => $"Path '{TargetFolder}' has invalid colon (':').",
                _ => $"Unknown directory creation exception in {nameof(FileHandler)}, ex: '{ex.GetType()}', message: {ex.Message}."
            };
            Logger.LogWarning("Cannot create directory '{dirPath}' because of '{ex}': '{failreason}'.",
                TargetFolder.FullPath, ex.GetType(), failReason);
            return new(ResponseMessage.CannotCreateDirectory, failReason);
        }
    }
}
