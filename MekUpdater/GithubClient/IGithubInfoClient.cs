using MekUpdater.GithubClient.ApiResults;
using MekUpdater.GithubClient.DataModels;

namespace MekUpdater.GithubClient;

/// <summary>
/// Implementation instructions for github api client 
/// that can check information about repository
/// </summary>
public interface IGithubInfoClient : IDisposable
{
    /// <summary>
    /// Make request to Github api and get all releases from 
    /// repository's "releases" section.
    /// </summary>
    /// <returns>
    /// GithubApiTResult representing response data and request status
    /// </returns>
    Task<GithubApiTResult<Release[]?>> GetReleases();

    /// <summary>
    /// Make request to Github api and get latest release from "releases" section
    /// </summary>
    /// <returns>
    /// GithubApiTResult representing response data and request status
    /// </returns>
    Task<GithubApiTResult<Release?>> GetLatestRelease();

    /// <summary>
    /// Make request to Github repository's main section and get parsed data about repository
    /// </summary>
    /// <returns>
    /// GithubApiTResult representing github repository and request status
    /// </returns>
    Task<GithubApiTResult<RepositoryInfo?>> GetRepositoryInfo();

    /// <summary>
    /// Get latest release from github api and get assets from it.
    /// </summary>
    /// <returns>
    /// GithubApiTResult representing release assets in latest release and request status.
    /// </returns>
    Task<GithubApiTResult<Asset[]?>> GetLatestReleaseAssets();

    /// <summary>
    /// Get release byt given version tag.
    /// (github release tag must be format "vX.X.X" where X is any uint.)
    /// </summary>
    /// <param name="tag"></param>
    /// <returns>
    /// GithubApiTResult representing release assets in latest release and request status
    /// </returns>
    Task<GithubApiTResult<Release?>> GetRelease(VersionTag tag);
}
