using System.IO;
using System.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MekUpdater.GithubClient.FileManager;

internal class FileHandler
{
    internal FileHandler(IFilePath filePath)
    {
        FilePath = filePath;
    }

    internal FileHandler(IFilePath filePath, ILogger logger) : this(filePath)
    {
        Logger = logger;
    }

    internal ILogger Logger { get; } = NullLogger.Instance;

    internal IFilePath FilePath { get; }

    internal (bool Valid, string Msg) TryCreateDirectory()
    {
        if (FilePath.PathExist) return (true, string.Empty);
        if (Directory.Exists(FilePath.FolderPath.FullPath)) return (true, string.Empty);
        Logger.LogInformation("Try to create path '{path}'", FilePath);
        try
        {
            Directory.CreateDirectory(FilePath.FolderPath.FullPath);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            string failReason = ex switch
            {
                PathTooLongException => $"Too long path '{FilePath.FolderPath}'.",
                IOException => $"Specified directory, '{FilePath.FolderPath}', is file, not directory.",
                UnauthorizedAccessException => $"Cannot create directory '{FilePath.FolderPath}', no required permissions.",
                ArgumentException => $"Bad direcotry path '{FilePath.FolderPath}'.",
                NotSupportedException => $"Path '{FilePath.FolderPath}' has invalid colon (':').",
                _ => $"Unknown directory creation exception in {nameof(FileHandler)}, ex: '{ex.GetType()}', message: {ex.Message}."
            };
            Logger.LogError("Cannot create directory '{dirPath}' because of '{ex}': '{failreason}'.", 
                FilePath.FolderPath , ex.GetType(), failReason);
            return (false, failReason);
        }
    }

    internal async Task<(bool Valid, string Msg)> CopyStreamAsync(Stream? stream)
    {
        if (stream is null)
        {

            return (false, "Cannot copy stream that is null.");
        }
        try 
        {
            using var fileStream = new FileStream(FilePath.FullPath, FileMode.OpenOrCreate);
            if (fileStream is null)
            {
                return (false, $"Cannot initialize filesteam at path {FilePath}, " +
                    $"stream cannot be copied to destination");
            }
            await stream.CopyToAsync(fileStream);
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
            return (false, failReason);
        }
    }
}
