using MekUpdater.GithubClient;
using MekUpdater.GithubClient.ApiResults;
using MekUpdater.GithubClient.FileManagers;
using MekUpdater.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SetupLauncher = MekUpdater.GithubClient.FileManagers.SetupLauncher;

namespace MekUpdater;

/// <summary>
/// Main API of MekUdpater
/// </summary>
public class UpdaterAPI : IDisposable
{

    /// <summary>
    /// New UpdaterAPI with no logging. Note that this class implements IDisposable.
    /// </summary>
    /// <param name="githubUsername">Github username of the repository owner.</param>
    /// <param name="repositoryName">Name of the github repository.</param>
    public UpdaterAPI(string githubUsername, string repositoryName)
        : this(githubUsername, repositoryName, NullLogger<GithubApiClient>.Instance) { }

    /// <summary>
    /// New UpdaterAPI with logging. Note that this class implements IDisposable.
    /// </summary>
    /// <param name="githubUsername">Github username of the repository owner.</param>
    /// <param name="repositoryName">Name of the github repository.</param>
    /// <param name="logger">Logger to be used.</param>
    public UpdaterAPI(string githubUsername, string repositoryName, ILogger<GithubApiClient> logger)
    {
        DownloadClient = new GithubDownloadClient(githubUsername, repositoryName, logger);
        Logger = logger;

        FolderPath appData = WindowsPath.AppDataFolder;
        CacheDirectory = WindowsPath.AppDataFolder;
        CacheDirectory.Append($@"{nameof(MekUpdater)}\Cache\");
    }


    private IGithubDownloadClient DownloadClient { get; }
    private IGithubInfoClient InfoClient => DownloadClient.InfoClient;
    private ILogger Logger { get; }
    private FolderPath CacheDirectory { get; init; }



    /// <summary>
    /// Download latest released and launch setup.
    /// </summary>
    /// <param name="destination"></param>
    /// <returns>FileSystemResult of SetupExePath representing action status and information about process.</returns>
    public async Task<FileSystemResult<SetupExePath>> GetAndLaunchLatestRelease(FolderPath destination)
    {
        return await GetAndLaunchLatestRelease(VersionTag.Min, destination);
    }

    /// <summary>
    /// Download latest released version if it is newer than the current version and launch setup.
    /// </summary>
    /// <param name="current"></param>
    /// <param name="destination"></param>
    /// <returns>FileSystemResult of SetupExePath representing action status and information about process.</returns>
    public async Task<FileSystemResult<SetupExePath>> GetAndLaunchLatestRelease(IVersion current, FolderPath destination)
    {
        var downloaded = await DownloadLatestIfNewer(current);
        if (downloaded.NotSuccess())
        {
            Logger.LogError("Failed to download update.");
            return new(downloaded.ResponseMessage, downloaded.Message);
        }

        var extracted = await Extract(downloaded.ResultPath, destination);
        if (extracted.NotSuccess())
        {
            Logger.LogError("Failed to extract update.");
            return new(extracted.ResponseMessage, extracted.Message);
        }

        var launched = Launch(extracted.ResultPath);
        if (launched.NotSuccess())
        {
            Logger.LogError("Failed to launch setup");
            return launched;
        }
        Logger.LogInformation("Successfully downloaded and launched setup file from latest release.");
        return launched;
    }

    /// <summary>
    /// Download and extract latest release to given folder.
    /// </summary>
    /// <param name="destination"></param>
    /// <returns>FileSystemResult of FolderPath representing action status and information about process</returns>
    public async Task<FileSystemResult<FolderPath>> GetLatestRelease(FolderPath destination)
    {
        return await GetLatestRelease(VersionTag.Min, destination);
    }

    /// <summary>
    /// Download and extract latest release to given folder if it is newer than the current version.
    /// </summary>
    /// <param name="current"></param>
    /// <param name="destination"></param>
    /// <returns>FileSystemResult of FolderPath representing action status and information about process</returns>
    public async Task<FileSystemResult<FolderPath>> GetLatestRelease(IVersion current, FolderPath destination)
    {
        var downloaded = await DownloadLatestIfNewer(current);
        if (downloaded.NotSuccess())
        {
            return new(downloaded.ResponseMessage)
            {
                Message = downloaded.Message,
                ResultPath = downloaded.ResultPath?.FolderPath
            };
        }
        return await Extract(downloaded.ResultPath, destination);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        DownloadClient?.Dispose();
        GC.SuppressFinalize(this);
    }


    private async Task<DownloadResult<ZipPath>> DownloadLatestIfNewer(IVersion current)
    {
        Logger.LogInformation("Get release info for latest release.");
        var infoResult = await InfoClient.GetLatestRelease();
        if (infoResult.NotSuccess())
        {
            Logger.LogError("Failed to check for updates.");
            return new(infoResult.ResponseMessage)
            {
                Message = infoResult.Message
            };
        }
        if (VersionTag.TryParse(infoResult.Content?.TagName, out var latest) is false)
        {
            Logger.LogInformation("Cannot parse version from string '{version}'", infoResult.Content?.TagName);
            return new(ResponseMessage.InvalidVersion)
            {
                Message = $"Cannot parse version from string '{infoResult.Content?.TagName}'."
            };
        }
        if (latest.CompareTo(current) <= 0)
        {
            Logger.LogInformation("Current version '{current}' is same or newer than latest {latest}.", 
                current.ToString(), latest.ToString());
            return new(ResponseMessage.UpdateAlreadyInstalled)
            {
                Message = $"Current version '{current}' is same or newer than latest {latest}."
            };
        }
        return await DownloadClient.DownloadLatestReleaseZip(CacheDirectory);
    }
    private async Task<FileSystemResult<FolderPath>> Extract(ZipPath? source, FolderPath destination)
    {
        Logger.LogInformation("Extract file.");
        if (source is null)
        {
            Logger.LogError("Source zip file is null.");
            return new(ResponseMessage.CannotFindFile)
            {
                Message = "Source zip file is null.",
                ResultPath = destination
            };
        }
        var fileExtracter = new FileExtracter(source, destination, Logger);
        var result = await fileExtracter.ExtractAsync();
        if (result.NotSuccess())
        {
            Logger.LogError("Failed to extract zip file at '{path}'.", result.ResultPath?.FullPath);
            return result;
        }
        Logger.LogInformation("Extracted file successfully.");
        return result;
    }
    private FileSystemResult<SetupExePath> Launch(FolderPath? setupContainerFolder)
    {
        Logger.LogInformation("Launch setup.");
        var result = new SetupLauncher(setupContainerFolder, Logger).LaunchSetup();
        if (result.NotSuccess())
        {
            Logger.LogError("Setup launch not successful.");
            return result;
        }
        Logger.LogInformation("Setup launch successful.");
        return result;
    }

    private async Task<FileSystemResult<ZipPath>> RemoveUselessZipFileAsync(ZipPath? path)
    {
        Logger.LogInformation("Delete useless zip file.");
        if (path?.PathExist is not true)
        {
            Logger.LogWarning("File at '{path}' was probably already deleted.", path);
            return new(ResponseMessage.Success)
            {
                Message = "File is probably already deleted.",
                ResultPath = path
            };
        }
        var handler = new FileHandler(path);
        return await handler.DeleteAsync();
        
        
    }
    

}
