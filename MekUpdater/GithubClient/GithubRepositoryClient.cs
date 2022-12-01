using MekUpdater.GithubClient.ApiResults;

namespace MekUpdater.GithubClient;

internal class GithubRepositoryClient : IGithubRepositoryClient
{
    public GithubRepositoryClient(string githubUserName, string repositoryName)
    {
        GithubUserName = githubUserName;
        RepositoryName = repositoryName;
    }

    public string GithubUserName { get; }
    public string RepositoryName { get; }

    public Task<LatestReleaseResult> GetLatestRelease()
    {
        throw new NotImplementedException();
    }

    public Task<ReleasesResult> GetReleases()
    {
        throw new NotImplementedException();
    }




}
