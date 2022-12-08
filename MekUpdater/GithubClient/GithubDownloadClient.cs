using MekUpdater.GithubClient.ApiResults;
using MekUpdater.GithubClient.DataModels;
using Microsoft.Extensions.Logging;

namespace MekUpdater.GithubClient;


/// <summary>
/// Api client for github api, download files from api endpoints and handle them locally.
/// </summary>
public class GithubDownloadClient : GithubApiClient, IGithubDownloadClient
{
    IGithubInfoClient _infoClient;

    /// <summary>
    /// Initialize new GithubDownloadClient with no logging
    /// </summary>
    /// <param name="githubUserName"></param>
    /// <param name="githubRepositoryName"></param>
    public GithubDownloadClient(string githubUserName, string githubRepositoryName) 
        : base(githubUserName, githubRepositoryName) 
    { 
        _infoClient = new GithubInfoClient(githubUserName, githubRepositoryName);
    }

    /// <summary>
    /// Initialize new GithubDownloadClient with logging
    /// </summary>
    /// <param name="githubUserName"></param>
    /// <param name="githubRepositoryName"></param>
    /// <param name="logger"></param>
    public GithubDownloadClient(string githubUserName, string githubRepositoryName, ILogger<GithubApiClient> logger) 
        : base(githubUserName, githubRepositoryName, logger) 
    {
        _infoClient = new GithubInfoClient(githubUserName, githubRepositoryName, logger);
    }

    /// <summary>
    /// Client to help getting information about releases
    /// </summary>
    /// <exception cref="ArgumentNullException">When value is null</exception>
    protected internal IGithubInfoClient InfoClient
    {
        get => _infoClient;
        init
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            _infoClient = value;
        }
    }

    /// <summary>
    /// Download latest release zip source code
    /// </summary>
    /// <returns>DownloadResult representing request data and info</returns>
    public async Task<DownloadResult<ZipPath>> DownloadReleaseZip(VersionTag tag, FolderPath destinationFolder)
    {
        string url = $"{BaseAddress}/zipball/{tag.FullVersion}";
        var path = destinationFolder.ToFilePath<ZipPath>($"{tag.FullVersion}.zip");
        Logger.LogInformation("Download file from '{url}' to '{path}'", url, path);
        return await DownloadFileAsync(url, path);
    }

    /// <summary>
    /// Download asset form specific version to given path. 
    /// Downloads first asset from given releasse that has matching name.
    /// </summary>
    /// <param name="tag">Version tag of the release.</param>
    /// <param name="path">ResultPath where asset will be downloaded.</param>
    /// <param name="assetName">Name that will be used to validate that the asset is the right one.</param>
    /// <param name="onlyFullMatch">Specifies weather asset name should fully match assetName or not.</param>
    /// <returns>DownloadResult representing download status and information about download.</returns>
    public async Task<DownloadResult<IFilePath>> DownloadAsset(
        VersionTag tag, FolderPath path, string assetName, bool onlyFullMatch = false)
    {
        Logger.LogInformation("Download asset from release '{tag}'.", tag);
        var release = await InfoClient.GetRelease(tag);
        if (release.ResponseMessage.NotSuccess())
        {
            Logger.LogError("Cannot validate assets, because of '{msg}': '{explanation}'.", 
                release.ResponseMessage, release.Message);
            return new(release.ResponseMessage, release.Message);
        }

        Func<Asset, bool> validationAction = x => x.Name?.Contains(assetName ?? string.Empty) is true;
        if (onlyFullMatch)
        {
            validationAction = x => x.Name == assetName;
        }

        var asset = release.Content?.Assets.FirstOrDefault(validationAction);
        if (asset is null) 
        {
            Logger.LogError("Cannot download asset from selected release with name '{name}'.", assetName);
            return new(ResponseMessage.NoMatchingAssetName,
                $"Cannot download asset from selected release with name '{assetName}'.");
        }
        if (string.IsNullOrWhiteSpace(asset.DownloadUrl))
        {
            Logger.LogError("Asset url cannot be empty");
            return new(ResponseMessage.NoDownloadUrl, $"Cannot download asset with no download url.");
        }
        if (string.IsNullOrWhiteSpace(asset.Name))
        {
            Logger.LogError("Cannot download asset with no file name.");
            return new(ResponseMessage.NoProperFileName, $"Cannot download asset with no file name.");
        }

        Logger.LogInformation("Asset ready to be downloaded from '{url}'.", asset.DownloadUrl);
        IFilePath downloadPath = path.ToFilePath<FilePath>(asset.Name);
        return await DownloadFileAsync(asset.DownloadUrl, downloadPath);
    }

    /// <summary>
    /// Download latest release into specified path.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="fileName">
    /// File name for downloaded zip file. Default file name is in format "release_{releaseVersion}.zip"
    /// Lacking .zip -extension will be automatically added.
    /// </param>
    /// <returns>DownloadResult representing download status and information about download.</returns>
    public async Task<DownloadResult<ZipPath>> DownloadLatestReleaseZip(FolderPath path, string? fileName = null)
    {
        var release = await InfoClient.GetLatestRelease();
        if (release.ResponseMessage.NotSuccess())
        {
            Logger.LogError("Failed to load latest release because of '{msg}': '{explanation}'.", 
                release.ResponseMessage, release.Message);
            return new(release.ResponseMessage)
            {
                Message = release.Message
            };
        }
        if (string.IsNullOrWhiteSpace(release.Content?.ZipUrl))
        {
            Logger.LogError("Cannot download release with no download url.");
            return new(ResponseMessage.NoDownloadUrl)
            {
                Message = "Latest release does not have download url."
            };
        }
        if (string.IsNullOrWhiteSpace(fileName))
        {
            string tagName = release.Content.TagName ?? "unknown";
            fileName = $"release_{tagName}.zip";
        }
        ZipPath destination = path.ToFilePath<ZipPath>(fileName);
        return await DownloadFileAsync(release.Content.ZipUrl, destination);
    }
}
