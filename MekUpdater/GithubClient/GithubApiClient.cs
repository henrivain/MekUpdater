using System.Net;
using System.Text.Json;
using MekUpdater.GithubClient.ApiResults;
using MekUpdater.GithubClient.FileManager;
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
    private protected GithubApiClient(string githubUserName, string githubRepositoryName)
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
    private protected GithubApiClient(
        string githubUserName, string githubRepositoryName, ILogger<GithubApiClient> logger)
        : this(githubUserName, githubRepositoryName)
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
    private protected ILogger<GithubApiClient> Logger { get; } = NullLogger<GithubApiClient>.Instance;

    /// <summary>
    /// Download file form given url into given file pat. Path will be created or old data will be overritten.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="path"></param>
    /// <returns>DownloadResult representing download status and used data.</returns>
    private protected virtual async Task<DownloadResult> DownloadFileAsync(string url, IFilePath path)
    {
        Logger.LogInformation("Download file from '{url}' to '{path}'", url, path.FullPath);
        FileHandler fileHandler = new(path, Logger);
        var (isPathValid, pathCreationMessage) = fileHandler.TryCreateDirectory();
        if (isPathValid is false)
        {
            Logger.LogError("Cannot create required path '{path}', because of '{reason}'", path.FullPath, pathCreationMessage);
            return new(ResponseMessage.CannotCreateFile)
            {
                Message = pathCreationMessage,
                DownloadUrl = url
            };
        }

        Logger.LogInformation("Starting download from {url}", url);
        using var response = await GetResponse(url);
        Logger.LogInformation("Download ready");
        if (response.ResponseMessage.NotSuccess())
        {
            Logger.LogError("Http request to '{url}' failed, because of '{reason}'", url, response.Message);
            return new(response.ResponseMessage)
            {
                Message = response.Message,
                DownloadUrl = url
            };
        }
        if (response.Content.NotSuccess())
        {
            HttpStatusCode? status = response.Content?.StatusCode;
            HttpRequestMessage? message = response.Content?.RequestMessage;
            Logger.LogError("Http request to url '{url}' was not successfull or response was null. " +
                "Status: '{status}', Message: '{msg}'", url, status, message);
            return new(ResponseMessage.UnsuccessfulRequest)
            {
                Message = $"Http request returned empty content with status: '{status}' and message: '{message}'"
            };

        }

        using Stream? stream = await response.Content!.Content.ReadAsStreamAsync();
        var (copySuccess, copyMessage) = await fileHandler.CopyStreamAsync(stream);

        if (copySuccess is false)
        {
            Logger.LogInformation("Failed to copy data from '{url}' into file '{file}' with message '{message}'",
                url, path.FullPath, copyMessage);
            return new(ResponseMessage.CannotCopyStream, copyMessage, path, url);
        }
        Logger.LogInformation("Successfully copied data from '{url}' into file '{file}'", url, path.FullPath);
        return new(ResponseMessage.Success, string.Empty, path, url);
    }

    /// <summary>
    /// Make request to github api and parse result into T type
    /// </summary>
    /// <typeparam name="T">
    /// Type of response data, 
    /// it is used when parsing from json
    /// </typeparam>
    /// <param name="url">
    /// Full url path to Github api endpoint
    /// </param>
    /// <returns>
    /// GithubApiTResult of given type.
    /// This result represents response data and request status
    /// </returns>
    private protected virtual async Task<GithubApiTResult<T?>> GetApiResultAsync<T>(string url)
    {
        Logger.LogInformation("Sending request to api endpoint at '{url}'.", url);
        using var response = await GetResponse(url);
        if (response.ResponseMessage.IsSuccess())
        {
            var parsed = await ParseJsonAsync<T>(response.Content);
            if (parsed.ResponseMessage.IsSuccess())
            {
                Logger.LogInformation("Request to '{url}' was successful.", url);
            }
            else
            {
                Logger.LogError("Parsing github api response json into '{type}' failed " +
                    "because of '{reason}': '{explanation}'.", typeof(T), parsed.ResponseMessage, parsed.Message);
            }
            return new GithubApiTResult<T?>(parsed.ResponseMessage, parsed.Result)
            {
                Message = parsed.Message
            };
        }
        Logger.LogError("Github api request failed. Reason: '{reason}': '{explanation}'",
            response.ResponseMessage, response.Message);
        return new GithubApiTResult<T?>(response.ResponseMessage, response.Message);
    }

    /// <summary>
    /// Get response from given http adress asynchronously. 
    /// Also handle exceptions and convert them into Content messages and string message
    /// <para/>!! DOES NOT HANDLE BAD REQUEST 
    /// (RETURNS SUCCESS EVEN IF STATUS CODE IS FOR EXAMPLE '404', NO EXCEPTIONS WILL BE THROWN) !!
    /// </summary>
    /// <param name="address">url address where request will be made</param>
    /// <returns>HttpRequestResult that represents response data and response status</returns>
    private protected virtual async Task<HttpRequestResult> GetResponse(string address)
    {
        try
        {
            return new(ResponseMessage.Success)
            {
                Content = await Client.GetAsync(address)
            };
        }
        catch (Exception ex)
        {
            Logger.LogError("Http request to {url} failed because of '{ex}': '{msg}'",
                address, ex.GetType(), ex.Message);
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
                Message = $"Http request to '{address}' failed because of " +
                    $"{ex.GetType()}: {ex.Message}.",
                Content = null
            };
        }
    }

    /// <summary>
    /// Parse HttpResponseMessage content into wanted T object. 
    /// Also checks if request was successful.
    /// </summary>
    /// <typeparam name="T">Type of wanted result.</typeparam>
    /// <param name="response">HttpResponseMessage object from http request.</param>
    /// <returns>
    /// Success ResponseMessage with Parsed json as T object if response content valid. 
    /// <para/>Otherwise Result null, ResponseMessage different from success and Message explaining reason for error.
    /// </returns>
    private protected static async Task<(T? Result, ResponseMessage ResponseMessage, string Message)> ParseJsonAsync<T>(
        HttpResponseMessage? response)
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
    private protected static (T? Result, ResponseMessage Msg) ParseJson<T>(string? json)
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
    /// Dispose all managed and unmanaged resources from this GithubApiClient instance
    /// </summary>
    public virtual void Dispose()
    {
        Client?.Dispose();
        GC.SuppressFinalize(this);
    }
}

