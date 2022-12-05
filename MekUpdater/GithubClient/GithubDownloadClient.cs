using MekUpdater.GithubClient.ApiResults;
using MekUpdater.GithubClient.DataModel;
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
    public async Task<DownloadResult> DownloadReleaseZip(VersionTag tag, FolderPath destinationFolder)
    {
        string url = $"{BaseAddress}/zipball/{tag.FullVersion}";
        var path = destinationFolder.ToFilePath<ZipPath>($"{tag.FullVersion}.zip");
        
        Logger.LogInformation("Download file from '{url}' to '{path}'", url, path);
        
        return await DownloadFileAsync(url, path);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="path"></param>
    /// <param name="assetName"></param>
    /// <param name="onlyFullMatch"></param>
    /// <returns></returns>
    public async Task<DownloadResult> DownloadAsset(VersionTag tag, FolderPath path, string assetName, bool onlyFullMatch = false)
    {
        var release = await InfoClient.GetRelease(tag);
        if (release.ResponseMessage.NotSuccess())
        {
            return new(release.ResponseMessage, release.Message);
        }

        Func<Asset, bool> validationAction = x => x.Name?.Contains(assetName ?? string.Empty) is true;
        if (onlyFullMatch)
        {
            validationAction = x => x.Name == assetName;
        }
        
        var requestedAsset = release.Content?.Assets.FirstOrDefault(validationAction);
        if (requestedAsset is null) 
        { 
            return new(ResponseMessage.NoMatchingAssetName, $"Cannot download asset, because selected release does not have asset with matching name '{assetName}'.");
        }
        if (requestedAsset.DownloadUrl is null)
        {
            return new(ResponseMessage.NoDownloadUrl, $"Cannot download asset with no download url.");

        }
        if (string.IsNullOrWhiteSpace(requestedAsset.Name))
        {
            return new(ResponseMessage.NoProperFileName, $"Cannot download asset with no file name.");
        }

        FilePath downloadPath = path.ToFilePath<FilePath>(requestedAsset.Name);
        return await DownloadFileAsync(requestedAsset.DownloadUrl, downloadPath);
    }


}
