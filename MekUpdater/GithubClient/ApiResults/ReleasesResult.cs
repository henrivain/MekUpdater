using MekUpdater.GithubClient.DataModel;

namespace MekUpdater.GithubClient.ApiResults;

/// <summary>
/// Get all public releases from github repository
/// </summary>
public class ReleasesResult : GithubApiResult
{
    /// <summary>
    /// Init new ReleasesResult, with given respponse message
    /// </summary>
    internal ReleasesResult(ResponseMessage responseMessage) : base(responseMessage) { }

    /// <summary>
    /// Init new ReleasesResult with given response message and more thorough explanation about status
    /// </summary>
    /// <param name="responseMessage"></param>
    /// <param name="message"></param>
    internal ReleasesResult(ResponseMessage responseMessage, string message) : base(responseMessage, message) { }

    /// <summary>
    /// Init new ReleasesResult with given respponse message and array of releases
    /// </summary>
    /// <exception cref="ArgumentNullException">if releases array is null</exception>
    internal ReleasesResult(ResponseMessage responseMessage, Release[]? releases) : this(responseMessage)
    {
        Releases = releases;
    }

    /// <summary>
    /// Array of releases read and parsed from github api
    /// </summary>
    public Release[]? Releases { get; init; }
}
