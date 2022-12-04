namespace MekUpdater.GithubClient.ApiResults;

internal class HttpRequestResult : GithubApiResult, IDisposable
{
    internal HttpRequestResult(ResponseMessage responseMessage) 
        : base(responseMessage) { }

    internal HttpRequestResult(
        ResponseMessage responseMessage, HttpResponseMessage response) 
        : this(responseMessage)
    {
        Response = response;
    }

    public HttpResponseMessage? Response { get; init; }


    public void Dispose()
    {
        Response?.Dispose();
    }


}
