namespace MekUpdater.GithubClient.ApiResults;

/// <summary>
/// Data structure to store information about download file
/// </summary>
public class DownloadResult : GithubApiResult
{
    /// <summary>
    /// Init new GithubApiResult of T with given response message
    /// </summary>
    internal DownloadResult(ResponseMessage responseMessage) : base(responseMessage) { }

    /// <summary>
    /// Init new GithubApiResult of T with given response message and more thorough explanation about status
    /// </summary>
    /// <param name="responseMessage"></param>
    /// <param name="message"></param>
    internal DownloadResult(ResponseMessage responseMessage, string message) : base(responseMessage, message) { }

    /// <summary>
    /// Init new GithubApiResult of T with given response message and more thorough explanation about status
    /// </summary>
    /// <param name="responseMessage"></param>
    /// <param name="message"></param>
    /// <param name="outputLocation"></param>
    internal DownloadResult(ResponseMessage responseMessage, string message, ILocalPath outputLocation) : base(responseMessage, message)
    {
        OutputLocation = outputLocation;
    }

    /// <summary>
    /// Location where file(s) where downloaded
    /// </summary>
    public ILocalPath? OutputLocation { get; init; }
}
