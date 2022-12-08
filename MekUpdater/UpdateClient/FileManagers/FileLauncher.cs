using System.ComponentModel;
using System.Diagnostics;
using MekUpdater.GithubClient.ApiResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MekUpdater.GithubClient.FileManagers;

/// <summary>
/// Launch new processes from file system
/// </summary>
public class FileLauncher
{
    /// <summary>
    /// New Filelauncher with given target file, no logging.
    /// </summary>
    /// <param name="file"></param>
    public FileLauncher(IFilePath file) : this(file, NullLogger.Instance) { }

    /// <summary>
    /// New Filelauncher with given target file and logging.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="logger"></param>
    public FileLauncher(IFilePath file, ILogger logger)
    {
        File = file;
        Logger = logger;
    }

    /// <summary>
    /// Target file
    /// </summary>
    public IFilePath File { get; }

    private ILogger Logger { get; }


    /// <summary>
    /// Launch process from from given file.
    /// </summary>
    /// <param name="requireAdmin">Does process require admin permissions to run?</param>
    public FileSystemResult<IFilePath> LaunchProcess(bool requireAdmin = false)
    {
        Logger.LogInformation("Launch process at '{path}', require admin '{admin}'", 
            File.FullPath, requireAdmin);

        if (File.PathExist is false)
        {
            Logger.LogError("Cannot launch process because file '{path}' is not defined", File.FullPath);
            return new(ResponseMessage.CannotLaunchProcess, $"File '{File}' doesn't exist.", File);
        }
        try
        {
            Process process = new()
            {
                StartInfo = new()
                {
                    FileName = File.FullPath,
                    UseShellExecute = false,
                    Verb = requireAdmin ? "runas" : string.Empty
                }
            };
            process.Start();
            Logger.LogInformation("Process launched successfully.");
            return new(ResponseMessage.Success, File);
        }
        catch (Exception ex)
        {
            string error = ex switch
            {
                ObjectDisposedException => "Process was disposed too early.",
                InvalidOperationException => "Cannot launch process from file that doens't exist.",
                Win32Exception => "Error whilst launching file, win32 error.",
                PlatformNotSupportedException => "Cannot launch process, unsupported operating system.",
                _ => $"Unknown process launch exception at '{nameof(FileLauncher)}'"
            };
            Logger.LogError("Cannot launch file");
            return new(ResponseMessage.CannotLaunchProcess, error, File);
        }
    }
}
