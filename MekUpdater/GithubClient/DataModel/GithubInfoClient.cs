using MekUpdater.GithubClient.ApiResults;
using Microsoft.Extensions.Logging;

namespace MekUpdater.GithubClient.DataModel;

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
        : base (githubUserName, githubRepositoryName, logger) { }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<GithubApiTResult<Release?>> GetLatestRelease()
    {
        string url = $"{BaseAddress}/releases/latest";
        return await GetApiResult<Release?>(url);

    }



    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<GithubApiTResult<Release[]?>> GetReleases()
    {
        string url = $"{BaseAddress}/releases";
        return await GetApiResult<Release[]?>(url);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<GithubApiTResult<RepositoryInfo?>> GetRepositoryInfo()
    {
        string url = BaseAddress;
        return await GetApiResult<RepositoryInfo?>(url);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
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
