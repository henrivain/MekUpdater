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
    /// Init new ReleasesResult, with given respponse message and array of releases
    /// </summary>
    /// <exception cref="ArgumentNullException">if releases array is null</exception>
    internal ReleasesResult(ResponseMessage responseMessage, Release[]? releases) : this(responseMessage)
    {
        Releases = releases ?? throw new ArgumentNullException(nameof(releases));
    }

    /// <summary>
    /// Array of releases read and parsed from github api
    /// </summary>
    public Release[]? Releases { get; init; }
}
