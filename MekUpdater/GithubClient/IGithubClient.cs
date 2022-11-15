using MekUpdater.GithubClient.ApiResults;

namespace MekUpdater.GithubClient;

internal interface IGithubClient
{
    Task<UpdateCheckResult> GetVersionData();

    Task<RepositoryApiResponseResult> GetFullApiResponse();

    Task<ReleasesApiResponseResult> MakeRequestToReleases();
}
