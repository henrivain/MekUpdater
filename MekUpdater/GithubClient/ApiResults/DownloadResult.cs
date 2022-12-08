namespace MekUpdater.GithubClient.ApiResults;

/// <summary>
/// Data structure to store information about file download
/// </summary>
public class DownloadResult<T> : FileSystemResult<T> where T : ILocalPath
{
    internal DownloadResult(ResponseMessage responseMessage) : base(responseMessage) { }

    internal DownloadResult(ResponseMessage responseMessage, string message) 
        : base(responseMessage, message) { }

    internal DownloadResult(ResponseMessage responseMessage, string message, T outputLocation, string downloadUrl) 
        : base(responseMessage, message, outputLocation)
    {
        DownloadUrl = downloadUrl;
    }

    /// <summary>
    /// Url address where data was downloaded from
    /// </summary>
    public string DownloadUrl { get; init; } = string.Empty;
}
