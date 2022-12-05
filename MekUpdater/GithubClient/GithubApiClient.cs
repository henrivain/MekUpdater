using System.Text.Json;
using MekUpdater.GithubClient.ApiResults;
using MekUpdater.GithubClient.DataModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MekUpdater.GithubClient;

/// <summary>
/// New client for handling requests to github api
/// </summary>
public class GithubApiClient : IDisposable
{
    /// <summary>
    /// Initialize new client to call github api
    /// </summary>
    /// <param name="githubUserName"></param>
    /// <param name="githubRepositoryName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public GithubApiClient(string githubUserName, string githubRepositoryName)
    {
        if (string.IsNullOrWhiteSpace(githubUserName))
            throw new ArgumentNullException(nameof(githubUserName));
        if (string.IsNullOrWhiteSpace(githubRepositoryName))
            throw new ArgumentNullException(nameof(githubUserName));
        BaseAddress = $"https://api.github.com/repos/{githubUserName}/{githubRepositoryName}";

        HttpClient client = new(new HttpClientHandler()
        {
            UseDefaultCredentials = true,
            UseProxy = false
        })
        {
            BaseAddress = new Uri(BaseAddress)
        };
        client.DefaultRequestHeaders.Add("User-Agent", "request");
        Client = client;
    }

    /// <summary>
    /// Initialize new client to call github api. Provided logger will be used.
    /// </summary>
    /// <param name="githubUserName"></param>
    /// <param name="githubRepositoryName"></param>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException">If username or repository name is null or whitspace</exception>
    public GithubApiClient(string githubUserName, string githubRepositoryName, ILogger<GithubApiClient> logger) : this(githubUserName, githubRepositoryName)
    {
        Logger = logger;
        Logger.LogInformation("Initialize new {className} with user name: '{githubUserName}' and repository name: '{githubRepositoryName}'", 
            nameof(GithubApiClient), githubUserName, githubRepositoryName);
    }

    /// <summary>
    /// Base url address to api (includes repo owner name and repo name)
    /// </summary>
    public string BaseAddress { get; }
    private HttpClient Client { get; }
    private ILogger<GithubApiClient> Logger { get; } = NullLogger<GithubApiClient>.Instance;

    /// <summary>
    /// Make request to Github repository's main section and get parsed data about repository
    /// </summary>
    /// <returns>Releases result representing latest release in github repository and request status</returns>
    public virtual async Task<RepositoryInfoResult> GetRepositoryInfo()
    {
        string url = BaseAddress;
        Logger.LogInformation("Make request to '{url}'.", url);

        using var response = await GetResponse(url);
        if (response.ResponseMessage.IsSuccess())
        {
            Logger.LogInformation("Request to '{url}' successful.", url);
            var parsed = await ParseJsonAsync<RepositoryInfo?>(response.Response);
            return new(parsed.ResponseMessage, parsed.Result)
            {
                Message = parsed.Message
            };
        }
        Logger.LogWarning("Request to '{url}' failed because of '{responseMessage}', '{message}'.", 
            url, response.ResponseMessage, response.Message);
        return new(response.ResponseMessage, response.Message);
    }

    /// <summary>
    /// Make request to Github repository's "releases/latest" section and get parsed data
    /// </summary>
    /// <returns>Releases result representing latest release in github repository and request status</returns>
    public virtual async Task<LatestReleaseResult> GetLatestRelease()
    {
        string url = $"{BaseAddress}/releases/latest";
        Logger.LogInformation("Make request to '{url}'.", url);

        using var response = await GetResponse(url);
        if (response.ResponseMessage.IsSuccess())
        {
            Logger.LogInformation("Request to '{url}' successful.", url);
            var parsed = await ParseJsonAsync<Release?>(response.Response);
            return new(parsed.ResponseMessage, parsed.Result)
            {
                Message = parsed.Message
            };
        }
        Logger.LogWarning("Request to '{url}' failed because of '{responseMessage}', '{message}'.",
            url, response.ResponseMessage, response.Message);
        return new(response.ResponseMessage, response.Message);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <returns></returns>
    protected virtual async Task<GithubApiTResult<T?>> GetApiResult<T>(string url)
    {
        using var response = await GetResponse(url);
        if (response.ResponseMessage.IsSuccess())
        {
            var parsed = await ParseJsonAsync<T>(response.Response);
            return new GithubApiTResult<T?>(parsed.ResponseMessage, parsed.Result)
            {
                Message = parsed.Message
            };
        }
        return new GithubApiTResult<T?>(response.ResponseMessage, response.Message);
    }

    /// <summary>
    /// Make request to Github repository's "releases" section and get parsed data
    /// </summary>
    /// <returns>Releases result representing all releases in github repository and request status</returns>
    public virtual async Task<GithubApiTResult<Release[]?>> GetReleases()
    {
        string url = $"{BaseAddress}/releases";
        Logger.LogInformation("Make request to '{url}'.", url);

        using var response = await GetResponse(url);
        if (response.ResponseMessage.IsSuccess())
        {
            Logger.LogInformation("Request to '{url}' successful.", url);
            var parsed = await ParseJsonAsync<Release[]?>(response.Response);
            return new(parsed.ResponseMessage, parsed.Result)
            {
                Message = response.Message,
            };
        }
        Logger.LogWarning("Request to '{url}' failed because of '{responseMessage}', '{message}'.",
            url, response.ResponseMessage, response.Message);
        return new(response.ResponseMessage, response.Message);
    }

    /// <summary>
    /// Get only release assets from latest github release
    /// </summary>
    /// <returns></returns>
    public virtual async Task<LatestAssetsResult> GetLatestReleaseAssets()
    {
        Logger.LogInformation("Try to get release assets from latest release.");
        LatestReleaseResult releaseResult = await GetLatestRelease();
        return (LatestAssetsResult)releaseResult;
    }



    /// <summary>
    /// Parse HttpResponseMessage content into wanted T object.
    /// </summary>
    /// <typeparam name="T">Type of wanted result.</typeparam>
    /// <param name="response">HttpResponseMessage object from http request.</param>
    /// <returns>
    /// Success ResponseMessage with Parsed json as T object if response content valid. 
    /// <para/>Otherwise Result null, ResponseMessage different from success and Message explaining reason for error.
    /// </returns>
    private static async Task<(T? Result, ResponseMessage ResponseMessage, string Message)> ParseJsonAsync<T>(HttpResponseMessage? response)
    {
        if (response is null)
        {
            return new(default, ResponseMessage.ResponseObjectNull, $"Given HttpResponseMessage is null and cannot be parsed into desired type.");
        }
        var json = await response.Content.ReadAsStringAsync();
        var (result, msg) = ParseJson<T>(json);
        if (msg.IsSuccess())
        {
            return new(result, msg, string.Empty);
        }
        if (response.IsSuccessStatusCode is false)
        {
            string message = $"Unsuccesful http request. Status code: '{response.StatusCode}' Request message: '{response.RequestMessage}'.";
            return new(default, ResponseMessage.UnsuccessfulRequest, message); 
        }
        return new(default, msg, "Bad json string.");
    }

    /// <summary>
    /// Parse json string into object with wanted type T
    /// </summary>
    /// <typeparam name="T">Type of wanted result.</typeparam>
    /// <param name="json">Json string.</param>
    /// <returns>ResponseMessage "Success" and Result with parsed data if given json string valid, else ResponseMessage not success and Result null</returns>
    private static (T? Result, ResponseMessage Msg) ParseJson<T>(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return (default, ResponseMessage.JsonStringNull);
        }
        try
        {
            return (JsonSerializer.Deserialize<T>(json), ResponseMessage.Success);
        }
        catch (Exception ex)
        {
            ResponseMessage msg = ex switch
            {
                ArgumentNullException => ResponseMessage.JsonStringNull,
                JsonException => ResponseMessage.InvalidJson,
                NotSupportedException => ResponseMessage.NoValidJsonConverter,
                _ => ResponseMessage.UnknownJsonParseException
            };
            return (default, msg);
        }
    }

    /// <summary>
    /// Get response from given http adress asynchronously. Also handle exceptions and convert them into Response messages and string message
    /// </summary>
    /// <param name="address">url address where request will be made</param>
    /// <returns>HttpRequestResult that represents response data and response status</returns>
    private async Task<HttpRequestResult> GetResponse(string address)
    {
        try
        {
            return new(ResponseMessage.Success)
            {
                Response = await Client.GetAsync(address)
            };
        }
        catch (Exception ex)
        {
            ResponseMessage error = ex switch
            {
                UriFormatException => ResponseMessage.BadUri,
                InvalidOperationException => ResponseMessage.UriNotAbsolute,
                TaskCanceledException => ResponseMessage.ServerTimedOut,
                HttpRequestException => ResponseMessage.NetworkError,
                _ => ResponseMessage.UnknownHttpRequestException
            };
            return new(error)
            {
                Message = $"Operation {nameof(GetResponse)} failed because of {ex.GetType()}: {ex.Message}.\n"
                    + $"Used Uri was {address}",
                Response = null
            };
        }
    }

    /// <summary>
    /// Dispose all managed and unmanaged resources from this GithubApiClient instance
    /// </summary>
    public virtual void Dispose()
    {
        Client?.Dispose();
        GC.SuppressFinalize(this);
    }
}

