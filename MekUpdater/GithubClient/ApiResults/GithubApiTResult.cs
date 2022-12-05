namespace MekUpdater.GithubClient.ApiResults;

/// <summary>
/// Datatype for different kinds of Github api result 
/// </summary>
/// <typeparam name="T">Github api response data type (from data models)</typeparam>
public class GithubApiTResult<T> : GithubApiResult
{
    /// <summary>
    /// Init new GithubApiResult of T with given response message
    /// </summary>
    internal GithubApiTResult(ResponseMessage responseMessage) : base(responseMessage) { }

    /// <summary>
    /// Init new GithubApiResult of T with given response message and more thorough explanation about status
    /// </summary>
    /// <param name="responseMessage"></param>
    /// <param name="message"></param>
    internal GithubApiTResult(ResponseMessage responseMessage, string message) : base(responseMessage, message) { }

    /// <summary>
    /// Init new GithubApiResult of T with given response message and content of T
    /// </summary>
    /// <exception cref="ArgumentNullException">if releases array is null</exception>
    internal GithubApiTResult(ResponseMessage responseMessage, T? content) : this(responseMessage)
    {
        Content = content;
    }

    /// <summary>
    /// Data requested and parsed from github api
    /// </summary>
    public T? Content { get; init; }
}
