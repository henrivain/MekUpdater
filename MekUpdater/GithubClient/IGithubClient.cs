#if NET7_0_OR_GREATER

using MekUpdater.GithubClient.ApiResults;

namespace MekUpdater.GithubClient;

internal interface IGithubClient
{
    Task<UpdateCheckResult> GetVersionData();

    Task<RepositoryApiResponseResult> GetFullApiResponse();

}
#endif
