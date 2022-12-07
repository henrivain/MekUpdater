using System.Security;
using MekUpdater.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MekUpdater.GithubClient.FileManagers;

/// <summary>
/// Create files and copy data to them.
/// </summary>
internal class FileHandler
{
    /// <summary>
    /// New FileHandler to create files and copy data to them. No logging.
    /// </summary>
    /// <param name="filePath"></param>
    internal FileHandler(IFilePath filePath)
    {
        FilePath = filePath;
        FolderHandler = new(filePath.FolderPath);
    }

    /// <summary>
    /// New FileHandler to create files and copy data to them. Uses given logger.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="logger"></param>
    internal FileHandler(IFilePath filePath, ILogger logger) : this(filePath)
    {
        Logger = logger;
    }

    /// <summary>
    /// Folder handler used to manage folders.
    /// </summary>
    protected internal virtual FolderHandler FolderHandler { get; }

    /// <summary>
    /// Logger used if initialized
    /// </summary>
    internal virtual ILogger Logger { get; init; } = NullLogger.Instance;

    /// <summary>
    /// Path to file with class is working with.
    /// </summary>
    internal IFilePath FilePath { get; }

    /// <summary>
    /// Create file and full path to it, if doesn't already exit.
    /// </summary>
    /// <returns>(true, "") if valid operation or file exist, otherwise (false, errorMessage).</returns>
    internal virtual (bool Valid, string Msg) TryCreateFile()
    {
        if (FilePath.PathExist) return (true, string.Empty);
        var dirResult = FolderHandler.TryCreateDirectory();
        if (dirResult.Valid is false) return dirResult;
        Logger.LogInformation("");
        try
        {
            File.Create(FilePath.FullPath);
            Logger.LogInformation("File created successfully.");
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            string failReason = ex switch
            {
               UnauthorizedAccessException => $"Cannot create directory '{FilePath}', lacking required permissions.",
               ArgumentException => $"Path '{FilePath}'includes invalid characters.",
               PathTooLongException => $"Path '{FilePath}' is too long.",
               DirectoryNotFoundException => throw new UnreachableException("Directory should always be created before this.", ex),
               IOException => $"I/O error ocurred when creating path '{FilePath}'.",
               NotSupportedException => $"Path '{FilePath}' has invalid format.",
                _ => $"Unknown dile creation exception in {nameof(FileHandler)}, ex: '{ex.GetType()}', message: {ex.Message}."
            };
            Logger.LogWarning("Cannot create file '{dirPath}' because of '{ex}': '{failreason}'.",
                FilePath.FullPath, ex.GetType(), failReason);
            return (false, failReason);
        }
    }

    /// <summary>
    /// Copy stream into TargetFolder async.
    /// </summary>
    /// <param name="stream"></param>
    /// <returns>(true, "") if operation successful, otherwise (false, errorMessage)</returns>
    internal virtual async Task<(bool Valid, string Msg)> WriteStreamAsync(Stream? stream)
    {
        Logger.LogInformation("Start copying stream into file '{path}'.", FilePath.FullPath);
        if (stream is null)
        {
            Logger.LogWarning("Cannot copy stream that is null.");
            return (false, "Cannot copy stream that is null.");
        }
        try 
        {
            using var fileStream = new FileStream(FilePath.FullPath, FileMode.OpenOrCreate);
            if (fileStream is null)
            {
                Logger.LogWarning("Cannot copy stream in the file stream '{path}' that is null.", FilePath.FullPath);
                return (false, $"Cannot initialize filesteam at path {FilePath}, " +
                    $"stream cannot be copied to destination");
            }
            await stream.CopyToAsync(fileStream);
            Logger.LogInformation("Copied stream into file successfully.");
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            string failReason = ex switch
            {
                ArgumentException => $"Bad path '{FilePath}', cannot copy stream.",
                SecurityException => $"Cannot initialize file stream at '{FilePath}', no required permissions.",
                UnauthorizedAccessException => $"Cannot initialize file stream at '{FilePath}', file is read-only.",
                FileNotFoundException => $"Cannot find file at '{FilePath}'.",
                IOException => $"Cannot copy stream, because it is closed or bad format in path '{FilePath}'",
                ObjectDisposedException => "File stream was disposed too early",
                NotSupportedException => "Current stream does not support reading or destination does not support writing.",
                _ => $"Unknown stream copy exception in {nameof(FileHandler)}, ex: '{ex.GetType()}', message: '{ex.Message}'."
            };
            Logger.LogWarning("Copying stream into file threw exception '{exType}': '{exMsg}'", ex.GetType(), ex.Message);
            return (false, failReason);
        }
    }
}
