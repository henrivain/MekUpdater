using MekUpdater.GithubClient.DataModel;

namespace MekUpdater.GithubClient.ApiResults;

/// <summary>
/// Data structure for github api request for getting latest release assets
/// </summary>
public class LatestAssetsResult : GithubApiResult
{
    /// <summary>
    /// Init new LatestAssetsResult with given response message
    /// </summary>
    /// <param name="responseMessage"></param>
    internal LatestAssetsResult(ResponseMessage responseMessage) : base(responseMessage) { }

    /// <summary>
    /// Init new LatestAssetsResult with given response message and array of asset
    /// </summary>
    /// <param name="responseMessage"></param>
    /// <param name="assets"></param>
    internal LatestAssetsResult(ResponseMessage responseMessage, Asset[] assets) : base(responseMessage)
    {
        Assets = assets;
    }

    /// <summary>
    /// Init new LatestAssetsResult with given response message and string message
    /// </summary>
    /// <param name="responseMessage"></param>
    /// <param name="message"></param>
    internal LatestAssetsResult(ResponseMessage responseMessage, string message) : base(responseMessage, message) { }

    /// <summary>
    /// Assets from github release
    /// </summary>
    public Asset[]? Assets { get; init; }

    /// <summary>
    /// Convert LatestReleaseResult to LatestAssetsResult. (Does lose data) All LatestAssetsResult are filled.
    /// </summary>
    /// <param name="result"></param>
    public static explicit operator LatestAssetsResult(LatestReleaseResult result)
    {
        return new(result.ResponseMessage, result.Message)
        {
            Assets = result?.Release?.Assets
        };
    }
}
