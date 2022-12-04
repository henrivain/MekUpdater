using MekUpdater.GithubClient.DataModel;

namespace MekUpdater.GithubClient.ApiResults;

/// <summary>
/// Data stucrure representing data and request status from github repository api
/// </summary>
public class RepositoryInfoResult : GithubApiResult
{
    /// <summary>
    /// Init new LatestReleaseResult with given response message.
    /// </summary>
    /// <param name="responseMessage"></param>
    internal RepositoryInfoResult(ResponseMessage responseMessage) 
        : base(responseMessage) { }

    /// <summary>
    /// Init new RepositoryInfoResult with given response message and repositoryInfo object from github repository
    /// </summary>
    /// <param name="responseMessage"></param>
    /// <param name="repositoryInfo"></param>
    internal RepositoryInfoResult(ResponseMessage responseMessage, RepositoryInfo? repositoryInfo) 
        : this(responseMessage)
    {
        RepositoryInfo = repositoryInfo;
    }

    /// <summary>
    /// Init new RepositoryInfoResult with given response message and more thorough explanation about status
    /// </summary>
    /// <param name="responseMessage"></param>
    /// <param name="message"></param>
    internal RepositoryInfoResult(ResponseMessage responseMessage, string message) 
        : base(responseMessage, message) { }

    /// <summary>
    /// Parsed data from github api
    /// </summary>
    public RepositoryInfo? RepositoryInfo { get; init; }
}
