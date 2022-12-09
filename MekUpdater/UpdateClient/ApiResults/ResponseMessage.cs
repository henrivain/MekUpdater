namespace MekUpdater.GithubClient.ApiResults;
/// <summary>
/// Status for github api response
/// </summary>
public enum ResponseMessage
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    Success,
    Error,
    ServerError,
    None,
    UnknownHttpRequestException,
    ServerTimedOut,
    UriNotAbsolute,
    NetworkError,
    BadUri,
    JsonStringNull,
    UnknownJsonParseException,
    NoValidJsonConverter,
    InvalidJson,
    ResponseJsonNull,
    UnsuccessfulRequest,
    ResponseObjectNull,
    CannotCreateFile,
    CannotReadStream,
    CannotCopyStream,
    NoMatchingAssetName,
    NoProperFileName,
    NoDownloadUrl,
    HttpRequestUnsuccessful,
    ExtractionError,
    CannotCreateDirectory,
    NotImplemented,
    CannotLaunchProcess,
    CannotFindSetup,
    UpdateAlreadyInstalled,
    CannotFindFile,
    InvalidVersion
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
