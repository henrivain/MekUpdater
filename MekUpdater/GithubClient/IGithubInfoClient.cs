using MekUpdater.GithubClient.ApiResults;
using MekUpdater.GithubClient.DataModel;

namespace MekUpdater.GithubClient;

internal interface IGithubInfoClient
{
    Task<GithubApiTResult<Release[]?>> GetReleases();
    Task<GithubApiTResult<Release?>> GetLatestRelease();
    Task<GithubApiTResult<RepositoryInfo?>> GetRepositoryInfo();
    Task<GithubApiTResult<Asset[]?>> GetLatestReleaseAssets();
}
