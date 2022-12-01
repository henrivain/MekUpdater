using MekUpdater.GithubClient.DataModel;

namespace MekUpdater.GithubClient.ApiResults;


/// <summary>
/// Result of Github api response that consist of latest release object and api response status
/// </summary>
public class LatestReleaseResult : GithubApiResult
{
    /// <summary>
    /// Init new LatestReleaseResult, with given respponse message
    /// </summary>
    internal LatestReleaseResult(ResponseMessage responseMessage) : base(responseMessage) { }

    /// <summary>
    /// Init new LatestReleaseResult, with given response message and release object from github repository
    /// </summary>
    /// <param name="responseMessage"></param>
    /// <param name="release"></param>
    internal LatestReleaseResult(ResponseMessage responseMessage, Release? release) : this(responseMessage)
    {
        Release = release;
    }

    /// <summary>
    /// Releases read and parsed from github repository
    /// </summary>
    public Release? Release { get; init; }
}
