using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MekUpdater.GithubClient.ApiResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MekUpdater.GithubClient.FileManagers;

/// <summary>
/// Find and launch your setup files
/// </summary>
public class SetupLauncher
{
    /// <summary>
    /// New SetupLauncher with given base folder, no logging.
    /// </summary>
    /// <param name="baseFolder"></param>
    public SetupLauncher(FolderPath? baseFolder) : this(baseFolder, NullLogger.Instance)
    {
        BaseFolder = baseFolder;
    }

    /// <summary>
    /// New SetupLauncher with given base folder and logging.
    /// </summary>
    /// <param name="baseFolder"></param>
    /// <param name="logger"></param>
    public SetupLauncher(FolderPath? baseFolder, ILogger logger)
    {
        BaseFolder = baseFolder;
        Logger = logger;
    }

    /// <summary>
    /// Folder or folder whose subfolders contain setup setupFromFolder
    /// </summary>
    public FolderPath? BaseFolder { get; }

    string[] _nameParams = { "setup", "exe" };

    /// <summary>
    /// Array of substrings that must be found in setup setupFromFolder name. Not case sensitive 
    /// <para/>By default { "setup", "exe" } => (setupFromFolder name must include substring "setup")
    /// </summary>
    public string[] NameParams
    {
        get => _nameParams;
        set => _nameParams = value.Select(x => x.ToLower()).ToArray();
    }

    /// <summary>
    /// Setup file location if it is already found
    /// </summary>
    public SetupExePath? SetupExePath { get; protected set; }
    
    private ILogger Logger { get; }



    /// <summary>
    /// Launch setup. Try find setup file if it isn't already defined.
    /// </summary>
    /// <param name="requireAdmin"></param>
    /// <returns>FileSystemResult of SetupExePath representing used path and action status</returns>
    public FileSystemResult<SetupExePath> LaunchSetup(bool requireAdmin = false)
    {
        var path = SetupExePath;
        if (path is null)
        {
            var result = TryFindSetup();
            if (result.ResultPath?.PathExist is not true)
            {
                return result;
            }
            path = result.ResultPath;
        }

        var launcher = new FileLauncher(path, Logger);
        var launchResult = launcher.LaunchProcess(requireAdmin);
        return new(launchResult.ResponseMessage)
        {
            ResultPath = path,
            Message = launchResult.Message
        };

    }

    /// <summary>
    /// Try find setup setupFromFolder from base folder or one of its subfolders
    /// </summary>
    /// <returns>FileSystemResult of SetupExePath that represents used path and action status</returns>
    public FileSystemResult<SetupExePath> TryFindSetup()
    {
        SetupExePath? result = TryEnumerateFilesAndFolders(BaseFolder?.FullPath);
        if (result?.PathExist is true)
        {
            return new(ResponseMessage.Success, result);
        }
        return new(ResponseMessage.CannotFindSetup);
    }

    private SetupExePath? TryEnumerateFilesAndFolders(string? folderPath)
    {
        if (folderPath is null)
        {
            return null;
        }

        try
        {
            var setupFromFolder = Directory.EnumerateFiles(folderPath)
                .Where(x => ContainsAll(x, NameParams))
                .Select(x => TryParse(x))
                .FirstOrDefault(x => x is not null);
            if (setupFromFolder is not null)
            {
                return setupFromFolder;
            }
            return Directory.EnumerateDirectories(folderPath)
                .Select(x => TryEnumerateFilesAndFolders(x))
                .FirstOrDefault(x => x is not null);
        }
        catch
        {
            return null;
        }
    }

    private static bool ContainsAll(string str, params string[] args)
    {
        string filename;
        // only compare file names and args
        try
        {
            filename = Path.GetFileName(str).ToLower();
        }
        catch
        {
            return false;
        }
        foreach (string arg in args)
        {
            if (filename.Contains(arg) is false)
            {
                return false;
            }
        }
        return true;
    }

    private static SetupExePath? TryParse(string path)
    {
        try
        {
            return new(path);
        }
        catch (ArgumentException)
        {
            return null;
        }
    }



}
