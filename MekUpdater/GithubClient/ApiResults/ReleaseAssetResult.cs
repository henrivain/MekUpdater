#if NET7_0_OR_GREATER

namespace MekUpdater.GithubClient.ApiResults;

/// <summary>
/// Result from request to github releases api
/// </summary>
public class RepositoryApiResponseResult
{
    /// <summary>
    /// Init new RepositoryApiResponseResult, some fields are required
    /// </summary>
    public RepositoryApiResponseResult() { }

    /// <summary>
    /// Status of api response e.g. success, error
    /// </summary>
    public required ResponseMessage ResponseMessage { get; set; }

    /// <summary>
    /// Data from api response, may be null if error occurred
    /// </summary>
    public ResultData? Data { get; set; }
}

#endif
