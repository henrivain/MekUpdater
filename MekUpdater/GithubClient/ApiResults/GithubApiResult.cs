namespace MekUpdater.GithubClient.ApiResults;

/// <summary>
/// Base class for different kind of Github api request results
/// </summary>
public class UpdaterApiResult
{
    /// <summary>
    /// initialize new UpdaterApiResult with given response message
    /// </summary>
    /// <param name="responseMessage"></param>
    internal UpdaterApiResult(ResponseMessage responseMessage)
    {
        ResponseMessage = responseMessage;
    }

    internal UpdaterApiResult(ResponseMessage responseMessage, string message) : this(responseMessage)
    {
        Message = message;
    }

    /// <summary>
    /// Api request status like "success" or "error"
    /// </summary>
    public virtual ResponseMessage ResponseMessage { get; }

    /// <summary>
    /// String representation of api response state
    /// </summary>
    public virtual string Message { get; init; } = string.Empty;
}
