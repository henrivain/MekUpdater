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
    /// New UpdaterAPI with no logging
    /// </summary>
    /// <param name="githubUsername">Github username of the repository owner.</param>
    /// <param name="repositoryName">Name of the github repository.</param>
    public UpdaterAPI(string githubUsername, string repositoryName)
        : this(githubUsername, repositoryName, NullLogger<GithubApiClient>.Instance) { }

    /// <summary>
    /// New UpdaterAPI with logging
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
    private FolderPath CacheDirectory { get; }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentVersion"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public async Task<UpdaterApiResult> GetLatest(IVersion currentVersion, FolderPath destination)
    {
        Logger.LogInformation("Get release info for latest release.");
        var infoResult = await InfoClient.GetLatestRelease();
        if (infoResult.NotSuccess())
        {
            Logger.LogError("Failed to check for updates.");
            return infoResult;
        }
        if (infoResult.Content?.TagName is null)
        {
            Logger.LogError("Latest version is not defined in the release.");
            return new(infoResult.ResponseMessage, infoResult.Message);
        }
        bool canParseVersion = VersionTag.TryParse(infoResult.Content.TagName, out VersionTag? latestVersion);
        if (canParseVersion is false)
        {
            Logger.LogInformation("Cannot parse version from string {version}", infoResult.Content.TagName);
            return new(ResponseMessage.InvalidVersion)
            {
                Message = $"Cannot parse version from string '{infoResult.Content.TagName}'."
            };
        }
        if (latestVersion.CompareTo(currentVersion) <= 0)
        {
            Logger.LogInformation("Latest version is not newer or same as current version.");
            return new(ResponseMessage.UpdateAlreadyInstalled)
            {
                Message = $"Current version '{currentVersion}' is same of newer as latest '{latestVersion}'."
            };
        }


        Logger.LogInformation("Download update.");
        var downloadResult = await DownloadClient.DownloadReleaseZip(latestVersion, destination);
        if (downloadResult.NotSuccess())
        {
            Logger.LogError("Failed to download update.");
            return downloadResult;
        }
        if (downloadResult.ResultPath?.PathExist is not true)
        {
            Logger.LogError("Download path is null.");
            return new(ResponseMessage.CannotFindFile)
            {
      
                Message = $"Downloaded zip file does not exist at '{downloadResult.ResultPath}'",
      
            };
        }

        Logger.LogInformation("Extract file.");
        var fileExtracter = new FileExtracter(downloadResult.ResultPath, Logger);
        var extractionResult = await fileExtracter.ExtractAsync();
        if (extractionResult.NotSuccess())
        {
            Logger.LogError("Failed to extract zip file.");
            return extractionResult;
        }
        if (extractionResult?.ResultPath?.PathExist is not true)
        {
            Logger.LogError("Setup file is not defined.");
            return new(ResponseMessage.CannotFindFile)
            {
                Message = $"Setup file does not exist at path '{extractionResult?.ResultPath}'."
            };
        }

        Logger.LogInformation("Start process");
        var setupLauncher = new SetupLauncher(extractionResult.ResultPath, Logger);
        var setupLaunchResult = setupLauncher.LaunchSetup();
        if (setupLaunchResult.NotSuccess())
        {
            Logger.LogError("Setup launch not successful");
            return setupLaunchResult;
        }
        Logger.LogInformation("Setup launch successful");
        return new(ResponseMessage.Success)
        {
            Message = extractionResult.ResultPath.FullPath,
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="destination"></param>
    /// <returns></returns>
    public async Task<UpdaterApiResult> GetLatest(FolderPath destination)
    {
        return await GetLatest(VersionTag.Min, destination);
    }








    /// <inheritdoc/>
    public void Dispose()
    {
        DownloadClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}
