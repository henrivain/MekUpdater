using MekUpdater.GithubClient.ApiResults;

namespace MekUpdater.GithubClient;

internal interface IGithubRepositoryClient
{
    Task<ReleasesResult> GetReleases();
    Task<LatestReleaseResult> GetLatestRelease();
    Task<RepositoryInfoResult> GetRepositoryInfo();
    Task<LatestAssetsResult> GetLatestReleaseAssets();
}
