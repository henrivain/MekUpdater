namespace MekUpdater.GithubClient.ApiResults;

/// <summary>
/// Data structure to store information about file download
/// </summary>
public class DownloadResult : UpdaterApiResult
{
    /// <summary>
    /// Init new DownloadResultwith given response message
    /// </summary>
    internal DownloadResult(ResponseMessage responseMessage) : base(responseMessage) { }

    /// <summary>
    /// Init new DownloadResult with given response message and more thorough explanation about status
    /// </summary>
    /// <param name="responseMessage"></param>
    /// <param name="message"></param>
    internal DownloadResult(ResponseMessage responseMessage, string message) 
        : base(responseMessage, message) { }

    /// <summary>
    /// Init new DownloadResult with given response message, 
    /// more thorought explanation, output location and download url. 
    /// </summary>
    /// <param name="responseMessage"></param>
    /// <param name="message"></param>
    /// <param name="outputLocation"></param>
    /// <param name="downloadUrl"></param>
    internal DownloadResult(
        ResponseMessage responseMessage, string message, ILocalPath outputLocation, string downloadUrl) 
        : base(responseMessage, message)
    {
        OutputLocation = outputLocation;
        DownloadUrl = downloadUrl;
    }

    /// <summary>
    /// Location where file(s) where downloaded
    /// </summary>
    public ILocalPath? OutputLocation { get; init; }

    /// <summary>
    /// Url address where data was downloaded from
    /// </summary>
    public string DownloadUrl { get; init; } = string.Empty;
}
