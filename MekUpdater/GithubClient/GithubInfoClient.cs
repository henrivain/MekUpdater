using MekUpdater.GithubClient.ApiResults;
using MekUpdater.GithubClient.DataModel;
using Microsoft.Extensions.Logging;

namespace MekUpdater.GithubClient;

/// <summary>
/// Api client for github api, get information easily about your repositories!
/// </summary>
public class GithubInfoClient : GithubApiClient, IGithubInfoClient, IDisposable
{
    /// <summary>
    /// Initialize new GithubInfoClient with no logging
    /// </summary>
    /// <param name="githubUserName">target repository owner username</param>
    /// <param name="githubRepositoryName">target repository name</param>
    public GithubInfoClient(string githubUserName, string githubRepositoryName)
        : base(githubUserName, githubRepositoryName) { }

    /// <summary>
    /// Initialize new GithubInfoClient with logging
    /// </summary>
    /// <param name="githubUserName">target repository owner username</param>
    /// <param name="githubRepositoryName">target repository name</param>
    /// <param name="logger">logger used for logging</param>
    public GithubInfoClient(
        string githubUserName, string githubRepositoryName, ILogger<GithubApiClient> logger)
        : base(githubUserName, githubRepositoryName, logger) { }

    /// <inheritdoc/>
    public async Task<GithubApiTResult<Release?>> GetLatestRelease()
    {
        var url = $"{BaseAddress}/releases/latest";
        return await GetApiResultAsync<Release?>(url);

    }

    /// <inheritdoc/>
    public async Task<GithubApiTResult<Release[]?>> GetReleases()
    {
        var url = $"{BaseAddress}/releases";
        return await GetApiResultAsync<Release[]?>(url);
    }

    /// <inheritdoc/>
    public async Task<GithubApiTResult<RepositoryInfo?>> GetRepositoryInfo()
    {
        var url = BaseAddress;
        return await GetApiResultAsync<RepositoryInfo?>(url);
    }

    /// <inheritdoc/>
    public async Task<GithubApiTResult<Asset[]?>> GetLatestReleaseAssets()
    {
        var latest = await GetLatestRelease();
        return new GithubApiTResult<Asset[]?>(latest.ResponseMessage)
        {
            Message = latest.Message,
            Content = latest?.Content?.Assets
        };
    }

    /// <summary>
    /// Dispose all managed and unmanaged resources from this GithubInfoClient instance
    /// </summary>
    public override void Dispose()
    {
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}
