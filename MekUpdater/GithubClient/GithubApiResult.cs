using MekUpdater.GithubClient.ApiResults;

namespace MekUpdater.GithubClient;

internal class GithubApiResult
{
    public GithubApiResult() { }

    public GithubApiResult(ResponseMessage responseMessage)
    {
        ResponseMessage = responseMessage;
    }

    public required ResponseMessage ResponseMessage { get; set; }

    public string Message { get; set; } = string.Empty;


}
