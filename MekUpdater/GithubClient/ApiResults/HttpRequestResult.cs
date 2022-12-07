namespace MekUpdater.GithubClient.ApiResults;

internal class HttpRequestResult : UpdaterApiResult, IDisposable
{
    internal HttpRequestResult(ResponseMessage responseMessage) 
        : base(responseMessage) { }

    internal HttpRequestResult(
        ResponseMessage responseMessage, HttpResponseMessage response) 
        : this(responseMessage)
    {
        Content = response;
    }

    public HttpResponseMessage? Content { get; init; }


    public void Dispose()
    {
        Content?.Dispose();
    }


}
