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

    /// <summary>
    /// Convert DownloadResult of T to Convert DownloadResult of IFilePath
    /// </summary>
    /// <param name="value"></param>

    public static explicit operator DownloadResult<IFilePath>(DownloadResult<T> value)
    {
        return new(value.ResponseMessage)
        {
            DownloadUrl = value.DownloadUrl,
            Message = value.Message,
            ResultPath = (IFilePath?)value.ResultPath
        };
    }
}
